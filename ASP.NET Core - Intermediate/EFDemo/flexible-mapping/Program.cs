using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace flexible_mapping
{
    class Program
    {
        static void Main(string[] args)
        {
            SetupDatabase();

            using (var db = new BloggingContext())
            {
                var blog = new Blog { Name = ".NET Blog" };

                blog.SetUrl("https://blogs.msdn.microsoft.com/dotnet");

                db.Blogs.Add(blog);
                db.SaveChanges();
            }

            using (var db = new BloggingContext())
            {
                var blog = db.Blogs.Single();

                Console.WriteLine($"{blog.Name}: {blog.Url}");
            }

            Console.ReadLine();
        }

        private static void SetupDatabase()
        {
            using (var db = new BloggingContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }
    }

    public class BloggingContext : DbContext
    {
        private static readonly ILoggerFactory _loggerFactory = new LoggerFactory()
            .AddConsole((s, l) => l == LogLevel.Information && s.EndsWith("Command"));

        public DbSet<Blog> Blogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(
                    @"Server=RAMDAS\SQLEXPRESS;Database=Demo.FlexibleMapping;Trusted_Connection=True;ConnectRetryCount=0;")
                .UseLoggerFactory(_loggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set up a field mapping
            modelBuilder.Entity<Blog>()
                .Property<string>("Url")
                .HasField("_url");

            modelBuilder.Entity<Blog>()
                .HasMany(b => b.Posts)
                .WithOne()
                .HasForeignKey(p => p.BlogFK);
                
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }

        private string _url;

        public string Url { get { return _url; } }

        public void SetUrl(string url)
        {
            // Perform some domain logic...

            _url = url;
        }

        public ICollection<Post> Posts { get; set; }
    }

    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int BlogFK { get; set; }
    }
}
