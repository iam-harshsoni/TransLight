using Microsoft.AspNetCore.Identity;

namespace TransLight.DataAccess.IdentityModel
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FullName { get; set; }

        public Guid? CompanyId { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
