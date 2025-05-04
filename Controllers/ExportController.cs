using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QS.Models;
using Microsoft.Extensions.Logging;

namespace QS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExportController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExportController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ExportData()
        {
            var questions = await _context.Questions.ToListAsync();
            var repondants = await _context.Repondants.ToListAsync();
            var medicalServices = await _context.MedicalServices.ToListAsync();
            var categories = await _context.Categories.ToListAsync();
            var reponses = await _context.Reponses.ToListAsync();

            var reponsesDto = reponses.Select(r => new ReponseDto
            {
                QuestionId = r.QuestionId,
                // Type removed since 'Reponse' does not have a 'Type' property
                NumericValue = r.NumericValue,
                BooleanValue = r.BooleanValue,
                TextValue = r.TextValue,
                Repondant = new RepondantDto
                {
                    Id = r.RepondantId,
                    // Other mappings
                }
            }).ToList();

            var export = new ExportDto
            {
                Questions = questions,
                Repondants = repondants,
                MedicalServices = medicalServices,
                Categories = categories,
                Reponses = reponsesDto
            };

            return Ok(export);
        }
    }
}
