using System.Collections.Generic;
using QS.Models;

public class ExportDto
{
    public List<Question> Questions { get; set; }
    public List<Repondant> Repondants { get; set; }
    public List<MedicalService> MedicalServices { get; set; }
    public List<Categorie> Categories { get; set; }
    public List<ReponseDto> Reponses { get; set; }
}
