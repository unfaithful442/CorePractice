using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CorePractice.ViewModels
{
    public class BackEndUpdateUser
    {
        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }
        public DateTime DateOfBirth { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        [StringLength(256, ErrorMessage = "The Email cannot be longer than 256 characters")]
        [RegularExpression(pattern: @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", ErrorMessage = "Email Address is Invalid")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Firstname is required")]
        [StringLength(100, ErrorMessage = "The Firstname cannot be longer than 100 characters")]
        public string Firstname { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Lastname is required")]
        [StringLength(100, ErrorMessage = "The Lastname cannot be longer than 100 characters")]
        public string Lastname { get; set; }

        [StringLength(50, ErrorMessage = "The Mobile Number cannot be longer than 50 characters")]
        public string Mobile { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [StringLength(128, ErrorMessage = "The Password  cannot be longer than 128 characters")]
        [RegularExpression(pattern: @"(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,})$", ErrorMessage = "Password should be minimum 8 characters in length and contain both letters and numbers, at least one uppercase lette")]
        public string Password { get; set; }

        [StringLength(50, ErrorMessage = "The Phone Number cannot be longer than 50 characters")]
        public string Phone { get; set; }

    }
}
