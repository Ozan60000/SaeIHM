using System;
using System.Collections.Generic;
using System.Text;

namespace Puissance4_Systeme
{
    public class Pion
    {
        // Identifiant du joueur (ex: 1 pour le Joueur 1, 2 pour le Joueur 2)
        public int JoueurId { get; private set; }

        public Pion(int id)
        {
            this.JoueurId = id;
        }
    }
}
