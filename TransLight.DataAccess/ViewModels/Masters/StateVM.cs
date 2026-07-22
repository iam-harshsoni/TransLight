using System.ComponentModel.DataAnnotations;
using TransLight.Utility.Enums;

namespace TransLight.DataAccess.ViewModels.Masters
{
    public class StateVM
    {
        public Guid? Id { get; set; }

        [Required]
        public string Gst { get; set; } = null!;

        [Required]
        public string Code { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public YesNo UnionTerritory { get; set; }

        [Required]
        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }
    }
}
