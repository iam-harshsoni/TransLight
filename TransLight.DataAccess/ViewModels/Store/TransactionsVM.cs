using TransLight.Utility.Enums;

namespace TransLight.DataAccess.ViewModels.Store
{
    public class TransactionsVM
    {
        public Guid? Id { get; set; }

        public Guid CompanyId { get; set; }

        public Guid CompanySiteId { get; set; }

        public Guid PartyId { get; set; } = Guid.Empty;
        public string? PartyName { get; set; }

        public Guid PartySiteId { get; set; } = Guid.Empty;
        public string? PartySiteName { get; set; }

        public string Id2Format { get; set; } = string.Empty;

        public DateOnly Date { get; set; }

        public int Type { get; set; } = 0;

        public int TransactionType { get; set; } = 0;

        public Guid CurrencyId { get; set; }
        public string? CurrencyCode { get; set; }

        public decimal ExchangeRate { get; set; } = 1;

        public DeliveryTypes DeliveryType { get; set; }

        public YesNo Cancel { get; set; } = YesNo.No;

        public string Remarks { get; set; } = string.Empty;

        public decimal BasicAmt { get; set; } = 0;

        public decimal GstAmt { get; set; } = 0;

        public decimal RoundOffAmt { get; set; } = 0;

        public decimal TotalAmt { get; set; } = 0;
    }
}
