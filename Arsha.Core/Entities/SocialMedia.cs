using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arsha.Core.Entities
{
    public class SocialMedia:BaseModel
    {
        [Required]
        public string Link { get; set; }
        [Required]
        public string Icon { get; set; }
        [Required]
        public int TeamMembersId { get; set; }
        public TeamMembers? TeamMembers { get; set; }
    }
}
