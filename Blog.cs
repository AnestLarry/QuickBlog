namespace QuickBlog
{
	class Blog
	{
		private string title = string.Empty;
		private string description = string.Empty;
		private List<string> archives = new List<string>();
		private Dictionary<string, int> categories = new Dictionary<string, int>();
		private Dictionary<string, int> tags = new Dictionary<string, int>();

		public Blog(string blogTitle)
		{
			Title = blogTitle;
			archives = new List<string>();
			categories = new Dictionary<string, int>();
			Tags = new Dictionary<string, int>();
		}

		public string Title { get => title; set => title = value; }
		public Dictionary<string, int> Categories { get => categories; set => categories = value; }
		public string Description { get => description; set => description = value; }
		public List<string> Archives { get => archives; set => archives = value; }
		public Dictionary<string, int> Tags { get => tags; set => tags = value; }
	}
}