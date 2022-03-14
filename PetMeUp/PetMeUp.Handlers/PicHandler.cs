using PetMeUp.Models;
using PetMeUp.Models.Models;
using PetMeUp.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMeUp.Handlers
{
    public class PicHandler : IDisposable
    {
        private readonly LogHandler _log;
        private readonly PicRepo _Repo;
        public PicHandler(string conString, string dbType, LogHandler logger)
        {
            _log = logger;
            _Repo = new PicRepo(conString, (DatabaseType)Enum.Parse(typeof(DatabaseType), dbType));
        }

        public async Task<Pic> GetPic(int? id)
        {
            try
            {
                if (id == null)
                    return null;
                var pic = await _Repo.GetPic(id.Value);
                pic = TransformPictureToBase64(pic);
                return pic;
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in GetPic with message -> {ex.Message}", Severity.Exception);
                return null;
            }

        }

        public async Task<Pic> GetPic(string title, bool transformToBase64)
        {
            try
            {
                if (string.IsNullOrEmpty(title))
                    return null;
                var pic = await _Repo.GetPic(title);
                if (transformToBase64)
                    pic = TransformPictureToBase64(pic);
                return pic;
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in GetPic with message -> {ex.Message}", Severity.Exception);
                return null;
            }

        }

        public async Task<List<Pic>> GetPics()
        {
            try
            {
                var pics = await _Repo.GetPics();
                pics.ForEach(s => s = TransformPictureToBase64(s));
                return pics;
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in GetPics with message -> {ex.Message}", Severity.Exception);
                return null;
            }
        }

        public async Task<Pic> AddPic(Pic pic)
        {
            try
            {
                pic = SaveImage(pic);
                var success = await _Repo.AddPic(pic);
                return success ? pic : null;
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in AddPic with message -> {ex.Message}", Severity.Exception);
                return null;
            }
        }

        public async Task<Pic> UpdatePic(int id, Pic pic)
        {
            try
            {
                pic = SaveImage(pic);
                var success = await _Repo.UpdatePic(id, pic);
                return success ? pic : null;
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in UpdatePic with message -> {ex.Message}", Severity.Exception);
                return null;
            }

        }

        public async Task<bool> DeletePic(int id)
        {
            try
            {
                var success = await _Repo.DeletePic(id);
                return success;
            }
            catch (Exception ex)
            {
                await _log.WriteToLog($"Exception Caught in DeletePic with message -> {ex.Message}", Severity.Exception);
                return false;
            }
        }

        public Pic TransformPictureToBase64(Pic pic)
        {
            if (pic is null)
                return null;
            var path = Path.Combine(Environment.CurrentDirectory, "Images");
            if (Directory.Exists(path))
            {
                var fullPath = Path.Combine(path, pic.Name);
                if (File.Exists(fullPath))
                {
                    Byte[] bytes = File.ReadAllBytes(fullPath);
                    pic.Content = Convert.ToBase64String(bytes);
                    return pic;
                }
            }
            return pic;
        }

        public Pic SaveImage(Pic pic)
        {
            if (pic is null)
                return null;
            var path = Path.Combine(Environment.CurrentDirectory, "Images");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var fullPath = Path.Combine(path, pic.Name);
            File.WriteAllBytes(fullPath, Convert.FromBase64String(pic.Content));
            pic.Content = fullPath;
            return pic;
        }

        public void Dispose()
        {
            _Repo.Dispose();
        }
    }
}
