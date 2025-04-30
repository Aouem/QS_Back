namespace QS.Models
{
    public class RepondantDto
    {
                public int Id { get; set; }  // << AJOUTÉ
        public string Nom { get; set; } = string.Empty;
        public int Categorie { get; set; }
    }
}
