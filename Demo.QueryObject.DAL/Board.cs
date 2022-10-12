namespace Demo.QueryObject.DAL
{
    public class Board
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}