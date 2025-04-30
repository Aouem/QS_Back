using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization; // Assure-toi d'importer ce namespace
using QS.Models; // Assure-toi que QuestionType et ServiceType sont bien dans ce namespace
using QS.Enums;

namespace QS.Models
{
    public class Question
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; } // Texte de la question

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))] // Utilisation d'un convertisseur pour gérer les enums en JSON
        public QuestionType Type { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))] // Utilisation d'un convertisseur pour gérer les enums en JSON
        public ServiceType ServiceType { get; set; } // Lien au service médical

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        [JsonIgnore] // Ignore cette propriété lors de la sérialisation en JSON
        public ICollection<Reponse> Reponses { get; set; } = new List<Reponse>();
    }
}
