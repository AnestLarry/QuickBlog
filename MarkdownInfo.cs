﻿using Markdig;

namespace QuickBlog
{
	public class MarkdownInfo
	{
		private string title;
		private string content;
		private DateTime date;
		private List<string> tags;
		private List<string> categories;
		private string template;
		private int? top;

		public string Title { get => title; set => title = value; }
		public List<string> Categories { get => categories; set => categories = value; }
		public string Content { get => content; set => content = value; }
		public int Top { get => top == null ? 0 : top.Value; set => top = value; }
		public string Description { get => content.Substring(0, Content.Length < 200 ? Content.Length : 200); }
		public string ContentHTML { get => Markdown.ToHtml(content); }
		public string Template { get => template; set => template = value; }
		public string Location { get => $"{Date.Year}/{Date.Month}"; }
		public string URL { get => $"{Location}/{getValidPath(Title)}.html"; }
		public DateTime Date { get => date; set => date = value; }
		public List<string> Tags { get => tags; set => tags = value; }

		private bool FillWithPair(string[] pros)
		{
			switch (pros[0])
			{
				case "title":
					Title = pros[1].Trim();
					return true;
				case "date":
					Date = DateTime.ParseExact(
						pros[1].Trim(),
						"yyyy-MM-dd HH:mm:ss",
						System.Globalization.CultureInfo.InvariantCulture);
					return true;
				case "tags":
					Tags = pros[1].Split(",").Select(x => x.Trim()).Where(x => x.Length > 1).ToList();
					return true;
				case "categories":
					Categories = pros[1].Split(",").Select(x => x.Trim()).Where(x => x.Length > 1).ToList();
					return true;
				case "template":
					Template = pros[1].Trim();
					return true;
				case "top":
					Top = int.Parse(pros[1].Trim());
					return true;
				default:
					return false;
			}
		}

		public bool FillWithBlock(IEnumerable<string> values)
		{
			foreach (string value in values)
			{
				string[] pros = new string[] { value.Substring(0, value.IndexOf(":")), value.Substring(value.IndexOf(":") + 1, value.Length - 1 - value.IndexOf(":")) };
				FillWithPair(pros);
			}
			return true;
		}

		public override string? ToString()
		{
			return "{title:\"" + Title + "\"},{template:\"" + Template + "\"},{date:\"" + Date + "\"},{tags:\"" + Tags + "\"},{top:\"" + Top + "\"}";
		}

		private string getValidPath(string unsafePath)
		{
			char[] invalidChars = Path.GetInvalidPathChars();

			foreach (char c in invalidChars)
			{
				unsafePath = unsafePath.Replace(c.ToString(), string.Empty);
			}
			return unsafePath;
		}
	}
}