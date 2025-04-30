using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using QS.Models;

public class MedicalService
{
     public  int Id { get; set; }
    
 [Required]
    public required string Code { get; set; } // Ex: "URG" pour Urgences
    
   [Required]

    public required string Name { get; set; }
    
   
    public string? Description { get; set; }
  
}


