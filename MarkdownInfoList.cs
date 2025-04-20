using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickBlog
{
    class MarkdownInfoList : List<MarkdownInfo>
    {
        private CategoryTree categoryTree;

        public CategoryTree Categories
        {
            get
            {
                if (categoryTree == null)
                {
                    BuildCategoryTree();
                }
                return categoryTree;
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