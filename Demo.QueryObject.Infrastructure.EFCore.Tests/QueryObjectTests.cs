using Demo.QueryObject.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.QueryObject.Infrastructure.EFCore.Tests
{
    public class QueryObjectTests
    {
        private DbContextOptions<BoardDbContext> _options;

        public QueryObjectTests()
        {
            // This is needed to have fresh in-memory database with each new DbContext
            // Kudos to Nate Barbettini: https://stackoverflow.com/a/38897560 
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            _options = new DbContextOptionsBuilder<BoardDbContext>()
                .UseInMemoryDatabase(databaseName: $"test_db_{Guid.NewGuid}")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            using var _boardDbContext = new BoardDbContext(_options);

            _boardDbContext.Boards.Add(new Board { Id = 1, Name = "What have you found today?", Symbol = "wftd" });
            _boardDbContext.Posts.Add(new Post { Id = 1, Content = "A Lamp", BoardId = 1 });
            _boardDbContext.Posts.Add(new Post { Id = 2, Content = "A Tricoder", BoardId = 1 });
            _boardDbContext.Posts.Add(new Post { Id = 3, Content = "A Bear", BoardId = 1 });
            _boardDbContext.Posts.Add(new Post { Id = 4, Content = "Nothing :/", BoardId = 1 });

            _boardDbContext.SaveChanges();
        }

        [Fact]
        public async Task Where_OneValidPredicate_Filtered()
        {
            using var _boardDbContext = new BoardDbContext(_options);
            var queryObject = new EFCoreQueryObject<Post>(_boardDbContext);
            queryObject.Filter(x => x.Content.StartsWith("A"));

            var result = await queryObject.ExecuteAsync();

            Assert.True(result.Count() == 3);
        }

        [Fact]
        public async Task Where_OneValidPredicateOrdered_FilteredOrdered()
        {
            using var _boardDbContext = new BoardDbContext(_options);
            var queryObject = new EFCoreQueryObject<Post>(_boardDbContext);
            queryObject
                .Filter(x => x.Content.StartsWith("A"))
                .OrderBy(x => x.Content);

            var result = await queryObject.ExecuteAsync();

            Assert.True(result.First().Id == 3);
            Assert.True(result.Count() == 3);
        }

        [Fact]
        public async Task Where_OneValidPredicateOrderedPaged_FilteredOrderedPaged()
        {
            using var _boardDbContext = new BoardDbContext(_options);
            var queryObject = new EFCoreQueryObject<Post>(_boardDbContext);
            queryObject
                .Filter(x => x.Content.StartsWith("A"))
                .OrderBy(x => x.Content)
                .Page(2, 1);

            var result = await queryObject.ExecuteAsync();

            Assert.Single(result);
            Assert.True(result.First().Id == 1);
        }
    }
}