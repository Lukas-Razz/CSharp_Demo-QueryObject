namespace Demo.QueryObject.DAL
{
    public class Post
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public long BoardId { get; set; }
        public Board Board { get; set; }
    }
}