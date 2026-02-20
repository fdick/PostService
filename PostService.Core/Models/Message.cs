namespace PostService.Core.Models
{
    public class Message
    {
        public const int MAX_MESSAGE_LENGTH = 5000;

        private Message(Guid ID, Guid threadID, Guid userID, string msg, int likeQuantity, int dislikeQuantity, DateTime createTime, Guid? parentMessageId)
        {
            this.ID = ID;
            ThreadID = threadID;
            Msg = msg;
            LikeQuantity = likeQuantity;
            DislikeQuantity = dislikeQuantity;
            CreateTime = createTime;
            ParentMessageID = parentMessageId;
            UserID = userID;
        }

        public Guid ID { get; }
        public Guid ThreadID { get; }
        public Guid UserID { get; set; }
        public string Msg { get; }
        public int LikeQuantity { get; }
        public int DislikeQuantity { get; }
        public DateTime CreateTime { get; }
        public Guid? ParentMessageID { get; }

        public static (Message, string) Create(Guid ID, Guid threadID, Guid userID, string msg, int likeQuantity, int dislikeQuantity, DateTime createTime, Guid? parentMessageId)
        {
            string error = string.Empty;

            if (string.IsNullOrEmpty(msg))
            {
                error = "Message can not be null";
            }

            var m = new Message(ID, threadID, userID, msg, likeQuantity, dislikeQuantity, createTime, parentMessageId);

            return (m, error);
        }
    }
}
