using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QS.Models;

[Route("api/[controller]")]
[ApiController]
public class MedicalServiceController : ControllerBase
{
    private readonly AppDbContext _context;

    public MedicalServiceController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/MedicalService
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicalService>>> GetMedicalServices()
    {
        return await _context.MedicalServices.ToListAsync();
    }

    // GET: api/MedicalService/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MedicalService>> GetMedicalService(int id)
    {
        var medicalService = await _context.MedicalServices.FindAsync(id);

        if (medicalService == null)
        {
            return NotFound();
        }

        return medicalService;
    }

    // POST: api/MedicalService
    [HttpPost]
    public async Task<ActionResult<MedicalService>> PostMedicalService(MedicalService medicalService)
    {
        _context.MedicalServices.Add(medicalService);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMedicalService), new { id = medicalService.Id }, medicalService);
    }

    // PUT: api/MedicalService/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMedicalService(int id, MedicalService medicalService)
    {
        if (id != medicalService.Id)
        {
            return BadRequest();
        }

        _context.Entry(medicalService).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MedicalServiceExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/MedicalService/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMedicalService(int id)
    {
        var medicalService = await _context.MedicalServices.FindAsync(id);
        if (medicalService == null)
        {
            return NotFound();
        }

        _context.MedicalServices.Remove(medicalService);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool MedicalServiceExists(int id)
    {
        return _context.MedicalServices.Any(e => e.Id == id);
    }
}
