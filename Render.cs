using Fluid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickBlog
{
	class BlogRender
	{
		TemplateOptions templateOptions = new TemplateOptions();
		Blog templateCtx = new Blog("66ws");
		Dictionary<string, IFluidTemplate> templates;
		public BlogRender()
		{
			templateOptions.MemberAccessStrategy.Register<MarkdownInfoList>();
			templateOptions.MemberAccessStrategy.Register<MarkdownInfo>();
			templateOptions.MemberAccessStrategy.Register<Blog>();
			templateOptions.MemberAccessStrategy.Register<Page>();
			templateOptions.MemberAccessStrategy.Register<List<string>>();
		}
		public void Render(ref List<MarkdownInfo> markdownInfos, Dictionary<string, IFluidTemplate> template)
		{
			templates = template;
			renderPostPage(ref markdownInfos);
			renderIndexPage(ref markdownInfos);
			renderDateArchive(ref markdownInfos);
		}
		private void renderPostPage(ref List<MarkdownInfo> markdownInfos)
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
		private void renderIndexPage(ref List<MarkdownInfo> markdownInfos)
		{
			markdownInfos.Sort((x, y) => DateTime.Compare(y.Date, x.Date));
			renderPages(markdownInfos, 5, "index", "/{0}", true);
		}
		private void renderDateArchive(ref List<MarkdownInfo> markdownInfos)
		{
			foreach (var group in markdownInfos.GroupBy(x => x.Date.Year))
			{
				renderPages(group.ToList(), 5, "archive", $"/{group.Key}/{{0}}", true);
			};
			foreach (var group in markdownInfos.GroupBy(x => x.Date.Year.ToString() + "/" + x.Date.Month.ToString()))
			{
				renderPages(group.ToList(), 5, "archive", $"/{group.Key}/{{0}}", true);
			};
		}
		/// <param name="formatString">output{string.Format(formatString, pageIndex)}.html</param>
		private void renderPages(List<MarkdownInfo> mdlist, int pageSize, string pageTemplate, string formatString, bool indexPage)
		{
			int pageIndex = 1;
			var t = templates[pageTemplate];
			for (int i = 0; i < mdlist.Count; i += pageSize)
			{
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
			if (indexPage)
			{
				File.WriteAllText($"output{string.Format(formatString, "index.html")}", "<script>window.location = './1.html'</script>");
			}
		}
	}
}
