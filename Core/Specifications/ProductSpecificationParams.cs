namespace Core.Specifications
{
	public class ProductSpecificationParams
	{
		private const int MAX_PAGE_SIZE = 50;

		public int PageIndex { get; set; } = 1;

		public int PageSize
		{
			get { return _pageSie; }
			set { _pageSie = value > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : value; }
		}

		private int _pageSie = 6;

		public int? BrandId { get; set; }
		public int? TypeId { get; set; }
		public string Sort { get; set; }

		private string _search;

		public string Search
		{
			get => _search; 
			set => _search = value.ToLower();
		}

	}
}