using Microsoft.EntityFrameworkCore;
using PetMeUp.Models;
using PetMeUp.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMeUp.Repos
{
    public class SpeciesRepo : SuperRepo
    {
        public SpeciesRepo(string conString, DatabaseType dbtype) : base(conString, dbtype)
        {
        }

        public async Task<List<PetSpecie>> GetSpecies(int pageNo, int pageSize)
        {
            var data = await _db.Species.Skip(pageSize * (pageNo - 1)).Take(pageSize)
                                        .Include(s => s.Pic).Include(s => s.Group).ToListAsync();
            return data;
        }
        public async Task<List<PetSpecie>> GetSpecies()
        {
            var data = await _db.Species.Include(s => s.Pic)
                            .Include(s => s.Pic).Include(s => s.Group).ToListAsync();
            return data;
        }
        public async Task<List<PetSpecie>> GetSpecies(int groupId)
        {
            var data = await _db.Species.Where(f => f.Group != null).Where(s=>s.Group.Id == groupId)
                                        .Include(s => s.Pic).Include(s => s.Group).ToListAsync();
            return data;
        }

        public async Task<PetSpecie> GetSpecie(int id)
        {
            var data = await _db.Species.Where(f => f.Id == id)
                                          .Include(s => s.Pic).Include(s => s.Group).FirstOrDefaultAsync();
            return data;
        }

        public async Task<int> GetSpeciesCount()
        {
            return await _db.Species.CountAsync();
        }

        public async Task<bool> AddSpecie(PetSpecie newSpecie)
        {
            if (newSpecie == null)
                return false;
            if (await _db.Species.AnyAsync(s => s.Title == newSpecie.Title))
                return false;
            var pic = newSpecie.Pic;
            var group = newSpecie.Group;
            newSpecie.Pic = null;
            newSpecie.Group = null;
            await _db.Species.AddAsync(newSpecie);
            var result = await _db.SaveChangesAsync() != 0;
            if (result)
            {
                var specieInBase = await _db.Species.FirstOrDefaultAsync(s => s.Title == newSpecie.Title);
                if (specieInBase != null)
                {
                    specieInBase.Group = group;
                    specieInBase.Pic = pic;
                }
            }
            return await _db.SaveChangesAsync() != 0;
        }
        public async Task<bool> DeleteSpecie(int id)
        {
            var specieToDel = await _db.Species.Where(s => s.Id == id).FirstOrDefaultAsync();
            if (specieToDel is null)
                return false;
            bool result = true;
            if (specieToDel.Pic is not null)
            {
                _db.Pics.Remove(specieToDel.Pic);
                result = await _db.SaveChangesAsync() != 0;
            }
            if (!result) return false;
            _db.Species.Remove(specieToDel);
            return await _db.SaveChangesAsync() != 0;
        }

        public async Task<bool> UpdateSpecie(int id, PetSpecie newGroup)
        {
            var specieToUpdate = await _db.Species.Where(s => s.Id == id).FirstOrDefaultAsync();
            if (specieToUpdate is null)
                return false;
            specieToUpdate.Pic = newGroup.Pic;
            specieToUpdate.Group = newGroup.Group;
            specieToUpdate.Title = newGroup.Title;
            specieToUpdate.Description = newGroup.Description;
            return await _db.SaveChangesAsync() != 0;
        }
    }
}
