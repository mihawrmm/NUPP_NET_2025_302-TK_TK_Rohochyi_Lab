using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animals.Infrastructure.Models
{
    public class EnclosureModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<AnimalModel> Animals { get; set; } = new List<AnimalModel>();
    }
}
