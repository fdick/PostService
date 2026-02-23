using PostService.Core.Abstractions;
using PostService.Core.Models;

namespace PostService.Application.Services
{
    public class MessagesService : IMessagesService
    {
        private readonly IMessagesRepository _msgRepository;

        public MessagesService(IMessagesRepository msgRepository)
        {
            this._msgRepository = msgRepository;
        }

        public async Task<List<(Message, string)>> GetAllMessages()
        {
            return await _msgRepository.GetAll();
        }

        public async Task<List<(Message, string)>> GetMessagesInThreadAsync(Guid threadId)
        {
            return await _msgRepository.GetAllInThread(threadId);

        }

        public async Task<Guid> CreateMessage(Message msg)
        {
            try
            {
                //check for existing parent message by provided ID
                if (msg.ParentMessageID != null)
                {
                    var m = await _msgRepository.GetOne(msg.ParentMessageID.Value);
                    if (m.Item1 == null)
                        throw new Exception("Provided ID doesn't exist!");

                }

                return await _msgRepository.Create(msg);
            }
            catch (Exception e)
            {
                return Guid.Empty;
            }
        }

        public async Task<Guid> UpdateMessage(Guid id, string msg, int likesQuantity, int dislikesQuantity)
        {
            return await _msgRepository.Update(id, msg, likesQuantity, dislikesQuantity);
        }

        public async Task<Guid> DeleteMessage(Guid ID)
        {
            return await _msgRepository.Delete(ID);
        }

    }
}
