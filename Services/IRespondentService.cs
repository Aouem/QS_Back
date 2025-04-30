using QS.Models; // Assurez-vous que le namespace de votre modèle est correct
using QS.Enums; // Assurez-vous que l'énumération est incluse ici

namespace QS.Services
{
    public interface IRespondantService
    {
        Task<Repondant> CreateRepondant(Repondant repondant);
        Task UpdateRepondantStatus(int id, ReponseStatus status);
        Task<List<Repondant>> GetNonRepondants();
        Task AssignToService(int respondantId, int serviceId);
    }
}
