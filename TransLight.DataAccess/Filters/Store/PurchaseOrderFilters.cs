namespace TransLight.DataAccess.Filters.Store
{
    public class PurchaseOrderFilters : PaginationRequest
    {
        public string? PoNo { get; set; }
        public string? PoDate { get; set; }
        public string? PartyName { get; set; }
        public decimal? Amount { get; set; }
        public int? Cancel { get; set; } = -1;
    }
}
