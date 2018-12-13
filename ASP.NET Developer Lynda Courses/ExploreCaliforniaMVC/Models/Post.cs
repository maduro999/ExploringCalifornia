using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExploreCaliforniaMVC.Models
{
    //pass this strongly typed model in the view instead of ViewBag
    public class Post
    {
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Body { get; set; }
        public DateTime Posted { get; set; }
    }
}
