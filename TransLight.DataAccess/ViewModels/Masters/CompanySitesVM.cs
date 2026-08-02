using System.ComponentModel.DataAnnotations;
using TransLight.Utility.Enums;

namespace TransLight.DataAccess.ViewModels.Masters
{
    public class CompanySitesVM
    {
        public Guid? Id { get; set; }

        public Guid CompanyId { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string? City { get; set; }

        public int? Pincode { get; set; }

        public string? Contact { get; set; }

        public string? Email { get; set; }

        public string? GstNo { get; set; }

        public string? LutNo { get; set; }

        public string? EinvoiceUsername { get; set; }

        public string? EwayUsername { get; set; }

        [DataType(DataType.Password)]
        public string? EwayPassword { get; set; }

        public YesNo Active { get; set; }
    }
}
