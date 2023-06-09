using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class CustomerBasketDTO
{
    [Required] public string Id { get; set; }
    public List<BasketItemDTO> BasketItems { get; set; }
}
