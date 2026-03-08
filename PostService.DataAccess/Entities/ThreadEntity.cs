namespace PostService.DataAccess.Entities
{
    public class ThreadEntity
    {
        public Guid ID { get; set; }
        public string Name { get; set; }


        public Guid AuthorID { get; set; }
        public UserEntity Author { get; set; }
        public List<PostEntity>? Posts { get; set; } = new List<PostEntity>();
    }
}
