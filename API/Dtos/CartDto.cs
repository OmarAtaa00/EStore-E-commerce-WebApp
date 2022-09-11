using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class CartDto
    {
        [Required]
        public string Id { get; set; }
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
    }
}
