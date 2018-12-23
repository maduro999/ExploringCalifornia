using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExploreCaliforniaMVCWithData.Models;
using Microsoft.AspNetCore.Mvc;

/*
A Web API is a server-side web service that exposes a lightweight interface and data structures with an emphasis 
on usability and the ability to be consumed by HTTP base devices such as browsers. When it comes down to it, a Web API 
is all about representing your data as a collection of entities, a set of endpoints or URLs that devices can easily 
interact with using standard HTTP verbs.With this approach, each of the standard HTTP verbs GET, PUT, POST and DELETE 
correspond to an action that you can take on an entity. For example, GETs would indicate a request to retrieve an 
entity.PUTs indicate a request to update an existing entity. POSTs indicate a request to create a new entity. 
DELETEs indicate a request to remove or delete an entity from the system. In ASP.NET Core MVC, Web API endpoints 
are nothing but controller actions.
*/

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExploreCaliforniaMVCWithData.api
{
    /*I'll limit this entire API to only comments for a specific blog post instead of loading all comments int
     * the IQueryable<Comment> Get(), by applying 
             * a custom URL pattern that includes the postKey right in the URL. Which ties a specific comment to a blog post*/
    [Route("api/posts/{postKey}/comments")]
    // [Route("api/[controller]")]
    public class CommentsController : Controller
    {

        /* Next, inject the *********************************************************************************
         * BlogDataContext class into the Constructor of the Controller just like I did 
         * with the Blog Controller. Then, I'll implement the first GET method in the api, the one 
         * that returns all the comments, because this is the easiest one to implement. 
        */
        private readonly BlogDataContext _db;
        public CommentsController(BlogDataContext db)
        {
            _db = db;
        }
        //***************************************************************************************************

        // GET: api/<controller>
        [HttpGet]
        public IQueryable<Comment> Get(string postKey) //change the return type of the action to IQueryable<Comment>, and then I can just return the Comments collection directly.
        // public IEnumerable<string> Get()
        {
            /*Then I can update the action to only return comments related to Posts that match that postKey.*/
            //return new string[] { "value1", "value2" };
            return _db.Comments.Where(x => x.Post.Key == postKey);
        }
        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        /*Next, let's look at the Post action that will allow clients to create a new comment using the HttpPost verb. 
         * Like with normal controllers, I can use model binding to accept a Comment object. And MVC will map the payload 
         * of the Post to this object. Notice how the code that Visual Studio generated also prefixed the comment parameter 
         * with the FromBody attribute. This attribute tells model binding to only consider the body of the Http request 
         * when it's attempting to populate the properties of the parameter. Then just like before, I'll add a postKey 
         * parameter to the method so that I know what post the user is commenting on. Then I'll go ahead and implement this method. 
         * The first thing I'm going to do is get the post that the user is referring to. If I don't find this post, I'll simply return null. 
         * If I do find a post, however, I'll attach this comment to that post. With everything populated correctly, I can go ahead and 
         * save the comment to the database. And then return the saved comment back to the client*/
        //public void Post([FromBody]string value)
        public Comment Post(string postKey, [FromBody]Comment comment)
        {
            var post = _db.Posts.FirstOrDefault(x => x.Key == postKey);
            if (post == null)
                return null;

            comment.Post = post;
            comment.Posted = DateTime.Now;
            comment.Author = User.Identity.Name;

            _db.Comments.Add(comment);
            _db.SaveChanges();

            return comment;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
