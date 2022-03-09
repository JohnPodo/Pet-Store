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
    public class UserRepo : SuperRepo
    {
        public UserRepo(string conString, DatabaseType dbtype) : base(conString, dbtype)
        {
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _db.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            return user;
        }

        public async Task<List<User>> GetAllUsers() => await _db.Users.ToListAsync();

        public async Task<bool> AddUser(User user)
        {
            await _db.Users.AddAsync(user);
            var result = await _db.SaveChangesAsync();
            return result == 1;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var userToDel = await GetUserById(id);
            if (userToDel is null) return false;
            _db.Users.Remove(userToDel); 
            var result = await _db.SaveChangesAsync();
            return result == 1;
        }

        public async Task<User> GetUserFromUsername(string username)
        {
            var user = await _db.Users.Where(s => s.Username == username).FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> UpdateUser(User user,int id)
        { 
            var userToModify = await GetUserById(id);
            if (userToModify == null) return false;
            userToModify.Username = user.Username;
            userToModify.PasswordHash = user.PasswordHash;
            userToModify.PasswordSalt = user.PasswordSalt;   
            var result = await _db.SaveChangesAsync();
            return result == 1;
        }
    }
}
