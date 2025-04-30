
using Newtonsoft.Json;  // Add this import at the top if it's not already there
using System.ComponentModel.DataAnnotations;  // Add this import for MaxLength
using QS.Services;
using QS.Models;
public class FormResultEntity
{
    public int Id { get; set; }
    public int ServiceId { get; set; }
    public int RepondantId { get; set; }
    public string Reponses { get; set; }  // Stockage des réponses sous forme de JSON
    public Repondant Repondant { get; set; }
}
