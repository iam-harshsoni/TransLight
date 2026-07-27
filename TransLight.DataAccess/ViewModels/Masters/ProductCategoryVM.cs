using System.ComponentModel.DataAnnotations;
using TransLight.Utility.Enums;

namespace TransLight.DataAccess.ViewModels.Masters
{
    public class ProductCategoryVM
    {
        public Guid? Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public YesNo Active { get; set; }
    }
}
