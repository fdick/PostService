

namespace PostService.Core.Abstractions
{
    public interface IThreadRepository
    {
        Task<Guid> Create(Core.Models.Thread thread);
        Task<Guid> Delete(Guid id);
        Task<List<(Core.Models.Thread, string)>> GetAll();
        Task<Guid> Update(Guid id, string name, Guid authorId);
    }
}