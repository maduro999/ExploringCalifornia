using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExploreCaliforniaMVCWithData.Models
{
    public class Comment
    {
        /*
        you need to have a class defined in order to use Entity Framework to interact with a database.
        So, I'll start by creating the Comment Class in the Models folder, and populate it with a few properties.
        Notice the PostId and Post properties. It's no coincidence that these two properties are named similarly. 
        Creating the Post property of type Post and the corresponding long property named PostId tells Entity Framework 
        that this Comment entity is related to the post entity, which will allow you to reference the Post object as if it 
        were part of the Comment object itself. Then, just like with the Post entity, I have to add this entity to a 
        data context in order to interact with it as a database table. To do this, I'll open up the 
        BlogDataContext class and add a new DB set property named Comments.
        */

        public long Id { get; set; }

        public long PostId { get; set; }
        public virtual Post Post { get; set; }

        public DateTime Posted { get; set; }
        public string Author { get; set; }

        [Required]
        public string Body { get; set; }

    }
}
