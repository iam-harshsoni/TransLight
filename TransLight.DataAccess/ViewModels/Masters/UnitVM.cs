using System.ComponentModel.DataAnnotations;
using TransLight.DataAccess.Models;

namespace TransLight.DataAccess.ViewModels.Masters
{
    public class UnitVM
    {
        public Guid? Id { get; set; }

        [Required]
        public string Code { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        // Data for listing
        public IEnumerable<Unit> Items { get; set; } = [];
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; } = 1;

        // Search parameters
        public string? SearchCode { get; set; }
        public string? SearchName { get; set; }
    }
}
