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

        public async Task<List<PetGroup>> GetGroups(int noRows, int pageSize)
        {
            var data = await _db.Groups.Skip(pageSize * noRows).Take(pageSize).Include(s=>s.Pic).Include(s => s.Family).ToListAsync(); 
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
    }
}
