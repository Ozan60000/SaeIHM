using System;
using System.Collections.Generic;
using System.Text;

namespace Puissance4_Systeme
{
    public class Pion
    {
        public int JoueurId { get; private set; }

        public Pion(int id)
        {
            this.JoueurId = id;
        }
    }
}