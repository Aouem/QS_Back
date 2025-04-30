
namespace QS.Models
{
public class ReponseDto
{
    public int QuestionId { get; set; }
    public string Type { get; set; } = string.Empty;
    public int? NumericValue { get; set; }
    public bool? BooleanValue { get; set; }
    public string? TextValue { get; set; }
    public RepondantDto Repondant { get; set; } = new RepondantDto();
}
}