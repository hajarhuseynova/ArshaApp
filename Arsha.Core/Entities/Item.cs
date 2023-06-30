using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arsha.Core.Entities
{
    public class Item:BaseModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public string? Photo { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
