using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.Repository
{
    interface IRepository
    {
        IEnumerable<Object> GetAll();
        Object Get(int id);
        Object Add(Object item);
        void Remove(int id);
        bool Update(Object item);
    }
}