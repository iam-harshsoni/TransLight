using System.ComponentModel.DataAnnotations;
using TransLight.Utility.Enums;

namespace TransLight.DataAccess.ViewModels.Masters
{
    public class RawMaterialVM
    {
        public Guid? Id { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
        public string? CategoryName { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public string? Make { get; set; }

        public string? Pack { get; set; }

        public Guid? UnitId { get; set; }
        public string? Unit { get; set; }

        public decimal? Rate { get; set; }

        [Required]
        public decimal Gst { get; set; }

        [Required]
        public string Hsn { get; set; } = null!;

        public int? Msl { get; set; }

        public YesNo Active { get; set; }
    }
}
