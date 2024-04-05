using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanske.Klasser
{
    public class JudokaTechnique
    {
        public Technique Tech {  get; set; }
        public int Level { get; set; }

        public JudokaTechnique(Technique tech, int level)
        {
            Tech = tech;
            Level = level;
        }
    }
}
