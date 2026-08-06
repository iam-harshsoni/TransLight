namespace TransLight.DataAccess.ViewModels.Store
{
    public class TransactionDetailsVM
    {
        public Guid? Id { get; set; }

        public string Vertical { get; set; } = string.Empty;

        public Guid TransactionId { get; set; }

        public string SrNo { get; set; } = string.Empty;

        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Qty { get; set; }

        public Guid UnitId { get; set; }
        public string UnitCode { get; set; } = string.Empty;

        public decimal Rate { get; set; }

        public decimal BasicAmt { get; set; }

        public decimal GstPer { get; set; }

        public decimal GstAmt { get; set; }

        public decimal TotalAmt { get; set; }
    }
}
