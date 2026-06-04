using System;
using System.Collections.Generic;
using System.Text;

namespace Puissance4_Systeme
{
    public abstract class Joueur
    {
        public int Id { get; private set; }
        public string Nom { get; private set; }

        protected Joueur(int id, string nom)
        {
            Id = id;
            Nom = nom;
        }

        // Méthode abstraite qui sera redéfinie par l'IA pour calculer son coup
        public abstract int ChoisirCoup(Grille grille);
    }
}

namespace Puissance4_Systeme
{
    public class JoueurHumain : Joueur
    {
        public JoueurHumain(int id, string nom) : base(id, nom)
        {
        }

        public override int ChoisirCoup(Grille grille)
        {
            // Un joueur humain choisit sa colonne en cliquant sur l'IHM (WPF),
            // le moteur n'a donc pas besoin de calculer son coup ici.
            return -1;
        }
    }
}

namespace Puissance4_Systeme
{
    public class JoueurIA : Joueur
    {
        public int Profondeur { get; set; }

        public JoueurIA(int id, string nom, int profondeur = 4) : base(id, nom)
        {
            Profondeur = profondeur;
        }

        public override int ChoisirCoup(Grille grille)
        {
            // TODO: Intégrer l'algorithme Minimax / Alpha-Beta ici
            // Comportement temporaire : joue la première colonne disponible
            for (int c = 0; c < grille.NbColonnes; c++)
            {
                if (!grille.EstColonnePleine(c))
                {
                    return c;
                }
            }
            return -1; // Grille pleine
        }
    }
}