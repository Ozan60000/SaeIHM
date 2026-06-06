using System;
using System.Collections.Generic;

namespace Puissance4_Systeme
{
    public class JoueurIA : Joueur
    {
        public int Profondeur { get; set; }
        private const int SCORE_VICTOIRE = 1000000;

        public JoueurIA(int id, string nom, int profondeur = 4) : base(id, nom)
        {
            Profondeur = profondeur;
        }

        public override int ChoisirCoup(Grille grille)
        {
            int centre = grille.NbColonnes / 2;
            if (CompterPionsGrille(grille) < 2 && !grille.EstColonnePleine(centre)) return centre;

            int coupGagnant = TrouverCoupGagnantImmediat(grille, this.Id, grille.NbColonnes);
            if (coupGagnant != -1) return coupGagnant;

            int adversaireId = (this.Id == 1) ? 2 : 1;
            int coupA_Bloquer = TrouverCoupGagnantImmediat(grille, adversaireId, grille.NbColonnes);
            if (coupA_Bloquer != -1) return coupA_Bloquer;

            int meilleurCoup = -1;
            int meilleurScore = int.MinValue;

            for (int c = 0; c < grille.NbColonnes; c++)
            {
                if (!grille.EstColonnePleine(c))
                {
                    int ligne = grille.PlacerPion(c, new Pion(this.Id));
                    int scoreCoup = Minimax(grille, Profondeur - 1, int.MinValue, int.MaxValue, false, ligne, c);
                    grille.RetirerPion(c);

                    if (scoreCoup > meilleurScore)
                    {
                        meilleurScore = scoreCoup;
                        meilleurCoup = c;
                    }
                }
            }
            return (meilleurCoup != -1) ? meilleurCoup : centre;
        }

        private int Minimax(Grille grille, int profondeur, int alpha, int beta, bool maximisant, int derniereLigne, int derniereCol)
        {
            // Condition d'arrêt : Profondeur atteinte ou victoire
            if (grille.VerifierVictoire(derniereLigne, derniereCol, 4))
                return maximisant ? int.MinValue + 100 : int.MaxValue - 100;

            if (profondeur == 0) return EvaluerGrille(grille);

            int adversaireId = (this.Id == 1) ? 2 : 1;

            if (maximisant)
            {
                int maxEval = int.MinValue;
                for (int c = 0; c < grille.NbColonnes; c++)
                {
                    if (!grille.EstColonnePleine(c))
                    {
                        int l = grille.PlacerPion(c, new Pion(this.Id));
                        int eval = Minimax(grille, profondeur - 1, alpha, beta, false, l, c);
                        grille.RetirerPion(c);
                        maxEval = Math.Max(maxEval, eval);
                        alpha = Math.Max(alpha, eval);
                        if (beta <= alpha) break;
                    }
                }
                return maxEval;
            }
            else
            {
                int minEval = int.MaxValue;
                for (int c = 0; c < grille.NbColonnes; c++)
                {
                    if (!grille.EstColonnePleine(c))
                    {
                        int l = grille.PlacerPion(c, new Pion(adversaireId));
                        int eval = Minimax(grille, profondeur - 1, alpha, beta, true, l, c);
                        grille.RetirerPion(c);
                        minEval = Math.Min(minEval, eval);
                        beta = Math.Min(beta, eval);
                        if (beta <= alpha) break;
                    }
                }
                return minEval;
            }
        }

        private int EvaluerGrille(Grille grille)
        {
            // Heuristique simple : favorise le centre
            int score = 0;
            for (int l = 0; l < grille.NbLignes; l++)
            {
                for (int c = 0; c < grille.NbColonnes; c++)
                {
                    Pion? p = grille.GetPion(l, c);
                    if (p != null)
                    {
                        int val = (p.JoueurId == this.Id) ? 1 : -1;
                        score += val * (grille.NbColonnes - Math.Abs(c - grille.NbColonnes / 2));
                    }
                }
            }
            return score;
        }

        private int CompterPionsGrille(Grille grille)
        {
            int total = 0;
            for (int l = 0; l < grille.NbLignes; l++)
                for (int c = 0; c < grille.NbColonnes; c++)
                    if (grille.GetPion(l, c) != null) total++;
            return total;
        }

        private int TrouverCoupGagnantImmediat(Grille grille, int joueurId, int nbColonnes)
        {
            for (int c = 0; c < nbColonnes; c++)
            {
                if (!grille.EstColonnePleine(c))
                {
                    int l = grille.PlacerPion(c, new Pion(joueurId));
                    bool gagne = grille.VerifierVictoire(l, c, 4);
                    grille.RetirerPion(c);
                    if (gagne) return c;
                }
            }
            return -1;
        }
    }
}