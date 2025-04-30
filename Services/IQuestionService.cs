using System.Collections.Generic;
using System.Threading.Tasks;
using QS.Models; // Adapte le namespace selon l'emplacement de ta classe Question et ton enum ServiceType
using QS.Enums; // Adapte le namespace selon l'emplacement de ta classe Question et ton enum ServiceType

namespace QS.Services
{
    public interface IQuestionService
    {
        Task<Question> CreateQuestion(Question question);
        Task UpdateQuestion(int id, Question question);
        Task DeleteQuestion(int id);
        Task<Question> GetQuestionById(int id);
        Task<List<Question>> GetQuestionsByService(ServiceType serviceType);
    }
}
