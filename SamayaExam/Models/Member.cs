using SamayaExam.Models.Common;

namespace SamayaExam.Models;

public class Member:BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}
