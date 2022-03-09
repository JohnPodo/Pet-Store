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
    public class GroupHandler : IDisposable
    {
        private readonly GroupRepo _Repo;
        private readonly LogHandler _log;
        private readonly PicHandler _icHandler;
        public GroupHandler(string conString, string dbType, LogHandler logger)
        {
            _log = logger;
            _Repo = new GroupRepo(conString, (DatabaseType)Enum.Parse(typeof(DatabaseType), dbType));
            _icHandler = new PicHandler(conString, dbType, _log);
        }

        public async Task<DataResponse<PetGroup>> GetGroup(int id)
        {
            try
            {

                var group = await _Repo.GetGroup(id);
                group.Pic = await _icHandler.GetPic(group.Pic?.Id);
                return new DataResponse<PetGroup>(true, String.Empty, group); ;
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in GetGroup with message -> {ex.Message}", Severity.Exception);
                return new DataResponse<PetGroup>(false,"Error On Process",null);
            }
        }

        public async Task<DataResponse<List<PetGroup>>> GetGroups()
        {
            try
            {

                var groups = await _Repo.GetGroups();
                groups.ForEach(async group=>group.Family.Pic = await _icHandler.GetPic(group.Family?.Pic?.Id));
                groups.ForEach(async group => group.Pic = await _icHandler.GetPic(group.Pic?.Id));
                return new DataResponse<List<PetGroup>>(true, String.Empty, groups); ;
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in GetGroup with message -> {ex.Message}", Severity.Exception);
                return new DataResponse<List<PetGroup>>(false, "Error On Process", null);
            }
        }

        public void Dispose()
        {
           _icHandler.Dispose();
            _Repo.Dispose();
        }
    }
}
