using System.ComponentModel.DataAnnotations;
using TransLight.DataAccess.Models;

namespace TransLight.DataAccess.ViewModels.Masters
{
    public class CountryVM
    {
        public Guid? Id { get; set; }

        [Required]
        public string Code { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        public virtual ICollection<State> States { get; set; } = new List<State>();
    }
}
