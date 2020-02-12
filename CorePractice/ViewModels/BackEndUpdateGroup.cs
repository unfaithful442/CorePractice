using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CorePractice.ViewModels
{
    public class BackEndUpdateGroup
    {
        [Required(ErrorMessage = "GroupId is required")]
        public int GroupId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "GroupName is reuqired")]
        [StringLength(50, ErrorMessage = "The GroupName cannot be longer than 50 characters")]
        public string GroupName { get; set; }

        [StringLength(256, ErrorMessage = "The Description cannot be longer than 256 characters")]
        public string Description { get; set; }

    }
}
