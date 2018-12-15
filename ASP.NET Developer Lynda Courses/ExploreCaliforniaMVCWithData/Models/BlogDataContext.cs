using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ExploreCaliforniaMVCWithData.Models
{
    public class BlogDataContext : DbContext //inherit EF base class
    {
        /* Entity Framework is an object relational mapper or ORM, which is really just a fancy way of saying that it's a tool 
         * that allows you to save and retrieve data to and from a relational database using objects rather than hand coded sequel queries.
         * 
         * 
         * In order for the data context to actually be useful to us, we need to define properties on the class 
         * that represent the tables in the database. And the way that we do that is with the type Db set of T. For example, 
         * in order to persist and retrieve blog post objects to our database we can define a new post property with the following 
         * signature. DbSet of T is an Entity Framework type that exposes a simple by elegant API that represents a table in the database 
         * as a strongly typed collection of objects that we can use the link query syntax to query and filter or even insert or 
         * delete records with a call to a C sharp method. We do not to write any SQL to interact with the database.
         * 
         * The name of Db Set properties are also crucial as they represent the name of the corresponding table in the database. 
         * For instance, Entity Framework will query a table named Posts in the database in order to find and satisfy queries to the 
         * post property shown here. In order to be a valid database entity however, the post class must define a 
         * property that will act as the primary key in the database. See Post class Key.
         * 
         * But, we still need to do some more configuration before we can actually use it to connect to a database. 
         * First, I need to register the data context class with the dependency injection framework so that consumers 
         * don't need to know how to create instances of it.
         * 
         * */

        public DbSet<Post> Posts { get; set; }


        //add a constructor that accepts a Db context options object and just passes it to the Db context base class like this.
        public BlogDataContext(DbContextOptions<BlogDataContext> options) : base(options)
        {
            /* there is one last thing that I like to do especially during development and that is to make sure that the database 
             * actually exists before I try to connect to it. I can ask Entity Framework to do this for me by putting the following 
             * line in the constructor of the data context class. With this in place Entity Framework will check to make sure that 
             * database exists. Prior to making any calls to it and if it doesn't exist, Entity Framework will automatically 
             * generate the sequel schema required to create the database and then go ahead and execute that generated sequel 
             * to actually create that brand new database for us.
             */
            Database.EnsureCreated();

        }
    }
}
