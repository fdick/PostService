namespace PostService.Core.Models
{
    public class Thread
    {
        private Thread(Guid id, string name, Guid authorID)
        {
            this.ID = id;
            Name = name;
            AuthorID = authorID;
        }

        public Guid ID { get; }
        public Guid AuthorID { get; }
        public string Name { get; }

        public static (Thread, string) Create(Guid id, string name, Guid authorId)
        {
            string error = string.Empty;
            if (string.IsNullOrEmpty(name))
            {
                error = $"{nameof(name)} can not be null!";
            }
            var user = new Thread(id, name, authorId);
            return (user, error);
        }
    }
}
