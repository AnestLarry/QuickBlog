namespace QuickBlog
{
	class Blog
	{
		private string title;
		private string description;
		private List<string> archives;
		private Dictionary<string, int> categories;

		public Blog(string blogTitle)
		{
			Title = blogTitle;
			archives = new List<string>();
			categories = new Dictionary<string, int>();
		}

		public string Title { get => title; set => title = value; }
		public Dictionary<string, int> Categories { get => categories; set => categories = value; }
		public string Description { get => description; set => description = value; }
		public List<string> Archives { get => archives; set => archives = value; }
	}
}