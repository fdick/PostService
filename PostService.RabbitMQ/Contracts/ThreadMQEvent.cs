using PostService.RabbitMQ.Enums;

namespace PostService.RabbitMQ.Contracts
{
    public record ThreadMQEvent
    {
        public Guid ID { get; }
        public Guid AuthorID { get; }
        public string Name { get; }
        public OperationTypes Operation { get; set; }
        public DateTime OperationTime { get; set; }
    }
}
