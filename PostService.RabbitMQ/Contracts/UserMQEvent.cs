using PostService.RabbitMQ.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostService.RabbitMQ.Contracts
{
    public record UserMQEvent
    {
        public Guid ID { get; set; }
        public string Nickname { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public OperationTypes Operation { get; set; }
        public DateTime OperationTime { get; set; }
    }
}
