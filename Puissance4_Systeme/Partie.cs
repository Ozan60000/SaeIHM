using System;
using System.Collections.Generic;
using System.Text;

namespace Puissance4_Systeme
{
    public class Partie
    {
        public Grille GrilleJeu { get; private set; }
        public Joueur Joueur1 { get; private set; }
        public Joueur Joueur2 { get; private set; }
        public Joueur JoueurCourant { get; private set; }

        public int NbPionsGagnant { get; private set; }
        public bool EstTerminee { get; private set; }
        public Joueur? Gagnant { get; private set; }
        public bool EstEgalite { get; private set; }

        public Partie(int lignes, int colonnes, int nbPionsGagnant, Joueur j1, Joueur j2)
        {
            GrilleJeu = new Grille(lignes, colonnes);
            NbPionsGagnant = nbPionsGagnant;
            Joueur1 = j1;
            Joueur2 = j2;
            JoueurCourant = j1;
            EstTerminee = false;
            Gagnant = null;
            EstEgalite = false;
        }

        public void ChangerJoueur()
        {
            if (JoueurCourant == Joueur1)
                JoueurCourant = Joueur2;
            else
                JoueurCourant = Joueur1;
        }

        public bool JouerCoup(int colonne)
        {
            if (EstTerminee) return false;

            Pion nouveauPion = new Pion(JoueurCourant.Id);
            int ligneResultat = GrilleJeu.PlacerPion(colonne, nouveauPion);

            if (ligneResultat != -1)
            {
                if (GrilleJeu.VerifierVictoire(ligneResultat, colonne, NbPionsGagnant))
                {
                    EstTerminee = true;
                    Gagnant = JoueurCourant;
                    return true;
                }

                if (VerifierGrillePleine())
                {
                    EstTerminee = true;
                    EstEgalite = true;
                    return true;
                }

                ChangerJoueur();
                return true;
            }

            return false;
        }

        private bool VerifierGrillePleine()
        {
            for (int c = 0; c < GrilleJeu.NbColonnes; c++)
            {
                if (!GrilleJeu.EstColonnePleine(c))
                {
                    return false;
                }
            }
            return true;
        }
    }
}