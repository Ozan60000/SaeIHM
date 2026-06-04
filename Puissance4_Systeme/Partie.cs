using System;
using System.Collections.Generic;
using System.Text;

using System;

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
        public Joueur? Gagnant { get; private set; } // Ajoute le point d'interrogation ici
        public bool EstEgalite { get; private set; }

        public Partie(int lignes, int colonnes, int nbPionsGagnant, Joueur j1, Joueur j2)
        {
            GrilleJeu = new Grille(lignes, colonnes);
            NbPionsGagnant = nbPionsGagnant;
            Joueur1 = j1;
            Joueur2 = j2;
            JoueurCourant = j1; // Le joueur 1 commence toujours
            EstTerminee = false;
            Gagnant = null;
            EstEgalite = false;
        }

        // Alterne le tour entre les deux joueurs
        public void ChangerJoueur()
        {
            if (JoueurCourant == Joueur1)
                JoueurCourant = Joueur2;
            else
                JoueurCourant = Joueur1;
        }

        // Traite l'action de jouer un jeton dans une colonne
        public bool JouerCoup(int colonne)
        {
            if (EstTerminee) return false;

            // Crée le pion associé à l'ID du joueur qui est en train de jouer
            Pion nouveauPion = new Pion(JoueurCourant.Id);
            int ligneResultat = GrilleJeu.PlacerPion(colonne, nouveauPion);

            // Si le placement est réussi (colonne non pleine et valide)
            if (ligneResultat != -1)
            {
                // 1. Vérification de la victoire
                if (GrilleJeu.VerifierVictoire(ligneResultat, colonne, NbPionsGagnant))
                {
                    EstTerminee = true;
                    Gagnant = JoueurCourant;
                    return true;
                }

                // 2. Vérification de l'égalité (grille pleine)
                if (VerifierGrillePleine())
                {
                    EstTerminee = true;
                    EstEgalite = true;
                    return true;
                }

                // 3. Si pas de fin de match, on passe au joueur suivant
                ChangerJoueur();
                return true;
            }

            return false; // Coup impossible (colonne pleine)
        }

        // Méthode privée pour tester si la grille est totalement remplie
        private bool VerifierGrillePleine()
        {
            for (int c = 0; c < GrilleJeu.NbColonnes; c++)
            {
                if (!GrilleJeu.EstColonnePleine(c))
                {
                    return false; // Il reste au moins une colonne jouable
                }
            }
            return true; // Toutes les colonnes sont pleines
        }
    }
}