using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExploreCaliforniaMVC.Models
{

    public class Special
    {
        public string Key { get; internal set; }
        public string Name { get; internal set; }
        public string Type { get; internal set; }
        public int Price { get; internal set; }
    }
    public class SpecialsDataContext
    {
        //this method returns an array of 'Special' objects - this data is hardcaoded but can come from a web service or db
        public IEnumerable<Special> GetMonthlySpecials()
        {
            return new[]
            {
                new Special
                {
                    Key= "calm",
                    Name= "Nerds",
                    Type= "Candy",
                    Price= 50
                },
                new Special
                {
                    Key= "desert",
                    Name= "M&M",
                    Type= "Candy",
                    Price= 20
                }
            };
        }
    }
}
