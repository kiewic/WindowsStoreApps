using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SimpleBlogEditor
{
    class ContentBuilder
    {
        static public string RemoveExtraIdentation(string text)
        {
            // Replace tabs with spaces.
            text = text.Replace("\t", "    ");

            // Split in lines.
            string[] lines = text.Split(new char[] { '\n' });

            // Count minimum spaces are in all lines.
            int minSpacesCount = int.MaxValue;
            foreach (string line in lines)
            {
                // Empty lines does not count.
                if (String.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                // Count spaces in this line.
                int localSpacesCount = 0;
                foreach (char c in line.ToCharArray())
                {
                    if (c != ' ')
                    {
                        break;
                    }
                    localSpacesCount++;
                }

                // Keep the smaller count.
                if (localSpacesCount < minSpacesCount)
                {
                    minSpacesCount = localSpacesCount;
                }
            }

            // Remove minSpacesCount from each line.
            for (int i = 0; i < lines.Length; i++)
            {
                // Empty lines may not be enough big.
                if (lines[i].Length > minSpacesCount)
                {
                    lines[i] = lines[i].Substring(minSpacesCount);
                }
            }

            // Join lines.
            return String.Join("\n", lines);
        }

        static public string AddIdentation(string text)
        {
            // Replace tabs with spaces.
            text = text.Replace("\t", "    ");

            // Split in lines.
            string[] lines = text.Split(new char[] { '\n' });

            // Add identation to each line.
            for (int i = 0; i < lines.Length; i++)
            {
                // Do not ident empty lines.
                if (lines[i].Length != 0)
                {
                    lines[i] = "    " + lines[i];
                }
            }

            // Join lines.
            return String.Join("\n", lines);
        }

        static public PreTag GetPreTag(TextBox textBox)
        {
            PreTag preTag = new PreTag();
            preTag.ComesFromFile = false;

            Debug.WriteLine(textBox.SelectionStart);

            int lineFeeds = CountLineFeedsBeforeSelection(textBox.Text, textBox.SelectionStart);

            if (textBox.SelectedText.Length > 0)
            {
                preTag.Start = textBox.Text.IndexOf("<pre>", textBox.SelectionStart + lineFeeds);
            }

            // If TextBox.SelectedText is zero, the current cursor position is textBox.SelectionStart.
            if (preTag.Start < 0)
            {
                preTag.Start = textBox.Text.LastIndexOf("<pre>", textBox.SelectionStart + lineFeeds);
            }

            if (preTag.Start >= 0)
            {
                // If the is a <pre>, look for the next </pre>.
                preTag.End = textBox.Text.IndexOf("</pre>", preTag.Start);
            }

            if (preTag.Start < 0 || preTag.End < 0)
            {
                // <pre> or </pre> not found.
                return null;
            }

            preTag.Text = textBox.Text.Substring(preTag.Start, preTag.Length);
            return preTag;
        }

        static private int CountLineFeedsBeforeSelection(string text, int selectionStart)
        {
            int count = 0;
            int index = 0;
            while (index < selectionStart)
            {
                index = text.IndexOf("\r\n", index);
                if (index != -1)
                {
                    index += 2;
                    count++;
                }
                else
                {
                    break;
                }
            }
            return count;
        }

        public static string PreToText(string preString)
        {
            if (String.IsNullOrEmpty(preString))
            {
                return String.Empty;
            }

            // 5 for <pre>
            // 11 for <pre> and </pre>
            preString = preString.Substring(5, preString.Length - 11);
            return WebUtility.HtmlDecode(preString);
        }

        public static string TextToPre(string text)
        {
            return "<pre>" + WebUtility.HtmlEncode(text) + "</pre>";
        }
    }

    public class PreTag
    {
        public PreTag()
        {
            Start = -1;
            End = -1;
        }

        public int Start
        {
            get;
            set;
        }

        public int End
        {
            get;
            set;
        }

        public int Length
        {
            get
            {
                return End - Start + 6; // 6 to include </pre>
            }
        }

        public bool ComesFromFile
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }
    }
}
