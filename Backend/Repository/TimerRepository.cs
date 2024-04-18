using Backend.Models;
using Common.DTO;
using Microsoft.EntityFrameworkCore;
using Timer = Backend.Models.Timer;

namespace Backend.Repository
{
    public class TimerRepository
    {
        private readonly AppDbContext _context;
        public TimerRepository(AppDbContext context)
        {
                _context=context;
        }

        public async Task<Timer> Create(Timer model)
        {
            await _context.Timers.AddAsync(model);
            _context.SaveChanges();
            return model;
        }

        public async Task<List<TimerDTO>> GetAllTimer()
        {
            var result = await _context.Timers.Select(x=>new TimerDTO
            {
                StartTime = x.StartTime,
                EndTime=x.EndTime,
                Id=x.Id
            }).ToListAsync();
            return result;
        }
    }
}
