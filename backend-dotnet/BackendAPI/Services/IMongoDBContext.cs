using MongoDB.Driver;
using BackendAPI.Models;

namespace BackendAPI.Services
{
    public interface IMongoDBContext
    {
        IMongoCollection<User> Users { get; }
    }
} 