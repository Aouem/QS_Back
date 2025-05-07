using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QS.Models
{
    public class Repondant
    {
        public int Id { get; set; }

        // Propriété nullable pour permettre la suppression avec SetNull

        public string? Nom { get; set; }

        public string? Genre { get; set; }

        [MaxLength(50)]
        public string? TrancheAge { get; set; }
 

        // Clé étrangère vers Categorie (nullable)
        public int? CategorieId { get; set; }

        [JsonPropertyName("categorie")]
        public Categorie? Categorie { get; set; } // Navigation vers Categorie

        // Clé étrangère vers MedicalService (nullable)
        public int? ServiceId { get; set; }

        // Clé étrangère vers MedicalService (nullable aussi)
        [JsonPropertyName("service")]
        public MedicalService? Service { get; set; } // Navigation vers MedicalService

        // Ignore la collection lors de la sérialisation en JSON
        [JsonIgnore]
        public ICollection<Reponse> Reponses { get; set; } = new List<Reponse>();
    }
}
