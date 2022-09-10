using System;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Data
{
    //This is not generic repository cuz Redis not manged by EFCore 
    public class CartRepository : ICartRepository
    {
        private readonly IDatabase _database;
        public CartRepository(IConnectionMultiplexer redis)
        {

            _database = redis.GetDatabase();
        }
        public async Task<Cart> GetCartAsync(string cartId)
        {
            var data = await _database.StringGetAsync(cartId);
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Cart>(data);
        }
        public async Task<Cart> UpdateOrCreateCart(Cart cart)
        {
            var created = await _database.StringSetAsync(cart.Id,
            JsonSerializer.Serialize(cart), TimeSpan.FromDays(15));

            if (!created) return null;
            return await GetCartAsync(cart.Id);
        }
        public async Task<bool> DeleteCartAsync(string cartId)
        {
            return await _database.KeyDeleteAsync(cartId);
        }


    }
}
