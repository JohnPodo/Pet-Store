using PetMeUp.Dtos;
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
        private readonly FamilyHandler _famHandler;
        public GroupHandler(string conString, string dbType, LogHandler logger)
        {
            _log = logger;
            _Repo = new GroupRepo(conString, (DatabaseType)Enum.Parse(typeof(DatabaseType), dbType));
            _icHandler = new PicHandler(conString, dbType, _log);
            _famHandler = new FamilyHandler(conString, dbType, _log);
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
                return new DataResponse<PetGroup>(false, "Error On Process", null);
            }
        }

        public async Task<DataResponse<List<PetGroup>>> GetGroups()
        {
            try
            {

                var groups = await _Repo.GetGroups();
                groups.ForEach(async group => group.Pic = await _icHandler.GetPic(group.Pic?.Id));
                return new DataResponse<List<PetGroup>>(true, String.Empty, groups); ;
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in GetGroup with message -> {ex.Message}", Severity.Exception);
                return new DataResponse<List<PetGroup>>(false, "Error On Process", null);
            }
        }

        public async Task<DataCountResponse<List<PetGroup>>> GetGroups(int pageNo, int pageSize)
        {
            try
            {

                var groups = await _Repo.GetGroups(pageNo, pageSize);
                groups.ForEach(async group => group.Pic = await _icHandler.GetPic(group.Pic?.Id));
                var countOfGroups = await _Repo.GetGroupsCount();
                return new DataCountResponse<List<PetGroup>>(true, String.Empty, groups, countOfGroups);
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in GetGroup with message -> {ex.Message}", Severity.Exception);
                return new DataCountResponse<List<PetGroup>>(false, "Error On Process", null, 0);
            }
        }

        public async Task<BaseResponse> AddGroup(GroupDto dto)
        {
            try
            {
                var newGroup = new PetGroup()
                {
                    Title = dto.Title,
                    Description = dto.Description
                };
                var familyToAdd = await _famHandler.GetFamily(dto.FamilyId);
                if (familyToAdd.Data is null) new BaseResponse(false, "Invalid Process");
                newGroup.Family = familyToAdd.Data;
                if (dto.Pic is not null)
                {
                    newGroup.Pic = await _icHandler.GetPic(dto.Pic.Name, false);
                    if (newGroup.Pic is null)
                    {
                        var newPic = new Pic() { Name = dto.Pic.Name, Content = dto.Pic.Content };
                        var resultOfPick = await _icHandler.AddPic(newPic);
                        if (resultOfPick != null)
                            newGroup.Pic = resultOfPick;
                    }
                }
                var result = await _Repo.AddGroup(newGroup);
                return result ? new BaseResponse(true, String.Empty) : new BaseResponse(false, "Error On Process");
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in AddGroup with message -> {ex.Message}", Severity.Exception);
                return new BaseResponse(false, "Error On Process");
            }
        }

        public void Dispose()
        {
            _icHandler.Dispose();
            _Repo.Dispose();
        }
    }
}
