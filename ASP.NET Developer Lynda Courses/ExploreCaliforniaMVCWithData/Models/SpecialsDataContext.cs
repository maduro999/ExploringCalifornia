using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExploreCaliforniaMVCWithData.Models
{
    /*TODO Challenge: If you really want to challenge yourself, you should build a U.I. with controllers and views 
    that allow you to add and modify the Specials data in the database.*/

    //Challenge: Convert the SpecialsDataContext into an entity framework DbContext and then add an I.D. field to the Special class to act as its primary key in the database
    public class Special
    {
        
        public string Id { get; set; } //must have an ID as it acts as the primary key
        public string Key { get; internal set; }
        public string Name { get; internal set; }
        public string Type { get; internal set; }
        public int Price { get; internal set; }
    }

    public class SpecialsDataContext : DbContext
    {
        public DbSet<Special> Specials { get; set; }

        public SpecialsDataContext(DbContextOptions<SpecialsDataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        //Notice how I updated the existing get monthly specials method to just return the collection of Specials from the database. 
        //Creating methods like this on your Db context classes is not only supported, I'd encourage it.
        //You will also need to configure the context in the Startup ConfigureServices. And also add the connection string configuration to the json
        public IEnumerable<Special> GetMonthlySpecials()
        {
            return Specials.ToArray();
        }
    }

    //public class SpecialsDataContext
    //{
    //    //this method returns an array of 'Special' objects - this data is hardcaoded but can come from a web service or db
    //    public IEnumerable<Special> GetMonthlySpecials()
    //    {
    //        return new[]
    //        {
    //            new Special
    //            {
    //                Key= "calm",
    //                Name= "Nerds",
    //                Type= "Candy",
    //                Price= 50
    //            },
    //            new Special
    //            {
    //                Key= "desert",
    //                Name= "M&M",
    //                Type= "Candy",
    //                Price= 20
    //            }
    //        };
    //    }
    //}
}
