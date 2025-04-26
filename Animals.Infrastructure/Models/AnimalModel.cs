using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animals.Infrastructure.Models
{
    public class AnimalModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public int EnclosureId { get; set; }
        public EnclosureModel Enclosure { get; set; }
    }
}
