using ExploreCaliforniaMVCWithData.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExploreCaliforniaMVCWithData.ViewComponents
{
  
    [ViewComponent]
    public class MonthlySpecialsViewComponent: ViewComponent
    {

        private readonly SpecialsDataContext _specials;

        public MonthlySpecialsViewComponent(SpecialsDataContext specials)
        {
            //Use dependency injection to inject the SpecialsDataContext class into the constructor of this ViewComponent, and of course
            //to get dependency injection to work it needs to be registered in the Startup Class's ConfigureServices method.
            _specials = specials;
        }



        //A View Component: Consists of both a view and a class that contains the coresponding logic for that view. 
        //Folder name does not matter, however for MVC to detect that this class is a view component it must meet at least 'ONE' of the following criteria.
        //1. The name of the class end with the suffix, ViewComponent. This class satisfies that condition.
        //2. The class to be decorated with [ViewComponent] attribute
        //3. The class can extend from the ViewComponent base class

        //When the component is invoked, core mvc looks for a public method on the component class named invoke. Which accepts the same number
        //of params that the component was invoked with, and returns an instance of IViewComponentResult

        public IViewComponentResult Invoke()
        {
            var specials = _specials.GetMonthlySpecials();
            return View(specials);
        }

    }
}
