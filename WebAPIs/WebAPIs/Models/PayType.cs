using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models
{
    public enum PayType : int
    {
        Treat = 0,
        Exam,
        Pres,
        Surg,
        Hosp
    }
}