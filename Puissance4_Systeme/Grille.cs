using System;
using System.Collections.Generic;
using System.Text;

namespace Puissance4_Systeme
{
    public class Grille
    {
        private Pion?[,] _plateau;

        public int NbLignes { get; private set; }
        public int NbColonnes { get; private set; }

        public Grille(int lignes, int colonnes)
        {
            NbLignes = lignes;
            NbColonnes = colonnes;
            _plateau = new Pion[lignes, colonnes];
        }

        public bool EstColonnePleine(int colonne)
        {
            return _plateau[0, colonne] != null;
        }

        public int PlacerPion(int colonne, Pion pion)
        {
            if (colonne < 0 || colonne >= NbColonnes || EstColonnePleine(colonne))
            {
                return -1;
            }

            for (int l = NbLignes - 1; l >= 0; l--)
            {
                if (_plateau[l, colonne] == null)
                {
                    _plateau[l, colonne] = pion;
                    return l;
                }
            }
            return -1;
        }

        public Pion? GetPion(int ligne, int colonne)
        {
            if (ligne < 0 || ligne >= NbLignes || colonne < 0 || colonne >= NbColonnes)
                return null;

            return _plateau[ligne, colonne];
        }

        public void RetirerPion(int colonne)
        {
            if (colonne < 0 || colonne >= NbColonnes) return;

            for (int l = 0; l < NbLignes; l++)
            {
                if (_plateau[l, colonne] != null)
                {
                    _plateau[l, colonne] = null;
                    break;
                }
            }
        }

        public bool VerifierVictoire(int derniereLigne, int derniereColonne, int nbAAligner)
        {
            Pion? pionJoue = _plateau[derniereLigne, derniereColonne];
            if (pionJoue == null) return false;

            int idJoueur = pionJoue.JoueurId;

            int[,] directions = new int[,] { { 0, 1 }, { 1, 0 }, { 1, 1 }, { 1, -1 } };

            for (int i = 0; i < 4; i++)
            {
                int dLi = directions[i, 0];
                int dCo = directions[i, 1];

                int compteur = 1;

                compteur += CompterPionsDirection(derniereLigne, derniereColonne, dLi, dCo, idJoueur);
                compteur += CompterPionsDirection(derniereLigne, derniereColonne, -dLi, -dCo, idJoueur);

                if (compteur >= nbAAligner)
                {
                    return true;
                }
            }

            return false;
        }

        private int CompterPionsDirection(int ligneInitiale, int colonneInitiale, int dLi, int dCo, int idJoueur)
        {
            int compteur = 0;
            int l = ligneInitiale + dLi;
            int c = colonneInitiale + dCo;

            while (l >= 0 && l < NbLignes && c >= 0 && c < NbColonnes)
            {
                if (_plateau[l, c] != null && _plateau[l, c]!.JoueurId == idJoueur)
                {
                    compteur++;
                    l += dLi;
                    c += dCo;
                }
                else
                {
                    break;
                }
            }
            return compteur;
        }
    }
}