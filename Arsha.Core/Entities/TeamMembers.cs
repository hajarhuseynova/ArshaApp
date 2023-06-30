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
    public class TeamMembers:BaseModel
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
        [Required]
        public string Description { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
        [Required]
        public int PositionId { get; set; }
        public Position? Position { get; set; }
        public List<SocialMedia>? SocialMedias { get; set; }
    }
}
