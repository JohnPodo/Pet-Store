using PetMeUp.Models;
using PetMeUp.Models.Models;
using PetMeUp.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMeUp.Handlers
{
    public class LogHandler : IDisposable
    {
        private readonly Guid _ProcessSession; 
        private readonly LogRepo _Repo;
        public LogHandler(string conString,string dbtype)
        {
            _ProcessSession = Guid.NewGuid();
            _Repo = new LogRepo(conString, (DatabaseType)Enum.Parse(typeof(DatabaseType),dbtype));
        }

        public async Task<bool> WriteToLog(string message, Severity severity)
        {
            try
            {
                if (string.IsNullOrEmpty(message))
                    return false;
                LogMeUp model = new LogMeUp();
                model.Message = message;
                model.Severity = severity;
                model.ProcessSession = _ProcessSession;
                model.InsertDate = DateTime.Now;
                return await _Repo.AddToLog(model);
            }
            catch (Exception ex)
            {
                await WriteToLog($"Exception caught in GetAllLogsOfSession of handler with message \n Message -> {ex.Message}", Severity.Exception);
                return false;
            }
        }

        public async Task<bool> DeleteAllLogs()
        {
            try
            {
                return await _Repo.DeleteAllLogs(); 

            }
            catch (Exception ex)
            {
                await WriteToLog($"Exception caught in DeleteAllLogs of handler with message \n Message -> {ex.Message}", Severity.Exception);
                return false;
            }
        }

        public void Dispose()
        {
           _Repo.Dispose();
        }
    }
}
