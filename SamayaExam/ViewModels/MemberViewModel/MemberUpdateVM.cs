using System.ComponentModel.DataAnnotations;

namespace SamayaExam.ViewModels.MemberViewModel
{
    public class MemberUpdateVM
    {
        [Required]
        public int Id { get; set; }
        [Required,MinLength(3)]
        public string Name { get; set; } = string.Empty;
        
        public IFormFile? Image { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
