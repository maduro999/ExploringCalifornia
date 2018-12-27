using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExploreCaliforniaMVCWithData.Models;
using Microsoft.AspNetCore.Mvc;

/* CHALLENGE***************************
 * I only implemented two of the HTTP verbs that were in the code that Visual Studio generated for me, GET and POST. 
 * Your challenge is to go back and finish what I started, implementing all of the GET, PUT, POST and DELETE 
 * methods that I didn't get around to implementing*/

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
// API call example - for thie particular post "testing-post-4 and comment Id 4"http://localhost:58135/api/posts/testing-post-4/comments/4
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

        /*Challenge Solution*********************/
            // GET api/<controller>/5
            //[HttpGet("{id}")]
            //public string Get(int id)
            //{
            //    return "value";
            //}
        // GET api/values/5
        /*First, I took the get API that allows consumers to retrieve a single comment by its ID. The first thing 
         * I did was to change the return type of the action because the action should return a comment, not a string. 
         * Then I changed the int parameter to a long, to match the type of the ID property on the comment class. 
         * And finally, the good stuff. I found the comment that matches this ID in the comments collection.*/
        [HttpGet("{id}")]
        public Comment Get(long id)
        {

            var comment = _db.Comments.FirstOrDefault(x => x.Id == id);
            return comment;
        }
        /***************************************/

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

        /*Challenge Solution*********************/
            // PUT api/<controller>/5
            //[HttpPut("{id}")]
            //public void Put(int id, [FromBody]string value)
            //{
            //}
        /*The important thing here is really just remembering that in a web API, the put verb means that you want to update an existing entity. 
         *I'll modify the signature to take in the ID of the comment that should be updated, as well as a comment object to accept the updated 
         *comment data. Then, once again, I'll find the existing comment by its ID and, if I don't find it, I'll return an action result, indicating 
         *that it wasn't found. Of course, when I do this I need to update the signature from void to IActionResult. When it is found, however, 
         *I'll just copy the values over. 
         *Since this is a simple object, the only thing that I want the user to be able to update is the body property. And once I've updated 
         *everything, I can save that updated comment and respond to the request that everything was okay.*/
        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody]Comment updated)
        {
            var comment = _db.Comments.FirstOrDefault(x => x.Id == id);

            if (comment == null)
                return NotFound();

            comment.Body = updated.Body;

            _db.SaveChanges();

            return Ok();
        }
        /* to put (update comment) for example you have to include the body of the comment
         * { //this is the api call http://localhost:58135/api/posts/testing-post-4/comments/4
                "id": 4, <---- this is the comment Id (when calling the api it already contains this key so it can be omitted here) 
                "postId": 5, <---- the post id that the comment is related to (can also be omitted because the postId is related to the PostKey which is the post name in the api call (testing-post-4) in this case.
                "post": null,
                "posted": "2018-12-26T23:40:38.8879755", <---- date time posted
                "author": null,
                "body": "testing 123456789" <--- the updated body of the comment
            }
         */
        /***************************************/



        /*Challenge Solution*********************/
        //// DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
        /*While I will leave the return type as void, I'll change the int parameter to a long parameter to match the comment class again. 
         * Then I'll use the same code as before to locate the comment by its ID. If I find the comment, I'll remove it from the database 
         * by simply removing it from the collection. And don't forget to run save changes when you're done.*/
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(long id)
        {
            var comment = _db.Comments.FirstOrDefault(x => x.Id == id);

            if (comment != null)
            {
                _db.Comments.Remove(comment);
                _db.SaveChanges();
            }
        }

        /***************************************/

    }
}
