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
    public class SpeciesHandler : IDisposable
    {
        private readonly SpeciesRepo _Repo;
        private readonly LogHandler _log;
        private readonly PicHandler _icHandler;
        private readonly GroupHandler _groupHandler;
        public SpeciesHandler(string conString, string dbType, LogHandler logger)
        {
            _log = logger;
            _Repo = new SpeciesRepo(conString, (DatabaseType)Enum.Parse(typeof(DatabaseType), dbType));
            _icHandler = new PicHandler(conString, dbType, _log);
            _groupHandler = new GroupHandler(conString, dbType, _log);
        }

        public async Task<DataResponse<PetSpecie>> GetSpecie(int id)
        {
            try
            {

                var specie = await _Repo.GetSpecie(id);
                specie.Pic = await _icHandler.GetPic(specie.Pic?.Id);
                return new DataResponse<PetSpecie>(true, String.Empty, specie); ;
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in GetGroup with message -> {ex.Message}", Severity.Exception);
                return new DataResponse<PetSpecie>(false, "Error On Process", null);
            }
        }

        public async Task<DataResponse<List<PetSpecie>>> GetSpecies()
        {
            try
            {

                var species = await _Repo.GetSpecies();
                species.ForEach(async group => group.Pic = await _icHandler.GetPic(group.Pic?.Id));
                return new DataResponse<List<PetSpecie>>(true, String.Empty, species); ;
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in GetGroup with message -> {ex.Message}", Severity.Exception);
                return new DataResponse<List<PetSpecie>>(false, "Error On Process", null);
            }
        }

        public async Task<DataCountResponse<List<PetSpecie>>> GetSpecies(int pageNo, int pageSize)
        {
            try
            {

                var species = await _Repo.GetSpecies(pageNo, pageSize);
                species.ForEach(async group => group.Pic = await _icHandler.GetPic(group.Pic?.Id));
                var countOfGroups = await _Repo.GetSpeciesCount();
                return new DataCountResponse<List<PetSpecie>>(true, String.Empty, species, countOfGroups);
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in GetGroup with message -> {ex.Message}", Severity.Exception);
                return new DataCountResponse<List<PetSpecie>>(false, "Error On Process", null, 0);
            }
        }

        public async Task<BaseResponse> AddSpecie(SpecieDto dto)
        {
            try
            {
                var newSpecie = new PetSpecie()
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    Life_Span = dto.LifeSpan,
                    Origin = dto.Origin,
                    MaximumFemaleHeight = dto.MaximumFemaleHeight,
                    MaximumMaleWeight = dto  .MaximumMaleWeight,
                    MinimumFemaleHeight = dto.MinimumFemaleHeight,
                    MinimumMaleWeight = dto  .MinimumMaleWeight,
                };
                var groupToAdd = await _groupHandler.GetGroup(dto.GroupId);
                if (groupToAdd.Data is null) new BaseResponse(false, "Invalid Process");
                newSpecie.Group = groupToAdd.Data;
                if (dto.Pic is not null)
                {
                    newSpecie.Pic = await _icHandler.GetPic(dto.Pic.Name, false);
                    if (newSpecie.Pic is null)
                    {
                        var newPic = new Pic() { Name = dto.Pic.Name, Content = dto.Pic.Content };
                        var resultOfPick = await _icHandler.AddPic(newPic);
                        if (resultOfPick is not null)
                            newSpecie.Pic = resultOfPick;
                    }
                }
                var result = await _Repo.AddSpecie(newSpecie);
                return result ? new BaseResponse(true, String.Empty) : new BaseResponse(false, "Error On Process");
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in AddSpecie with message -> {ex.Message}", Severity.Exception);
                return new BaseResponse(false, "Error On Process");
            }
        }

        public async Task<BaseResponse> DeleteSpecie(int id)
        {
            try
            {
                var groupToDelete = await _Repo.GetSpecie(id);
                if (groupToDelete is null) return new BaseResponse(false, "Invalid Id");
                var result = await _Repo.DeleteSpecie(id);
                return result ? new BaseResponse(true, String.Empty) : new BaseResponse(false, "Error On Process");
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in DeleteSpecie with message -> {ex.Message}", Severity.Exception);
                return new BaseResponse(false, "Error On Process");
            }
        }

        public async Task<BaseResponse> UpdateSpecie(int id, SpecieDto newDto)
        {
            try
            {
                var specieToUpdate = await _Repo.GetSpecie(id);
                int idOfPick = 0;
                if (specieToUpdate is null) return new BaseResponse(false, "Invalid Id");
                bool result = true;
                if (newDto.Pic is not null && specieToUpdate.Pic is not null)
                {
                    var resultOfDeletePic = await _icHandler.UpdatePic(specieToUpdate.Pic.Id, new Pic() { Name = newDto.Pic.Name, Content = newDto.Pic.Content });
                    if (resultOfDeletePic is null) return new BaseResponse(false, "Error On Process");
                }
                if (newDto.Pic is not null && specieToUpdate.Pic is null)
                {
                    var resultOfAddPick = await _icHandler.AddPic(new Pic() { Name = newDto.Pic.Name, Content = newDto.Pic.Content });
                    if (resultOfAddPick is null) return new BaseResponse(false, "Error On Process");
                    specieToUpdate.Pic = resultOfAddPick;
                }
                if (newDto.Pic is null && specieToUpdate.Pic is not null)
                {
                    idOfPick = specieToUpdate.Pic.Id;
                    specieToUpdate.Pic = null;
                }
                specieToUpdate.Description = newDto.Description;
                specieToUpdate.Title = newDto.Title;
                var updatedFamily = await _groupHandler.GetGroup(newDto.GroupId);
                specieToUpdate.Group = updatedFamily.Data;
                result = await _Repo.UpdateSpecie(id, specieToUpdate);
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
                await _log.WriteToLog($"Exception Caught in UpdateSpecie with message -> {ex.Message}", Severity.Exception);
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
