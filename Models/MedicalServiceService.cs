using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QS.Models;
using QS.Enums;

namespace QS.Services
{
    public class MedicalServiceService : IMedicalServiceService
    {
        private readonly AppDbContext _context;

        public MedicalServiceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MedicalService> CreateService(MedicalService service)
        {
            _context.MedicalServices.Add(service);
            await _context.SaveChangesAsync();
            return service;
        }

        public async Task UpdateService(int id, MedicalService service)
        {
            var existingService = await _context.MedicalServices.FindAsync(id);
            if (existingService != null)
            {
                existingService.Name = service.Name;
                existingService.Code = service.Code;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteService(int id)
        {
            var service = await _context.MedicalServices.FindAsync(id);
            if (service != null)
            {
                _context.MedicalServices.Remove(service);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<MedicalService> GetServiceByCode(string code)
        {
            return await _context.MedicalServices.FirstOrDefaultAsync(s => s.Code == code);
        }

        public async Task<List<MedicalService>> GetAllServices()
        {
            return await _context.MedicalServices.ToListAsync();
        }
    }
}
