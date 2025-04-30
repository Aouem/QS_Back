
using QS.Models; // Adapte le namespace selon l'emplacement de ta classe Question et ton enum ServiceType
using QS.Enums;

// IMedicalServiceService.cs
public interface IMedicalServiceService
{
    Task<MedicalService> CreateService(MedicalService service);
    Task UpdateService(int id, MedicalService service);
    Task DeleteService(int id);
    Task<MedicalService> GetServiceByCode(string code);
    Task<List<MedicalService>> GetAllServices();
}