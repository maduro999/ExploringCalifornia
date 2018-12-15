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
        private readonly BlogDataContext _db;
        //Inject an instance of the blog data context class into the constructor.
        //Now that we have that, I can add the code _db.Posts.Add(post);_db.SaveChanges(); to the create action to save the newly submitted post to the database.  
        public BlogController(BlogDataContext db)
        {
            _db = db;
        }

        [Route("")]
        public IActionResult Index()
        {
            /*he nice thing about the DbSet Type is that it acts like a normal collection, which means that you can interact with it through link statements. 
             * For example, if I want to get all of the blog posts to display on the blog landing page, I'll simply replace the existing hard-coded data 
             * in the IndexAction with a call to ToArray on the Posts Property.
             * This will retrieve all of the posts from the database, and this is not actually what I want to do here. 
             * I really just want to get the couple of latest posts. So what I really want to do is order the posts by their 
             * posted date with the most recent first and then take the first couple of them. Once I've done that, 
             * I can navigate to the blog landing page and see these posts.
             */
            //var posts = _db.Posts.ToArray();
            var posts = _db.Posts.OrderByDescending(x => x.Posted).Take(5).ToArray();
            //var posts = new []
            //{
            //    new Post{
            //        Title = "First Blog Post!",
            //        Authors = "Rui",
            //        Body = "RM",
            //        Posted = DateTime.Now,
            //    },
            //    new Post{
            //        Title = "First Blog Post!",
            //        Authors = "Rui",
            //        Body = "RM",
            //        Posted = DateTime.Now,
            //    },

            //};

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

            //var post = new Post //Set the properties on an instance of this new Post class
            //{
            //    Title = "First Blog Post!",
            //    Authors = "Rui",
            //    Body = "RM",
            //    Posted = DateTime.Now,

            //};
            //update this Post action to read the individual Post from the data context
            var post = _db.Posts.FirstOrDefault(x => x.Key == key);

            return View(post); //Pass the instance right to the view in a parameter
        }

        //controller action to show form - using the HttpGet decorator
        [HttpGet, Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        //controller action to handle form post - using the HttpPost decorator
        [HttpPost, Route("create")]
        public IActionResult Create(Post post) //method signature must be different from the one above, despite the attributes HttpGet and HttpPost. So a param can be added.
        {//Here we are passing a complex type. The Post object as a param to thsi controller action. There are some properties in the Post class that a user should not
            //be posting values for, such as the Author or Date (Posted). Three ways to handle this below. In this case I am using option 3, but it not always be this way.
            //It varies based on situation.

            if (!ModelState.IsValid) //this returns a simple yes or no answer as to whether the user has satisfied all of the model validation rules in the Post class
            {
                return View();
            }

            post.Authors = User.Identity.Name;
            post.Posted = DateTime.Now;

            /* Notice how saving the post to the database is a two step process. Entity Framework works on the Unit of Work pattern, 
             * which is really just a fancy way of saying that first you tell the data context everything that you want to do. For example, 
             * adding a new post to the database. Then, the second step is to tell the data context to actually go ahead and execute what you've asked for, 
             * which we do by calling the save changes */
            _db.Posts.Add(post);
            _db.SaveChanges();

            //return View();
            //update the Create action to Redirect to the new Post once it's been successfully created. 
            //Notice how I'm passing in the parameters using an anonymous type, just like the ActionLink Helper in the View.
            return RedirectToAction("Post", "Blog", new
            {
                year = post.Posted.Year,
                month = post.Posted.Month,
                key = post.Key
            });

        }

        /* ----------------------------------------------------------------
        1.[HttpPost, Route("create")]
        public IActionResult Create(CreatePostRequest post) 
        {
            return View();
        }
        public class CreatePostRequest //only define the properties you want the user to submit data for - downside is you have to maintain a this new type
        {
            public string Title {get; set;}
            public string Body {get; set;}
        }
        --------------------------------------------------------------------
        2.[HttpPost, Route("create")]
        public IActionResult Create([Bind("Title", "Body")]Post post) 
        {                        //white list of all the properties a user is allowed to specify values for. Using the Bind attribute. - Downside is difficult to maintain.
            return View();
        }
        -------------------------------------------------------------------
        3.[HttpPost, Route("create")]
        public IActionResult Create(Post post) //explicitely overwrite the fields the user submits with the values that I want them to be, regardless of what the user passes in
        {                                       //Or is they pass anything in at all for these two fields.
            post.Authors = User.Identity.Name;
            post.Posted = DateTime.Now;
            return View();
        }

        */


    }
}
