using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProiectDaw.Models
{
    public class Review
    {
        public string Id { set; get; }
        public string review { set; get; } 
        public int rating { set; get; }
        public int ProductId { get; set; }

        public virtual  Product  product { get; set; }
    }
}