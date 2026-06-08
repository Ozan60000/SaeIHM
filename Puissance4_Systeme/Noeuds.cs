using System.Collections.Generic;

namespace Puissance4_Systeme
{
    public class Noeud
    {
        public int CoupJoue { get; set; }
        public int Score { get; set; }
        public List<Noeud> Enfants { get; set; }

        public Noeud(int coupJoue)
        {
            CoupJoue = coupJoue;
            Enfants = new List<Noeud>();
        }
    }
}