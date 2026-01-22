using Microsoft.AspNetCore.Identity;

namespace SamayaExam.Models
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
