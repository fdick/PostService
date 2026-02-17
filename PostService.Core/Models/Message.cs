namespace PostService.Core.Models
{
    public class Message
    {
        public const int MAX_MESSAGE_LENGTH = 5000;

        private Message(Guid ID, Guid threadID, string msg, int likeQuantity, int dislikeQuantity, DateTime createTime, List<Guid> subMessages)
        {
            this.ID = ID;
            ThreadID = threadID;
            Msg = msg;
            LikeQuantity = likeQuantity;
            DislikeQuantity = dislikeQuantity;
            CreateTime = createTime;
            SubMessagesIDs = subMessages;
        }

        public Guid ID { get; }
        public Guid ThreadID { get; }
        public string Msg { get; }
        public int LikeQuantity { get; }
        public int DislikeQuantity { get; }
        public DateTime CreateTime { get; }

        public List<Guid> SubMessagesIDs { get; }

        public static Message Create(Guid ID, Guid threadID, string msg, int likeQuantity, int dislikeQuantity, DateTime createTime, List<Guid> subMessages)
        {
            return new Message(ID, threadID, msg, likeQuantity, dislikeQuantity, createTime, subMessages);
        }
    }
}
