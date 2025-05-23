using Microsoft.EntityFrameworkCore;
using Weather.Infrastructure.Persistance.Models;

namespace Weather.Infrastructure.Persistance.Repositories
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly WeatherDbContext _context;

        public WeatherRepository(WeatherDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(WeatherRecord record)
        {
            await _context.WeatherRecords.AddAsync(record);
            await _context.SaveChangesAsync();
        }

        public async Task<WeatherRecord?> GetLatestAsync()
        {
            return await _context.WeatherRecords
                                 .OrderByDescending(r => r.Timestamp)
                                 .FirstOrDefaultAsync();
        }
    }
}
