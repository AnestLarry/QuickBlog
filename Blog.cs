namespace QuickBlog
{
	class Blog
	{
		private string title;
		private string description;
		private List<string> archives;
		public Blog(string blogTitle)
		{
			Title = blogTitle;
			archives = new List<string>();
		}

		public string Title { get => title; set => title = value; }
		public string Description { get => description; set => description = value; }
		public List<string> Archives { get => archives; set => archives = value; }
	}
}
