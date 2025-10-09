using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DialogueSystem
{
    public class TweeParser
    {
        public class Passage
        {
            public string Title;
            public string[] Tags;
            public string Body;
            public List<string> Images;
            public List<string> Choices;
        }

        public Dictionary<string, Passage> ParseTweeFile(string filePath)
        {
            var text = File.ReadAllText(filePath);
            return ParseTweeFileFromText(text);
        }
        
        public Dictionary<string, Passage> ParseTweeFileFromText(string text)
        {
            var passages = new Dictionary<string, Passage>();

            // Regex to match passages in the Twee file
            var passageRegex = new Regex(@"::\s*(?<title>[^\n]+)\s*(\[(?<tags>[^\]]+)\])?\s*\n(?<body>.*?)(?=\n::\s*|$)", RegexOptions.Singleline);
            var imageRegex = new Regex(@"\[\[Image:(?<path>[^\]]+)\]\]");
            var choiceRegex = new Regex(@"\[\[(?<choiceText>[^\|]+)\|(?<choiceTarget>[^\]]+)\]\]");

            var matches = passageRegex.Matches(text);
            foreach (Match match in matches)
            {
                var title = match.Groups["title"].Value.Trim();
                var tags = match.Groups["tags"].Value?.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                var body = match.Groups["body"].Value.Trim();

                var images = new List<string>();
                var choices = new List<string>();
                var imageMatches = imageRegex.Matches(body);
                foreach (Match imageMatch in imageMatches)
                {
                    images.Add(imageMatch.Groups["path"].Value.Trim());
                    body = body.Replace(imageMatch.Value, ""); // Remove image tag from body
                }

                var choiceMatches = choiceRegex.Matches(body);
                foreach (Match choiceMatch in choiceMatches)
                {
                    choices.Add(choiceMatch.Value.Trim());
                    body = body.Replace(choiceMatch.Value, ""); // Remove choice tag from body
                }

                passages[title] = new Passage
                {
                    Title = title,
                    Tags = tags ?? new string[0],
                    Body = body,
                    Images = images,
                    Choices = choices
                };
            }

            return passages;
        }
    }
}