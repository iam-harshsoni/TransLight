namespace TransLight.DataAccess.Filters.Masters
{
    public class ProductFilter : PaginationRequest
    {
        public string? CategoryName { get; set; }
        public string? UnitName { get; set; }
        public string? Name { get; set; }
        public string? Make { get; set; }
        public string? Pack { get; set; }
        public string? Hsn { get; set; }
        public int? Active { get; set; } = -1;

    }
}
