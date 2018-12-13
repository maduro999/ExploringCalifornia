using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExploreCaliforniaMVCWithData.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExploreCaliforniaMVCWithData.Controllers
{
    [Route("blog")] //route attribs can also be applied to the controller itself and all the actions will inherit this
    public class BlogController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            var posts = new []
            {
                new Post{
                    Title = "First Blog Post!",
                    Authors = "Rui",
                    Body = "RM",
                    Posted = DateTime.Now,
                },
                new Post{
                    Title = "First Blog Post!",
                    Authors = "Rui",
                    Body = "RM",
                    Posted = DateTime.Now,
                },

            };

            return View(posts);
            //return new ContentResult { Content = "Blog Posts Controller" };
        }

        //To customize the url for this controller action - we place the route attribute on top of the action
        //and pass it a string param that defines the custom route pattern to apply to just this action.
        //Since we want to url to include the year, month and title of the post, the pattern will look like this.
        //the url then /blog/2018/04/BlogName
        //[Route("blog/{year:int}/{month:int}/{key}")]
        //same as above but with date constraints- however; if the requested route falls outside of this range it will display a blank page
        //[Route("blog/{year:min(2000)}/{month:range(1,12)}/{key}")]
        //[Route("{year:min(2000)}/{month:range(1,12)}/{key?}")] the ? means the parameter is options and if not info is given for that param the route will still match and the page will display
        [Route("{year:min(2000)}/{month:range(1,12)}/{key}")] //the first part of this route "blog" is being inherited at the controller level
        public IActionResult Post(int year, int month, string key) 
        {

            //using viewbag to pass data from a controller to the view -
            //this has been replaced by a strongly typed model that will be passed to the view instead.
            //ViewBag.Title = "First Blog Post!";
            //ViewBag.Posted = DateTime.Now;
            //ViewBag.Author = "Rui";
            //ViewBag.Initials = "RM";

            //return View();

            var post = new Post //Set the properties on an instance of this new Post class
            {
                Title = "First Blog Post!",
                Authors = "Rui",
                Body = "RM",
                Posted = DateTime.Now,
             
            };

            return View(post); //Pass the instance right to the view in a parameter

            //return new ContentResult { Content = string.Format("Year: {0}; Month: {1}; Key {2}", year, month, key) };

            //if (id == null)
            //{
            //    return new ContentResult { Content = "null" };
            //}
            //else
            //{
            //    return new ContentResult { Content = id.ToString() };
            //}
        }

        //public IActionResult Post(int id)
        //{
        //    return new ContentResult { Content = id.ToString()};
        //}


        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }


    }
}
