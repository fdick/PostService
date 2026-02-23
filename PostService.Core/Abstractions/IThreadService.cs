

namespace PostService.Core.Abstractions
{
    public interface IThreadService
    {
        Task<Guid> CreateThreadAsync(Core.Models.Thread thread);
        Task<Guid> DeleteThreadAsync(Guid id);
        Task<List<(Core.Models.Thread, string)>> GetAllThreadsAsync();
        Task<Guid> UpdateThreadAsync(Guid id, string name, Guid authorId);
    }
}