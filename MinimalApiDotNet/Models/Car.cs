using System.ComponentModel.DataAnnotations;

namespace MinimalApiDotNet.Models
{
    public class Car
    {
        [Required] public string Id { get; set; }
        public string? Model { get; set; }
        public string? Make { get; set; }
        public string? Year { get; set; }
        public int? Amount { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
