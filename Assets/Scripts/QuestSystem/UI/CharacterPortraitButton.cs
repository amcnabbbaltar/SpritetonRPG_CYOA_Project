using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class CharacterPortraitButton : MonoBehaviour
{
    [SerializeField] private Image portraitImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject selectionHighlight;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Setup(string characterName, Sprite portrait, Action onClick)
    {
        if (nameText != null) nameText.text = characterName;
        if (portraitImage != null && portrait != null) portraitImage.sprite = portrait;
        button.onClick.AddListener(() => onClick());
    }

    public void SetSelected(bool selected)
    {
        if (selectionHighlight != null)
            selectionHighlight.SetActive(selected);
    }
}
