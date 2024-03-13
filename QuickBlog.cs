using Fluid;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace QuickBlog
{
    class QuickBlog
    {
        Dictionary<string, IFluidTemplate> templates = new Dictionary<string, IFluidTemplate>();
        FluidParser parser = new FluidParser();
        List<MarkdownInfo> markdownInfos = new List<MarkdownInfo>();
        BlogRender blogRender = new BlogRender();

        public void main(string[] args)
        {
            loadTemplates();
            loadMarkdowns();
            blogRender.Render(ref markdownInfos, templates);
            exportStatic();
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
        private void exportStatic()
        {
            if (Directory.Exists("./static"))
            {
                Func<string, string, int> w = null;
                w = (src, dst) =>
                {
                    if (!Directory.Exists(Path.Combine(dst, src))) { Directory.CreateDirectory(Path.Combine(dst, src)); }
                    foreach (var file in Directory.GetFiles(src))
                    {
                        if(!File.Exists(Path.Combine(dst, file)))
                        {
                            File.Copy(file, Path.Combine(dst, file));
                        }
                    }
                    foreach (var dir in Directory.GetDirectories(src))
                    {
                        if (!Directory.Exists(Path.Combine(dst, dir)))
                        {
                            Directory.CreateDirectory(Path.Combine(dst, dir));
                        }
                        w(dir, dst);
                    }
                    return 1;
                };
                w("./static", "./output");
            }
        }

    }
}
