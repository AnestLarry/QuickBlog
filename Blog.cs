namespace QuickBlog
{
	class Blog
	{
		private string title;
		private string description;
		private int[] page = new int[2];
		public Blog(string blogTitle)
		{
			Title = blogTitle;
		}

		public string Title { get => title; set => title = value; }
		public string Description { get => description; set => description = value; }
		public int[] Page { get => page; set => page = value; }
	}
}
