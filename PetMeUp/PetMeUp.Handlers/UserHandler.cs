using Microsoft.IdentityModel.Tokens;
using PetMeUp.Dtos;
using PetMeUp.Models;
using PetMeUp.Models.Models;
using PetMeUp.Models.Responses;
using PetMeUp.Repos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PetMeUp.Handlers
{
    public class UserHandler : IDisposable
    {
        private readonly UserRepo _Repo;
        private readonly LogHandler _log;
        public UserHandler(string conString,string dbType,LogHandler logger)
        {
            _log=logger;
            _Repo = new UserRepo(conString, (DatabaseType)Enum.Parse(typeof(DatabaseType), dbType));
        }

        public async Task<DataResponse<List<User>>> GetUsers()
        {
            try
            {

                var users = await _Repo.GetAllUsers();
                return new DataResponse<List<User>>(true,String.Empty,users);
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in GetUsers with message -> {ex.Message}", Severity.Exception);
                return new DataResponse<List<User>>(false,"Error On Proceess", null);
            }
        }

        public async Task<DataResponse<User>> GetUser(int id)
        {
            try
            {

                var users = await _Repo.GetUserById(id);
                return new DataResponse<User>(true, String.Empty, users);
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in GetUser with message -> {ex.Message}", Severity.Exception);
                return new DataResponse<User>(false, "Error On Proceess", null);
            }
        }
        public async Task<DataResponse<User>> GetUser(string username)
        {
            try
            {

                var users = await _Repo.GetUserFromUsername(username);
                return new DataResponse<User>(true, String.Empty, users);
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in GetUser with message -> {ex.Message}", Severity.Exception);
                return new DataResponse<User>(false, "Error On Proceess", null);
            }
        }

        public async Task<BaseResponse> AddUser(UserDto dto,bool isItGuest)
        {
            try
            {
                var newUser = new User();
                var checkIfUserExists = await _Repo.GetUserFromUsername(dto.Username);
                if (checkIfUserExists is not null) return new BaseResponse(false, "Username already exists");
                newUser.Username = dto.Username;
                (newUser.PasswordHash, newUser.PasswordSalt) = CreatePasswordHash(dto.Password);
                newUser.Role = isItGuest ? Role.User : Role.Employee;
                var result = await _Repo.AddUser(newUser);
                if(result) return new BaseResponse(true, String.Empty);
                else return new BaseResponse(false, "Error On Proceess");
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in AddUser with message -> {ex.Message}", Severity.Exception);
                return new BaseResponse(false, "Error On Proceess");
            }
        }

        public async Task<BaseResponse> UpdateUser(UserDto dto, int id)
        {
            try
            {
                var newUser = new User();
                var checkIfUserExists = await _Repo.GetUserFromUsername(dto.Username);
                if (checkIfUserExists is not null) return new BaseResponse(false, "Username already exists");
                newUser.Username = dto.Username;
                (newUser.PasswordHash, newUser.PasswordSalt) = CreatePasswordHash(dto.Password); 
                var result = await _Repo.UpdateUser(newUser,id);
                if (result) return new BaseResponse(true, String.Empty);
                else return new BaseResponse(false, "Invalid info");
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in AddUser with message -> {ex.Message}", Severity.Exception);
                return new BaseResponse(false, "Error On Proceess");
            }
        }

        public async Task<BaseResponse> DeleteUser(int id)
        {
            try
            { 
                var result = await _Repo.DeleteUser(id);
                if (result) return new BaseResponse(true, String.Empty);
                else return new BaseResponse(false, "Invalid info");
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in AddUser with message -> {ex.Message}", Severity.Exception);
                return new BaseResponse(false, "Error On Proceess");
            }
        }

        public async Task<DataResponse<string>> Login(UserDto dto,string secretKey)
        {
            try
            { 
                var user = await _Repo.GetUserFromUsername(dto.Username);
                if (user is not null)
                {
                    if (VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt))
                    {
                        return new DataResponse<string>(true, String.Empty, CreateToken(user,secretKey));
                    }
                } 
                return new DataResponse<string>(false, "Invalid Creds", null);
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in AddUser with message -> {ex.Message}", Severity.Exception);
                return new DataResponse<string>(false, "Error On Proceess", null); 
            }
        }




        private (byte[], byte[]) CreatePasswordHash(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                var passwordSalt = hmac.Key;
                var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return (passwordHash, passwordSalt);
            }
        }

        private bool VerifyPassword(string password, byte[] hash, byte[] salt)
        {
            using (var hmac = new HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(hash);
            }
        }

        private string CreateToken(User user,string secretKey)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.Role,user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public void Dispose()
        {
            _Repo.Dispose(); 
        }
    }
}