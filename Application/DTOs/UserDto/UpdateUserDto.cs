using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ItemDto
{
    public class UpdateUserDto
    {
        [MaxLength(50)]
        public string? Name { get; set; }
        [MaxLength(50)]
        public string? Password { get; set; }
    }
}
