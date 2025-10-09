using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime; // <- Keep Ink package imported
using IOPath = System.IO.Path;
namespace DialogueSystem
{
    public class NarrativePlayer : MonoBehaviour
    {
        public enum NarrativeMode
        {
            Auto,       // Prefer Ink if inkJSONAsset is assigned, otherwise Twee
            Ink,
            Twee
        }

        [Header("Mode")]
        public NarrativeMode mode = NarrativeMode.Auto;

        [Header("UI Components")]
        public TextMeshProUGUI passageText;
        public Button choiceButtonPrefab;          // Button with a TMP child for its label
        public Transform choiceButtonContainer;
        public Transform imageContainer;
        public Image imagePrefab;

        [Header("Choice Counter")]
        public TextMeshProUGUI myChoiceCounterUI;

        [Header("Twee Source (local text file)")]
        public string localFolder = "StreamingAssets/MyStories";
        public string localFileName = "story.twee"; // .twee / .tw2 / .txt
        private TweeParser tweeParser;
        private Dictionary<string, TweeParser.Passage> passages;
        private string currentPassageTitle;

        [Header("Ink Source (compiled JSON)")]
        [SerializeField] private TextAsset inkJSONAsset = null;
        public Story story;

        public string imageFolder = "StreamingAssets/MyStories";
        private int myChoices = 0;

        void Start()
        {
            // Decide mode
            var resolvedMode = mode;
            if (resolvedMode == NarrativeMode.Auto)
            {
                resolvedMode = inkJSONAsset != null ? NarrativeMode.Ink : NarrativeMode.Twee;
            }

            // Initialize per-mode
            if (resolvedMode == NarrativeMode.Ink)
            {
                StartInk();
            }
            else
            {
                StartCoroutine(StartTwee());
            }
        }

        #region ==== Shared UI helpers ====

        void ClearChoices()
        {
            if (!choiceButtonContainer) return;
            for (int i = choiceButtonContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(choiceButtonContainer.GetChild(i).gameObject);
            }
        }

        void ClearImages()
        {
            if (!imageContainer) return;
            for (int i = imageContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(imageContainer.GetChild(i).gameObject);
            }
        }

        Button AddChoiceButton(string label, Action onClick)
        {
            var btn = Instantiate(choiceButtonPrefab, choiceButtonContainer);
            var tmp = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp) tmp.text = label;
            btn.onClick.AddListener(() =>
            {
                myChoices += 1;
                if (myChoiceCounterUI) myChoiceCounterUI.text = "Choices made: " + myChoices;
                onClick?.Invoke();
            });
            return btn;
        }

        IEnumerator LoadImage(string imageFileName)
        {
            if (imagePrefab == null || imageContainer == null || string.IsNullOrEmpty(imageFileName))
                yield break;

            // Build local path
            string imagePath = IOPath.Combine(Application.dataPath, IOPath.Combine(imageFolder, imageFileName));

            if (!File.Exists(imagePath))
            {
                Debug.LogError("Image file not found: " + imagePath);
                yield break;
            }

            byte[] imageBytes = File.ReadAllBytes(imagePath);
            var texture = new Texture2D(2, 2);
            if (!texture.LoadImage(imageBytes))
            {
                Debug.LogError("Failed to load image: " + imagePath);
                yield break;
            }

            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            var img = Instantiate(imagePrefab, imageContainer);
            img.sprite = sprite;
            img.gameObject.SetActive(true);
        }

        #endregion

        #region ==== TWEE FLOW ====

        IEnumerator StartTwee()
        {
            tweeParser = new TweeParser();
            string filePath = IOPath.Combine(Application.dataPath, IOPath.Combine(localFolder, localFileName));

            if (!File.Exists(filePath))
            {
                Debug.LogError("Twee file not found: " + filePath);
                yield break;
            }

            string text = File.ReadAllText(filePath, Encoding.UTF8);
            passages = tweeParser.ParseTweeFileFromText(text);
            if (passages == null || passages.Count == 0)
            {
                Debug.LogError("No passages parsed from Twee.");
                yield break;
            }

            // Start at "Start" if exists, otherwise first passage
            if (passages.ContainsKey("Start"))
            {
                DisplayTweePassage("Start");
            }
            else
            {
                // Fallback: arbitrary first passage
                foreach (var kv in passages)
                {
                    DisplayTweePassage(kv.Key);
                    break;
                }
            }

            yield return null;
        }

        void DisplayTweePassage(string passageTitle)
        {
            if (passages == null || !passages.TryGetValue(passageTitle, out var passage))
            {
                Debug.LogError("Passage not found: " + passageTitle);
                return;
            }

            ClearChoices();
            ClearImages();

            currentPassageTitle = passageTitle;
            if (passageText) passageText.text = passage.Body;

            // Choices
            foreach (var choice in passage.Choices)
            {
                // Expecting [[Label|Target]]
                var start = choice.IndexOf("[[", StringComparison.Ordinal);
                var end = choice.IndexOf("]]", StringComparison.Ordinal);
                if (start >= 0 && end > start + 2)
                {
                    var inner = choice.Substring(start + 2, end - (start + 2));
                    var parts = inner.Split('|');
                    if (parts.Length == 2)
                    {
                        string label = parts[0].Trim();
                        string target = parts[1].Trim();
                        AddChoiceButton(label, () => DisplayTweePassage(target));
                    }
                }
            }

            // Images (filenames)
            foreach (var img in passage.Images)
            {
                StartCoroutine(LoadImage(img));
            }
        }

        #endregion

        #region ==== INK FLOW ====

        void StartInk()
        {
            if (inkJSONAsset == null)
            {
                Debug.LogError("Ink mode selected but no ink JSON assigned.");
                return;
            }

            story = new Story(inkJSONAsset.text);
            RefreshInkView();
        }

        void RefreshInkView()
        {
            if (story == null) return;

            ClearChoices();
            ClearImages();

            // Collect all content lines this step
            var sb = new StringBuilder();
            while (story.canContinue)
            {
                string line = story.Continue();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    sb.AppendLine(line.Trim());
                }

                // Handle tags per line (optional pattern: image: filename.png)
                if (story.currentTags != null)
                {
                    foreach (var tag in story.currentTags)
                    {
                        ParseInkTag(tag);
                    }
                }
            }

            if (passageText) passageText.text = sb.ToString().Trim();

            // Choices
            if (story.currentChoices != null && story.currentChoices.Count > 0)
            {
                for (int i = 0; i < story.currentChoices.Count; i++)
                {
                    var c = story.currentChoices[i];
                    AddChoiceButton(c.text.Trim(), () =>
                    {
                        story.ChooseChoiceIndex(c.index);
                        RefreshInkView();
                    });
                }
            }
            else
            {
                // End reached
                AddChoiceButton("End of story. Restart?", () =>
                {
                    story.ResetState();
                    myChoices = 0;
                    if (myChoiceCounterUI) myChoiceCounterUI.text = "Choices made: " + myChoices;
                    RefreshInkView();
                });
            }
        }

        void ParseInkTag(string tag)
        {
            // Example accepted formats:
            // "image: mypic.png"
            // "image=mypic.png"
            // "img: filename.webp"
            if (string.IsNullOrWhiteSpace(tag)) return;

            var t = tag.Trim();
            var lower = t.ToLowerInvariant();

            if (lower.StartsWith("image:") || lower.StartsWith("image=") || lower.StartsWith("img:") || lower.StartsWith("img="))
            {
                var sep = t.IndexOfAny(new[] { ':', '=' });
                if (sep >= 0 && sep < t.Length - 1)
                {
                    var filename = t.Substring(sep + 1).Trim();
                    if (!string.IsNullOrEmpty(filename))
                    {
                        StartCoroutine(LoadImage(filename));
                    }
                }
            }
        }

        #endregion
    }
}
