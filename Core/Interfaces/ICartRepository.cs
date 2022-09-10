using System;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetCartAsync(string cartId);
        Task<Cart> UpdateOrCreateCart(Cart cart);
        Task<bool> DeleteCartAsync(string cartId);
    }
}
