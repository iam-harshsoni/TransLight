using TransLight.DataAccess.ViewModels.Store;

namespace TransLight.DataAccess.Models
{
    public class PurchaseOrderDetailsVM : TransactionDetailsVM
    {
        public bool IsSelected { get; set; } = false;
    }
}
