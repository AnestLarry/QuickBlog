using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickBlog
{
	class Page
	{
		private int curPage;
		private int pageTotal;
		private int[] pageRange = Array.Empty<int>();

		public Page()
		{
		}

		public int CurPage { get => curPage; set => curPage = value; }
		public int PageTotal { get => pageTotal; set => pageTotal = value; }
		public int[] PageRange { get => pageRange; set => pageRange = value; }
	}
}
