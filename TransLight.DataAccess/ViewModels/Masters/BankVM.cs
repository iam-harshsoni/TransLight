using System.ComponentModel.DataAnnotations;

namespace TransLight.DataAccess.ViewModels.Masters
{
    public class BankVM
    {
        public Guid? Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
