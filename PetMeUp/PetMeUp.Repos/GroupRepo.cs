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
    public class GroupRepo : SuperRepo
    {
        public GroupRepo(string conString, DatabaseType dbtype) : base(conString, dbtype)
        {
        }

        public async Task<List<PetGroup>> GetGroups(int pageNo, int pageSize)
        {
            var data = await _db.Groups.Skip(pageSize * (pageNo - 1)).Take(pageSize).Include(s => s.Pic).Include(s => s.Family).ToListAsync();
            return data;
        }
        public async Task<List<PetGroup>> GetGroups()
        {
            var data = await _db.Groups.Include(s => s.Pic).Include(s => s.Family).ToListAsync();
            return data;
        }
        public async Task<List<PetGroup>> GetGroups(int familyId)
        {
            var data = await _db.Groups.Where(f => f.Family.Id == familyId).Include(s => s.Pic).Include(s => s.Family).ToListAsync();
            return data;
        }

        public async Task<PetGroup> GetGroup(int id)
        {
            var data = await _db.Groups.Where(f => f.Id == id).Include(s => s.Pic).Include(s => s.Family).FirstOrDefaultAsync();
            return data;
        }

        public async Task<int> GetGroupsCount()
        {
            return await _db.Groups.CountAsync();
        }

        public async Task<bool> AddGroup(PetGroup newGroup)
        {
            if (newGroup == null)
                return false;
            if (await _db.Groups.AnyAsync(s => s.Title == newGroup.Title))
                return false;
            var pic = newGroup.Pic;
            var family = newGroup.Family;
            newGroup.Pic = null;
            newGroup.Family = null;
            await _db.Groups.AddAsync(newGroup);
            var result = await _db.SaveChangesAsync() != 0;
            if (result)
            {
                var groupInBase = await _db.Groups.FirstOrDefaultAsync(s => s.Title == newGroup.Title);
                if (groupInBase != null)
                {
                    groupInBase.Family = family;
                    groupInBase.Pic = pic;
                }
            }
            return await _db.SaveChangesAsync() != 0;
        }
        public async Task<bool> DeleteGroup(int id)
        {
            var groupToDel = await _db.Groups.Where(s => s.Id == id).FirstOrDefaultAsync();
            if (groupToDel is null)
                return false;
            bool result = true;
            if (groupToDel.Pic is not null)
            {
                _db.Pics.Remove(groupToDel.Pic);
                result = await _db.SaveChangesAsync() != 0;
            }
            if (!result) return false;
            _db.Groups.Remove(groupToDel);
            return await _db.SaveChangesAsync() != 0;
        }

        public async Task<bool> UpdateGroup(int id,PetGroup newGroup)
        {
            var groupToUpdate = await _db.Groups.Where(s => s.Id == id).FirstOrDefaultAsync();
            if (groupToUpdate is null)
                return false;
            groupToUpdate.Pic = newGroup.Pic;
            groupToUpdate.Family = newGroup.Family;
            groupToUpdate.Title = newGroup.Title;
            groupToUpdate.Description = newGroup.Description;
            return await _db.SaveChangesAsync() != 0;
        }
    }
}
