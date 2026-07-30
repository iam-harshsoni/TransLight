namespace TransLight.DataAccess.Filters.Masters
{
    public class CompanyFilters : PaginationRequest
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Contact { get; set; }
        public string? Email { get; set; }
        public string? GstNo { get; set; }
    }
}
