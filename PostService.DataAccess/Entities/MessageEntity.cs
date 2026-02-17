namespace PostService.DataAccess.Entities
{
    public class MessageEntity
    {
        public Guid ID { get; set; }
        public Guid ThreadID { get; set; }
        public string Message { get; set; } = string.Empty;
        public int LikeQuantity { get; set; }
        public int DislikeQuantity { get; set; }
        public DateTime CreateTime { get; set; }

        public List<Guid> SubMessagesIDs { get; set; } = new List<Guid>();
    }
}
