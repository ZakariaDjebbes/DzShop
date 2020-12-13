namespace Core.Entities.Order
{
    public class ProductItemOrdered
    {
        public ProductItemOrdered()
        {
        }

        public ProductItemOrdered(int productItemId, string productName, string pcitureUrl)
        {
            ProductItemId = productItemId;
            ProductName = productName;
            PictureUrl = pcitureUrl;
        }

        public int ProductItemId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
    }
}