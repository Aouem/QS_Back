using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using QS.Models;

public class Reponse
{
    public int Id { get; set; }
 
    // Clés étrangères
    [JsonPropertyName("question_id")]  // Personnalisation du nom de la propriété dans le JSON
    public int QuestionId { get; set; }
    
    // Relation obligatoire vers Question
    [JsonPropertyName("question")]  // Personnalisation du nom de la propriété dans le JSON
    public required Question Question { get; set; }

    [JsonPropertyName("repondant_id")]  // Personnalisation du nom de la propriété dans le JSON
    public int RepondantId { get; set; }

     // Relation obligatoire vers Repondant
    [JsonPropertyName("repondant")]  // Personnalisation du nom de la propriété dans le JSON
    public required Repondant Repondant { get; set; }

    // Réponses typées
    [JsonPropertyName("numeric_value")]  // Personnalisation du nom de la propriété dans le JSON
    public int? NumericValue { get; set; }   // Pour les échelles 1-5

    [JsonPropertyName("boolean_value")]  // Personnalisation du nom de la propriété dans le JSON
    public bool? BooleanValue { get; set; }  // Pour Vrai/Faux

    [JsonPropertyName("text_value")]  // Personnalisation du nom de la propriété dans le JSON
    public string? TextValue { get; set; }    // Pour les réponses libres
}
