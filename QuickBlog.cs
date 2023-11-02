using Fluid;
using System.Text.RegularExpressions;

namespace QuickBlog
{
	class QuickBlog
	{
		Dictionary<string, IFluidTemplate> templates = new Dictionary<string, IFluidTemplate>();
		FluidParser parser = new FluidParser();
		List<MarkdownInfo> markdownInfos = new List<MarkdownInfo>();
		Blog templateCtx = new Blog("66ws");
		TemplateOptions templateOptions = new TemplateOptions();
		public QuickBlog()
		{
			templateOptions.MemberAccessStrategy.Register<MarkdownInfoList>();
			templateOptions.MemberAccessStrategy.Register<MarkdownInfo>();
			templateOptions.MemberAccessStrategy.Register<Blog>();
			templateOptions.MemberAccessStrategy.Register<Page>();
			templateOptions.MemberAccessStrategy.Register<List<string>>();
		}
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
			renderDateArchive();
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
			markdownInfos.Sort((x, y) => DateTime.Compare(y.Date, x.Date));
			renderPages(markdownInfos, 5, "/{0}");
		}
		private void renderDateArchive()
		{
			foreach (var group in markdownInfos.GroupBy(x => x.Date.Year))
			{
				renderPages(group.ToList(), 5, $"/{group.Key}/{{0}}");
			};
			foreach (var group in markdownInfos.GroupBy(x => x.Date.Year.ToString() + "/" + x.Date.Month.ToString()))
			{
				renderPages(group.ToList(), 5, $"/{group.Key}/{{0}}");
			};
		}
		/// <param name="formatString">output{string.Format(formatString, pageIndex)}.html</param>
		private void renderPages(List<MarkdownInfo> mdlist, int pageSize, string formatString)
		{
			int pageIndex = 1;
			for (int i = 0; i < mdlist.Count; i += pageSize)
			{
				var t = templates["index"];
				MarkdownInfoList markdownInfoList = new MarkdownInfoList();
				for (int j = i; j < i + pageSize && j < mdlist.Count; j++)
				{
					var markdown = mdlist[j];
					markdownInfoList.Add(markdown);
				}
				var page = new Page();
				page.CurPage = pageIndex;
				page.PageTotal = (int)Math.Ceiling(mdlist.Count * 1.0 / pageSize);
				page.PageRange = Enumerable.Range(
					pageIndex - 2 > 1 ? (pageIndex - 2 > page.PageTotal - 5 ? page.PageTotal - 4 : pageIndex - 2) : 1,
					page.PageTotal < 5 ? page.PageTotal : 5)
					.ToArray();
				var ctx = new TemplateContext(new { posts = markdownInfoList, blog = templateCtx, page = page }, templateOptions);
				File.WriteAllText(
					$"output{string.Format(formatString, pageIndex)}.html",
					t.Render(ctx)
					);
				pageIndex++;
			}
			File.WriteAllText($"output{string.Format(formatString, "index.html")}", "<script>window.location = './1.html'</script>");
		}
	}
}
