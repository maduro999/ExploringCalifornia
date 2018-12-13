using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExploreCaliforniaMVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        //A controller is a container for an Action
        //Every public method in a controller class is an Action
        //Action Method


        //public string Index()
        //{

        //    return "Test";
        //}

        public IActionResult Index()
        {
            return View();
            //every controller action return a content result - asp.net converts it into a string or you can do it yourself
            //return new ContentResult { Content = "Home Controller Index"} ;

        }
    }
}
