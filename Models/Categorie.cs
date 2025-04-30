using Newtonsoft.Json;  // Add this import at the top if it's not already there
using System.ComponentModel.DataAnnotations;  // Add this import for MaxLength
using QS.Services;
using QS.Models;

public class Categorie
{
    public int? Id { get; set; }
    
    [MaxLength(50)]  // Example: if you want to limit the length of the "Nom"
    public string? Nom { get; set; } = string.Empty; // Ex: "Actif", "Retraité", etc.

    // Initialize the collection to avoid null reference issues
    public Categorie()
    {
        Repondants = new List<Repondant>();
    }

    // Ignore the Repondants collection during serialization to prevent circular reference
    [JsonIgnore]
    public ICollection<Repondant>? Repondants { get; set; }
}
