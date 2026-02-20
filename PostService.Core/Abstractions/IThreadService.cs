

namespace PostService.Core.Abstractions
{
    public interface IThreadService
    {
        Task<Guid> CreateThread(Core.Models.Thread thread);
        Task<Guid> DeleteUser(Guid id);
        Task<List<(Core.Models.Thread, string)>> GetAllThreads();
        Task<Guid> UpdateUser(Guid id, string name, Guid authorId);
    }
}