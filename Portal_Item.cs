

using System;
using System.ComponentModel.DataAnnotations;

namespace Portal_Item
{
    public class PortalItem
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
