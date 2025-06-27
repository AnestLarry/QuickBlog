using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickBlog
{
    class MarkdownInfoList : List<MarkdownInfo>
    {
        private CategoryTree categoryTree = new CategoryTree();

        public CategoryTree Categories
        {
            get
            {
                BuildCategoryTree();
                return categoryTree;
            }
        }

        public Dictionary<string, int> Tags
        {
            get
            {
                var tags = new Dictionary<string, int>();
                foreach (var post in this)
                {
                    if (post.Tags != null)
                    {
                        foreach (var tag in post.Tags)
                        {
                            if (tags.ContainsKey(tag))
                            {
                                tags[tag]++;
                            }
                            else
                            {
                                tags.Add(tag, 1);
                            }
                        }
                    }
                }
                return tags;
            }
        }

        private void BuildCategoryTree()
        {
            categoryTree = new CategoryTree();
            foreach (var post in this)
            {
                categoryTree.AddPost(post);
            }
        }
    }
}