using System;

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

        // Méthode abstraite que les enfants devront coder
        public abstract int ChoisirCoup(Grille grille);
    }
}