using PostService.Core.Models;

namespace PostService.Core.Abstractions
{
    public interface IUserService
    {
        Task<Guid> CreateUser(User user);
        Task<Guid> DeleteUser(Guid id);
        Task<List<(User, string)>> GetAllUsers();
        Task<Guid> UpdateUser(Guid id, string name, string lastname, string email, string nickname);
    }
}