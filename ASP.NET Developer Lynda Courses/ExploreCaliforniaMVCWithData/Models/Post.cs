using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExploreCaliforniaMVCWithData.Models
{
    //pass this strongly typed model in the view instead of ViewBag
    public class Post
    {
        /* Entity Framework uses the convention of calling this key property Id so I'll add a property with that name to the post class 
         * and use the long type to indicate that it should be an incremental numeric id. And, while I'm in here, I'll create another 
         * property named key that will give us a unique string that we can use to find the post as opposed to passing around ugly id 
         * numbers everywhere. That's all the code that's required to define the Db context that Entity Framework needs.
         */
        public long Id { get; set; }
        private string _key;
        public string Key
        {
            get
            {
                if (_key == null)
                {
                    _key = Regex.Replace(Title.ToLower(), "[^a-z0-9]", "-");
                }
                return _key;
            }
            set { _key = value; }
        }
        //---------------------------------------------------------------------------------------------------------------------------------

        //form validation - data annotation validation attributes .NET has a bunch of them. Check online documentation.
        //Since our form is posting data via the post class, let's just start decorating the Post class with validation rules
        [Display(Name = "Blog Post Title")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Must be between 5 and 100 Charachters")]
        public string Title { get; set; }

        public string Authors { get; set; }

        [Required]
        [MinLength(100, ErrorMessage = "Post must be at leaast 100 charachters")]
        [DataType(DataType.MultilineText)] //because thisdatatypeis set to multilineText I can use the EditorFor instead of the TextAreaFor, the The EditorFor
        //will determine the datatype to be used based on the attribute, which is MultilineText in this case.
        public string Body { get; set; }

        public DateTime Posted { get; set; }
    }
}
