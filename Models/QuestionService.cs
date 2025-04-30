using QS.Models; // Assurez-vous que le namespace des modèles est correct
using QS.Enums; // Assurez-vous que l'énumération ServiceType est incluse
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QS.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly AppDbContext _context;

        public QuestionService(AppDbContext context)
        {
            _context = context;
        }

        // Créer une question
        public async Task<Question> CreateQuestion(Question question)
        {
            if (question == null) throw new ArgumentNullException(nameof(question));

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            return question;
        }

        // Mettre à jour une question
        public async Task UpdateQuestion(int id, Question question)
        {
            var existingQuestion = await _context.Questions.FindAsync(id);
            if (existingQuestion == null) throw new KeyNotFoundException("Question introuvable.");

            existingQuestion.Text = question.Text; // Mettez à jour les autres propriétés selon le besoin

            await _context.SaveChangesAsync();
        }

        // Supprimer une question
        public async Task DeleteQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null) throw new KeyNotFoundException("Question introuvable.");

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
        }

        // Récupérer une question par ID
        public async Task<Question> GetQuestionById(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null) throw new KeyNotFoundException("Question introuvable.");

            return question;
        }

        // Récupérer des questions par type de service
        public async Task<List<Question>> GetQuestionsByService(ServiceType serviceType)
        {
            return await _context.Questions
                .Where(q => q.ServiceType == serviceType) // Assurez-vous que le modèle contient ce champ
                .ToListAsync();
        }
    }
}
