using System;
using System.Collections.Generic;

namespace QuickBlog
{
    public class CategoryNode
    {
        public string Name { get; set; }
        public Dictionary<string, CategoryNode> Children { get; set; }
        public List<MarkdownInfo> Posts { get; set; }
        public int PostCount { get; set; }

        public CategoryNode(string name)
        {
            Name = name;
            Children = new Dictionary<string, CategoryNode>();
            Posts = new List<MarkdownInfo>();
            PostCount = 0;
        }

        public void AddPost(MarkdownInfo post)
        {
            Posts.Add(post);
            PostCount++;
        }

        public int GetTotalPosts()
        {
            // 如果是叶子节点（没有子分类），直接返回当前节点的文章数
            if (Children.Count == 0)
            {
                return PostCount;
            }
            
            // 如果有子分类，只计算子分类的文章总数
            int total = 0;
            foreach (var child in Children.Values)
            {
                total += child.GetTotalPosts();
            }
            return total;
        }
    }

    public class CategoryTree
    {
        private CategoryNode root;

        public CategoryTree()
        {
            root = new CategoryNode("root");
        }

        public CategoryNode Root => root;

        public void AddPost(MarkdownInfo post)
        {
            if (post.Categories == null || post.Categories.Count == 0)
            {
                root.AddPost(post);
                return;
            }

            CategoryNode current = root;
            foreach (var category in post.Categories)
            {
                if (!current.Children.ContainsKey(category))
                {
                    current.Children[category] = new CategoryNode(category);
                }
                current = current.Children[category];
            }
            current.AddPost(post);
        }

        public Dictionary<string, int> GetAllCategories()
        {
            var result = new Dictionary<string, int>();
            TraverseTree(root, "", result);
            return result;
        }

        private void TraverseTree(CategoryNode node, string prefix, Dictionary<string, int> result)
        {
            foreach (var child in node.Children)
            {
                string categoryPath = string.IsNullOrEmpty(prefix) ? child.Key : $"{prefix}/{child.Key}";
                result[categoryPath] = child.Value.GetTotalPosts();
                TraverseTree(child.Value, categoryPath, result);
            }
        }


    }
}