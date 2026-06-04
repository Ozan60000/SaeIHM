using System;
using System.Collections.Generic;
using System.Text;

namespace Puissance4_Systeme
{
    public class Grille
    {
        private Pion[,] _plateau;

        public int NbLignes { get; private set; }
        public int NbColonnes { get; private set; }

        public Grille(int lignes, int colonnes)
        {
            NbLignes = lignes;
            NbColonnes = colonnes;
            _plateau = new Pion[lignes, colonnes]; // Par défaut, toutes les cases sont à null
        }

        // Vérifie si on peut encore jouer dans cette colonne (si la case tout en haut est vide)
        public bool EstColonnePleine(int colonne)
        {
            return _plateau[0, colonne] != null;
        }

        // Fait tomber un pion dans la colonne choisie
        public int PlacerPion(int colonne, Pion pion)
        {
            // Sécurité : si la colonne n'existe pas ou est pleine
            if (colonne < 0 || colonne >= NbColonnes || EstColonnePleine(colonne))
            {
                return -1; // Coup invalide
            }

            // On parcourt la colonne de bas en haut pour simuler la gravité
            for (int l = NbLignes - 1; l >= 0; l--)
            {
                if (_plateau[l, colonne] == null)
                {
                    _plateau[l, colonne] = pion;
                    return l; // On retourne l'index de la ligne où le pion s'est arrêté
                }
            }
            return -1;
        }

        // Permet de récupérer un pion à une case précise (très utile pour vérifier la victoire plus tard)
        public Pion GetPion(int ligne, int colonne)
        {
            if (ligne < 0 || ligne >= NbLignes || colonne < 0 || colonne >= NbColonnes)
                return null;

            return _plateau[ligne, colonne];
        }

        // Méthode principale pour vérifier si le dernier coup est gagnant
        public bool VerifierVictoire(int derniereLigne, int derniereColonne, int nbAAligner)
        {
            Pion pionJoue = _plateau[derniereLigne, derniereColonne];
            if (pionJoue == null) return false; // Sécurité

            int idJoueur = pionJoue.JoueurId;

            // Tableau représentant les 4 axes à vérifier : 
            // {décalageLigne, décalageColonne}
            // 1. Horizontal (0, 1)
            // 2. Vertical (1, 0)
            // 3. Diagonale descendante (1, 1)
            // 4. Diagonale montante (1, -1)
            int[,] directions = new int[,] { { 0, 1 }, { 1, 0 }, { 1, 1 }, { 1, -1 } };

            // On teste chaque axe
            for (int i = 0; i < 4; i++)
            {
                int dLi = directions[i, 0];
                int dCo = directions[i, 1];

                // On compte déjà le pion qu'on vient de poser (donc on commence à 1)
                int compteur = 1;

                // On compte les pions de la même couleur dans un sens...
                compteur += CompterPionsDirection(derniereLigne, derniereColonne, dLi, dCo, idJoueur);
                // ... puis dans le sens diamétralement opposé
                compteur += CompterPionsDirection(derniereLigne, derniereColonne, -dLi, -dCo, idJoueur);

                // Si on a atteint ou dépassé le nombre requis, c'est gagné !
                if (compteur >= nbAAligner)
                {
                    return true;
                }
            }

            return false;
        }

        // Méthode utilitaire privée qui compte les pions consécutifs dans une direction précise
        private int CompterPionsDirection(int ligneInitiale, int colonneInitiale, int dLi, int dCo, int idJoueur)
        {
            int compteur = 0;
            int l = ligneInitiale + dLi;
            int c = colonneInitiale + dCo;

            // Tant qu'on reste dans les limites de la grille
            while (l >= 0 && l < NbLignes && c >= 0 && c < NbColonnes)
            {
                // Si on tombe sur un pion du même joueur, on incrémente et on continue d'avancer
                if (_plateau[l, c] != null && _plateau[l, c].JoueurId == idJoueur)
                {
                    compteur++;
                    l += dLi;
                    c += dCo;
                }
                else
                {
                    // Dès qu'on tombe sur une case vide ou un pion adverse, on s'arrête
                    break;
                }
            }
            return compteur;
        }
    } // <-- Fermeture de la classe Grille
} // <-- Fermeture du namespace Puissance4_Systeme