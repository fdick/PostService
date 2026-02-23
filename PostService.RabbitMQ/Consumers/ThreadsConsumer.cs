using MassTransit;
using PostService.Core.Abstractions;
using PostService.Core.Models;
using PostService.RabbitMQ.Contracts;
using PostService.RabbitMQ.Enums;

namespace PostService.RabbitMQ.Consumers
{
    public class ThreadsConsumer : IConsumer<ThreadMQEvent>
    {
        private readonly IThreadService _threadService;

        public ThreadsConsumer(IThreadService threadService)
        {
            this._threadService = threadService;
        }
        public async Task Consume(ConsumeContext<ThreadMQEvent> context)
        {
            var thread = PostService.Core.Models.Thread.Create(
                context.Message.ID,
                context.Message.Name,
                context.Message.AuthorID
                );

            if (!string.IsNullOrEmpty(thread.Item2))
                throw new Exception();

            switch (context.Message.Operation)
            {
                case OperationTypes.Create:
                    var g = await _threadService.CreateThreadAsync(thread.Item1);
                    break;
                case OperationTypes.Update:
                    await _threadService.UpdateThreadAsync(
                        thread.Item1.ID,
                        thread.Item1.Name,
                        thread.Item1.AuthorID
                        );
                    break;
                case OperationTypes.Delete:
                    await _threadService.DeleteThreadAsync(thread.Item1.ID);

                    break;
                default:
                    break;
            }

        }
    }
}
