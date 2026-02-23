using PostService.Core.Models;

namespace PostService.Core.Abstractions
{
    public interface IUserService
    {
        Task<Guid> CreateUserAsync(User user);
        Task<Guid> DeleteUserAsync(Guid id);
        Task<List<(User, string)>> GetAllUsersAsync();
        Task<Guid> UpdateUserAsync(Guid id, string name, string lastname, string email, string nickname);
    }
}