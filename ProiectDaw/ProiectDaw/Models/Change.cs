using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProiectDaw.Models
{
    public class Change
    {

        public int Id { get; set; }
        public DateTime DateChanged { get; set; }
        public String FieldChanged { get; set; }
        public String NewValue { get; set; }
        public String OldValue { get; set; }
        public int ProductId { get; set; }
        public virtual Product product { get; set; }


        }
            
    }