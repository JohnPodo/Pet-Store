using Microsoft.EntityFrameworkCore;
using PetMeUp.Models;
using PetMeUp.Models.Models;

namespace PetMeUp.Repos
{
    public class LogRepo : SuperRepo
    {
        public LogRepo(string conString, DatabaseType dbtype) : base(conString, dbtype)
        {
        }

        public async Task<bool> AddToLog(LogMeUp model)
        {
            await _db.Logs.AddAsync(model);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAllLogs()
        {
            var logsToDel = _db.Logs.Where(s => s.InsertDate.AddDays(7).Date > DateTime.Now.Date);
            var countOfRows = logsToDel.Count();
            _db.Logs.RemoveRange(logsToDel);
            var result = await _db.SaveChangesAsync();
            return countOfRows == result;
        }
    }
}