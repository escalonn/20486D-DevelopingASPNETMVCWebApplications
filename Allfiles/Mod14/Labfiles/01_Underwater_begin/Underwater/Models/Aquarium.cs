using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Underwater.Models
{
    public class Aquarium
    {
        [Key]
        public int AquariumId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        [Required]
        public string Number { get; set; }

        public bool Open { get; set; }

        public virtual ICollection<Fish> Fishes { get; set; }
    }
}
