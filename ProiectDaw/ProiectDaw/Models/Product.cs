using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectDaw.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Titlul este obligatoriu")]
        [StringLength(20, ErrorMessage = "Titlul nu poate avea mai mult de 20 caractere")]
        public string Title { get; set; }


        [Required(ErrorMessage = "Descrierea produsului este obligatorie")]
        public string Description { get; set; }


        [Required(ErrorMessage = "Pretul produsului este obligatoriu")]
         public decimal Price { get; set; }


        [Range(0, 5, ErrorMessage = "Ratingul este intre 0 si 5")]
        public float Rating { get; set; }

        [Required(ErrorMessage = "Categoria este obligatorie")]
        public int CategoryId { get; set; }

        public ICollection<int> ReviewsId{ get; set; }
       
        public DateTime LastDateUpdated { get; set; }

        public String Image_path { get; set; }

        public DateTime Date { get; set; }

        public bool IsApproved { get; set; }
        
        //public IEnumerable<int> ChangesId { get; set; }
        //[DataType(DataType.DateTime, ErrorMessage = "Campul trebuie sa contina data si ora")]
        public virtual Category Category { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Change> Changes { get; set; }


    }


}
    
