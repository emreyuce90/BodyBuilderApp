using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Interfaces
{
    public interface IRedisService
    {
        //Get
        Task<T?> GetAsync<T>(string key);
        //Add
        Task SetAsync<T>(string key, T value);
        //Remove
        Task RemoveAsync(string key);
    }
}
