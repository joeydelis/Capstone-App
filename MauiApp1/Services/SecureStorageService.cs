using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    public class SecureStorageService : ISecureStorageService
    {
        public Task SetAsync(string key, string value) => SecureStorage.SetAsync(key, value);
        public Task<string> GetAsync(string key) => SecureStorage.GetAsync(key);
        public void Remove(string key) => SecureStorage.Remove(key);
    }
}
