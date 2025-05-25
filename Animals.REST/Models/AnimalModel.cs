using System.ComponentModel.DataAnnotations;

namespace Animals.REST.Models
{
    public class AnimalModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(0, 100)]
        public int Age { get; set; }

        public int EnclosureId { get; set; }
    }
}
