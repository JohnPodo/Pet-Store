using PetMeUp.Models;
using PetMeUp.Models.Models;
using PetMeUp.Models.Responses;
using PetMeUp.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMeUp.Handlers
{
    public class FamilyHandler : IDisposable
    {
        private readonly FamilyRepo _Repo;
        private readonly LogHandler _log;
        private readonly PicHandler _icHandler;
        public FamilyHandler(string conString, string dbType, LogHandler logger)
        {
            _log = logger;
            _Repo = new FamilyRepo(conString, (DatabaseType)Enum.Parse(typeof(DatabaseType), dbType));
            _icHandler = new PicHandler(conString, dbType, _log);
        }

        public async Task<DataResponse<PetFamily>> GetFamily(int id)
        {
            try
            {

                var family = await _Repo.GetFamily(id);
                family.Pic = await _icHandler.GetPic(family.Pic?.Id);
                return new DataResponse<PetFamily>(true, String.Empty, family); ;
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in GetGroup with message -> {ex.Message}", Severity.Exception);
                return new DataResponse<PetFamily>(false,"Error On Process",null);
            }
        }

        public async Task<DataResponse<List<PetFamily>>> GetFamilies()
        {
            try
            {

                var groups = await _Repo.GetFamilies(); 
                groups.ForEach(async group => group.Pic = await _icHandler.GetPic(group.Pic?.Id));
                return new DataResponse<List<PetFamily>>(true, String.Empty, groups); ;
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in GetGroup with message -> {ex.Message}", Severity.Exception);
                return new DataResponse<List<PetFamily>>(false, "Error On Process", null);
            }
        }

        public void Dispose()
        {
           _icHandler.Dispose();
            _Repo.Dispose();
        }
    }
}
