using System.ComponentModel.DataAnnotations;

namespace SamayaExam.ViewModels.MemberViewModel
{
    public class MemberCreateVM
    {
        [Required, MinLength(3)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public IFormFile Image { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
