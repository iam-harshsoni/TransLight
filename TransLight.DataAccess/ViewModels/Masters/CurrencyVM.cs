using System.ComponentModel.DataAnnotations;

namespace TransLight.DataAccess.ViewModels.Masters
{
    public class CurrencyVM
    {
        public Guid? Id { get; set; }

        [Required]
        public string Code { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

    }
}
