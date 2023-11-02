﻿using Fluid;
using System.Text.RegularExpressions;

namespace QuickBlog
{
	class QuickBlog
	{
		Dictionary<string, IFluidTemplate> templates = new Dictionary<string, IFluidTemplate>();
		FluidParser parser = new FluidParser();
		List<MarkdownInfo> markdownInfos = new List<MarkdownInfo>();
		Blog templateCtx = new Blog("66ws");
		public void main(string[] args)
		{
			loadTemplates();
			loadMarkdowns();
			render();
		}
		private void loadTemplates()
		{
			foreach (var t in Directory.GetFiles("templates"))
			{
				if (parser.TryParse(File.ReadAllText(t), out var template, out var error))
				{
					templates.Add(Regex.Match(t, "([^\\\\]+)\\.").Groups[1].Value, template);
				}
				else
				{
					Console.WriteLine($"Error: {error}");
				}
			};
		}
		private void loadMarkdowns()
		{
			if (!Directory.Exists("src")) throw new Exception("`src` folder is not existed!");
			loadMarkdownsFromFolder("src");
			var folders = new Queue<string>(Directory.GetDirectories("src"));
			while (folders.Count > 0)
			{
				var folder = folders.Dequeue();
				loadMarkdownsFromFolder(folder);
				foreach (var f in Directory.GetDirectories(folder))
				{
					folders.Enqueue(f);
				};
			}
		}
		private void loadMarkdownsFromFolder(string folder)
		{
			foreach (string md in Directory.GetFiles(folder).Where(x => x.ToLower().EndsWith(".md")))
			{
				string[] lines = File.ReadAllLines(md);
				MarkdownInfo markdownInfo = new MarkdownInfo();
				int metaEndIndex = Array.IndexOf(lines, "---");
				if (metaEndIndex == -1) { throw new Exception("no meta information."); }
				markdownInfo.FillWithBlock(lines.Take(metaEndIndex));
				markdownInfo.Content = string.Join("\n", new ArraySegment<string>(lines, metaEndIndex + 1, lines.Length - metaEndIndex - 1));
				markdownInfos.Add(markdownInfo);
			}
		}
		private void render()
		{
			renderPostPage();
			renderIndexPage();
			File.WriteAllText("output/index.html", "<script>window.location = '/1.html'</script>");
		}
		private void renderPostPage()
		{
			if (!Directory.Exists("output")) Directory.CreateDirectory("output");
			foreach (var markdown in markdownInfos)
			{
				var t = templates[markdown.Template];
				var options = new TemplateOptions();
				options.MemberAccessStrategy.Register<MarkdownInfo>();
				options.MemberAccessStrategy.Register<Blog>();
				var ctx = new TemplateContext(new { post = markdown, blog = templateCtx }, options);
				Directory.CreateDirectory($"output/{markdown.Location}");
				File.WriteAllText(
					$"output/{markdown.URL}",
					t.Render(ctx)
					);
			};
		}
		private void renderIndexPage()
		{
			int pageIndex = 1;
			markdownInfos.Sort((x, y) => DateTime.Compare(y.Date, x.Date));

			for (int i = 0; i < markdownInfos.Count; i += 5)
			{
				var t = templates["index"];
				MarkdownInfoList markdownInfoList = new MarkdownInfoList();
				for (int j = i; j < i + 5 && j < markdownInfos.Count; j++)
				{
					var markdown = markdownInfos[j];
					markdownInfoList.Add(markdown);
				}
				var page = new Page();
				page.CurPage = pageIndex;
				page.PageTotal = (int)Math.Ceiling(markdownInfos.Count / 5.0);
				page.PageRange = Enumerable.Range(
					pageIndex - 2 > 1 ? (pageIndex - 2 > page.PageTotal - 5 ? page.PageTotal - 4 : pageIndex - 2) : 1,
					page.PageTotal < 5 ? page.PageTotal : 5)
					.ToArray();
				var options = new TemplateOptions();
				options.MemberAccessStrategy.Register<MarkdownInfoList>();
				options.MemberAccessStrategy.Register<MarkdownInfo>();
				options.MemberAccessStrategy.Register<Blog>();
				options.MemberAccessStrategy.Register<Page>();
				options.MemberAccessStrategy.Register<List<string>>();
				var ctx = new TemplateContext(new { posts = markdownInfoList, blog = templateCtx, page = page }, options);
				File.WriteAllText(
					$"output/{pageIndex}.html",
					t.Render(ctx)
					);
				pageIndex++;
			}
		}
	}
}
