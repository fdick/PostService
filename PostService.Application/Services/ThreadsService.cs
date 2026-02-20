using PostService.Core.Abstractions;

namespace PostService.Application.Services
{
    public class ThreadsService : IThreadService
    {
        private readonly IThreadRepository _repository;

        public ThreadsService(IThreadRepository repository)
        {
            this._repository = repository;
        }

        public async Task<List<(Core.Models.Thread, string)>> GetAllThreads()
        {
            return await _repository.GetAll();
        }

        public async Task<Guid> CreateThread(Core.Models.Thread thread)
        {
            return await _repository.Create(thread);
        }

        public async Task<Guid> DeleteUser(Guid id)
        {
            return await _repository.Delete(id);
        }

        public async Task<Guid> UpdateUser(Guid id, string name, Guid authorId)
        {
            return await _repository.Update(id, name, authorId);
        }
    }
}
