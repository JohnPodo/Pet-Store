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
                        if (resultOfPick is not null)
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

        public async Task<BaseResponse> DeleteGroup(int id)
        {
            try
            {
                var groupToDelete = await _Repo.GetGroup(id);
                if (groupToDelete is null) return new BaseResponse(false, "Invalid Id");
                var result = await _Repo.DeleteGroup(id);
                return result ? new BaseResponse(true, String.Empty) : new BaseResponse(false, "Error On Process");
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in DeleteGroup with message -> {ex.Message}", Severity.Exception);
                return new BaseResponse(false, "Error On Process");
            }
        }

        public async Task<BaseResponse> UpdateGroup(int id, GroupDto newDto)
        {
            try
            {
                var groupToUpdate = await _Repo.GetGroup(id);
                int idOfPick = 0;
                if (groupToUpdate is null) return new BaseResponse(false, "Invalid Id");
                bool result = true;
                if (newDto.Pic is not null && groupToUpdate.Pic is not null)
                {
                    var resultOfDeletePic = await _icHandler.UpdatePic(groupToUpdate.Pic.Id, new Pic() { Name = newDto.Pic.Name, Content = newDto.Pic.Content });
                    if (resultOfDeletePic is null) return new BaseResponse(false, "Error On Process");
                }
                if (newDto.Pic is not null && groupToUpdate.Pic is null)
                {
                    var resultOfAddPick = await _icHandler.AddPic(new Pic() { Name = newDto.Pic.Name, Content = newDto.Pic.Content });
                    if (resultOfAddPick is null) return new BaseResponse(false, "Error On Process");
                    groupToUpdate.Pic = resultOfAddPick;
                }
                if (newDto.Pic is null && groupToUpdate.Pic is not null)
                {
                    idOfPick = groupToUpdate.Pic.Id;
                    groupToUpdate.Pic = null;
                }
                groupToUpdate.Description = newDto.Description;
                groupToUpdate.Title = newDto.Title;
                var updatedFamily = await _famHandler.GetFamily(newDto.FamilyId);
                groupToUpdate.Family = updatedFamily.Data;
                result = await _Repo.UpdateGroup(id, groupToUpdate);
                if(!result) new BaseResponse(false, "Error On Process");
                if (idOfPick != 0)
                {
                    result = await _icHandler.DeletePic(idOfPick);
                    if (!result) return new BaseResponse(false, "Error On Process");
                }
                return new BaseResponse(true, String.Empty);
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in UpdateGroup with message -> {ex.Message}", Severity.Exception);
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
