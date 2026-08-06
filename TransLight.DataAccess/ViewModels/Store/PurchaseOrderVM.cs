using TransLight.DataAccess.Models;

namespace TransLight.DataAccess.ViewModels.Store
{
    public class PurchaseOrderVM : TransactionsVM
    {
        public ICollection<PurchaseOrderDetailsVM> PurchaseOrderDetails { get; set; } = [];
    }
}
