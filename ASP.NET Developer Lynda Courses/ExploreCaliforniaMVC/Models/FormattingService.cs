using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ExploreCaliforniaMVC.Models
{
    public class FormattingService
    {
        //Centralize the formatting of all dates using this class and allows all dates referencing this class be formatted this way
        public string AsReadableDate(DateTime date)
        {
            return date.ToString("d");
        }
    }
}
