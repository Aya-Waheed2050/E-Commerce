using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects.BasketModuleDto
{
    public class BasketItemDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;

        [Range(1 , double.MaxValue)]
        public decimal Price { get; set; }

        [Range(1, 99)]
        public int Quantity { get; set; }

    }
}
