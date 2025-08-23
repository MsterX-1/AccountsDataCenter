using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ItemDto
{
    public class CreateUserDto
    {
        [MaxLength(50)]
        public required string UserName { get; set; }
        [MaxLength(50)]
        public required string Password { get; set; }
    }
}
