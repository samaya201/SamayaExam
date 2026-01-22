using SamayaExam.Models.Common;

namespace SamayaExam.Models;

public class Category : BaseEntity 
{
    public string Title { get; set; } = null!;
    public ICollection<Member> Members { get; set; } = [];
}
