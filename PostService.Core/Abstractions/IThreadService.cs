

namespace PostService.Core.Abstractions
{
    public interface IThreadService
    {
        Task<Guid> CreateThreadAsync(Core.Models.Thread thread);
        Task<Guid> DeleteUserAsync(Guid id);
        Task<List<(Core.Models.Thread, string)>> GetAllThreadsAsync();
        Task<Guid> UpdateUserAsync(Guid id, string name, Guid authorId);
    }
}