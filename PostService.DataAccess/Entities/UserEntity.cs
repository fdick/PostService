using System;
using System.Collections.Generic;
using System.Text;

namespace PostService.DataAccess.Entities
{
    public class UserEntity
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public List<MessageEntity> Messages { get; set; } = new List<MessageEntity>();
    }
}
