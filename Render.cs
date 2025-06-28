using Fluid;
using Microsoft.Extensions.FileProviders;
using System.Linq;

namespace QuickBlog
{
    class BlogRender
    {
        TemplateOptions templateOptions = new TemplateOptions();
        Blog blogCtx = new Blog("66ws");
        Dictionary<string, IFluidTemplate> templates = new Dictionary<string, IFluidTemplate>();
        public BlogRender()
        {
            templateOptions.MemberAccessStrategy.Register<MarkdownInfoList>();
            templateOptions.MemberAccessStrategy.Register<MarkdownInfo>();
            templateOptions.MemberAccessStrategy.Register<Blog>();
            templateOptions.MemberAccessStrategy.Register<Page>();
            templateOptions.MemberAccessStrategy.Register<List<string>>();
            templateOptions.MemberAccessStrategy.Register<Dictionary<string, int>>();
            templateOptions.FileProvider = new PhysicalFileProvider($"{AppDomain.CurrentDomain.BaseDirectory}templates");
        }
        public void Render(ref List<MarkdownInfo> markdownInfos, Dictionary<string, IFluidTemplate> template)
        {
            templates = template;
            var markdownInfoList = new MarkdownInfoList();
            markdownInfoList.AddRange(markdownInfos);
            blogCtx.Categories = markdownInfoList.Categories.GetAllCategories();
            
            renderTags(markdownInfoList);
            renderPostPage(ref markdownInfos);
            renderDateArchive(ref markdownInfos);
            renderIndexPage(ref markdownInfos);
            renderCategories(markdownInfoList);
        }

        private void renderTags(MarkdownInfoList markdownInfos)
        {
            blogCtx.Tags.Clear();
            var tags = markdownInfos.SelectMany(x => x.Tags).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct();
            foreach (var tag in tags)
            {
                blogCtx.Tags.Add(tag, markdownInfos.Where(x => x.Tags.Contains(tag)).Count());
            }
            var t = templates["tags"];
            var ctx = new TemplateContext(new { blog = blogCtx }, templateOptions);
            File.WriteAllText(
                $"output/tags.html",
                t.Render(ctx)
                );

            foreach (var tag in tags)
            {
                renderPages(markdownInfos.Where(x => x.Tags.Contains(tag)).ToList(), 5, "tag", $"/tag/{tag}/{{0}}", true, tag);
            }
        }

        private void renderCategories(MarkdownInfoList markdownInfos)
        {
            var categoryTree = markdownInfos.Categories;
            foreach (var category in categoryTree.GetAllCategories())
            {
                var categoryPath = category.Key;
                var posts = GetPostsInCategory(markdownInfos, categoryPath);
                var outputPath = $"categories/{categoryPath.Replace('/', '-')}";
                var displayCategory = categoryPath.Split('/').Last();
                renderCategory(posts, outputPath, displayCategory);
            }
        }

        private List<MarkdownInfo> GetPostsInCategory(MarkdownInfoList markdownInfos, string categoryPath)
        {
            var categories = categoryPath.Split('/');
            return markdownInfos.Where(post =>
                post.Categories != null &&
                post.Categories.Count >= categories.Length &&
                categories.SequenceEqual(post.Categories.Take(categories.Length)))
                .OrderByDescending(x => x.Date)
                .ToList();
        }

        private void renderCategory(List<MarkdownInfo> posts, string outputPath, string displayCategory, int pageSize = 5)
        {
            int pageIndex = 1;
            var template = templates["category"];

            for (int i = 0; i < posts.Count; i += pageSize)
            {
                var pageMarkdowns = new MarkdownInfoList();
                for (int j = i; j < i + pageSize && j < posts.Count; j++)
                {
                    pageMarkdowns.Add(posts[j]);
                }

                var page = new Page
                {
                    CurPage = pageIndex,
                    PageTotal = (int)Math.Ceiling(posts.Count * 1.0 / pageSize)
                };

                page.PageRange = Enumerable.Range(
                    pageIndex - 2 > 1 ? (pageIndex - 2 > page.PageTotal - 5 ? page.PageTotal - 4 : pageIndex - 2) : 1,
                    page.PageTotal < 5 ? page.PageTotal : 5)
                    .ToArray();

                var ctx = new TemplateContext(
                    new
                    {
                        posts = pageMarkdowns,
                        blog = blogCtx,
                        page = page,
                        category = displayCategory
                    },
                    templateOptions
                );

                var pagePath = $"output/{outputPath}/{pageIndex}.html";
                var directory = Path.GetDirectoryName(pagePath);
                if (directory != null && !Directory.Exists(directory)) Directory.CreateDirectory(directory);

                File.WriteAllText(pagePath, template.Render(ctx));
                pageIndex++;
            }

            // 创建index.html重定向到第一页
            var indexPath = $"output/{outputPath}/index.html";
            File.WriteAllText(indexPath, "<script>window.location = './1.html'</script>");
        }

        private void renderPostPage(ref List<MarkdownInfo> markdownInfos)
        {
            if (!Directory.Exists("output")) Directory.CreateDirectory("output");
            foreach (var markdown in markdownInfos)
            {
                var t = templates[markdown.Template];
                var ctx = new TemplateContext(new { post = markdown, blog = blogCtx }, templateOptions);
                Directory.CreateDirectory($"output/{markdown.Location}");
                File.WriteAllText(
                    $"output/{markdown.URL}",
                    t.Render(ctx)
                    );
            };
        }
        private void renderIndexPage(ref List<MarkdownInfo> markdownInfos)
        {
            markdownInfos = markdownInfos.OrderByDescending(x => x.Top).ThenByDescending(x => x.Date).ToList();
            renderPages(markdownInfos, 5, "index", "/{0}", true);
        }
        private void renderDateArchive(ref List<MarkdownInfo> markdownInfos)
        {
            markdownInfos.GroupBy(x => x.Date.Year.ToString())
                .Union(markdownInfos.GroupBy(x => x.Date.Year.ToString() + "/" + x.Date.Month.ToString()))
                .Select(group => { blogCtx.Archives.Add(group.Key); return group; })
                .Count();
            blogCtx.Archives.Sort();
            blogCtx.Archives.Reverse();
            markdownInfos.GroupBy(x => x.Date.Year.ToString())
                .Union(markdownInfos.GroupBy(x => x.Date.Year.ToString() + "/" + x.Date.Month.ToString()))
                .Select(group => { renderPages(group.ToList(), 5, "archive", $"/{group.Key}/{{0}}", true); return group; })
                .Count();
        }
        /// <param name="formatString">output{string.Format(formatString, pageIndex)}.html</param>
        private void renderPages(List<MarkdownInfo> mdlist, int pageSize, string pageTemplate, string formatString, bool indexPage, string? category = null)
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
                var ctx = new TemplateContext(new { posts = markdownInfoList, blog = blogCtx, page = page, category = (string?)category }, templateOptions);
                var outputPath = $"output{string.Format(formatString, pageIndex)}.html";
                var directory = Path.GetDirectoryName(outputPath);
                if (directory != null && !Directory.Exists(directory)) Directory.CreateDirectory(directory);
                File.WriteAllText(
                    outputPath,
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
