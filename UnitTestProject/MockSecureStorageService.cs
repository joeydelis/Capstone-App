using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp1.Services;

namespace UnitTestProject
{
    public class MockSecureStorageService : ISecureStorageService
    {
        private readonly Dictionary<string, string> _storage = new();

        public Task SetAsync(string key, string value)
        {
            _storage[key] = value;
            return Task.CompletedTask;
        }

        public Task<string> GetAsync(string key)
        {
            _storage.TryGetValue(key, out var value);
            return Task.FromResult(value);
        }

        public void Remove(string key)
        {
            _storage.Remove(key);
        }
    }
}
