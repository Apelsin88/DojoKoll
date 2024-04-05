using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanske
{
    public class Technique
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Subcategory { get; set; }
        
        public List<Judoka> Judokas { get; set; } = new List<Judoka>();

        public Technique(int id, string name, int level, int subcategory)
        {
            Id = id;
            Name = name;
            Level = level;
            Subcategory = subcategory;

        }
    }
}
