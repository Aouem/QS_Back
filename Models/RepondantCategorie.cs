using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using QS.Models;

namespace QS.Models
{
    public class RepondantCategorie
    {

    public int Id { get; set; }
        
  
    public string? Name { get; set; } // Ex: "Visiteur", "Patient", "Accompagnant"
    
   
    public string? Description { get; set; }
}
}