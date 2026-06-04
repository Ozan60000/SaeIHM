using System;
using System.Collections.Generic;

namespace Puissance4_Systeme
{
    public class JoueurIA : Joueur
    {
        public int Profondeur { get; set; }
        private const int SCORE_VICTOIRE = 1000000;

        public JoueurIA(int id, string nom, int profondeur = 5) : base(id, nom)
        {
            Profondeur = profondeur;
        }

        // Point d'entrée obligatoire (hérité de Joueur)
        public override int ChoisirCoup(Grille grille)
        {
            int centre = grille.NbColonnes / 2;

            // 1. Stratégie d'ouverture : jouer au centre au tout début
            if (CompterPionsGrille(grille) < 2 && !grille.EstColonnePleine(centre))
            {
                return centre;
            }

            // 2. Vérifier si l'IA peut gagner immédiatement en 1 coup
            int coupGagnant = TrouverCoupGagnantImmediat(grille, this.Id, grille.NbColonnes);
            if (coupGagnant != -1) return coupGagnant;

            // 3. Vérifier si l'adversaire peut gagner au prochain coup et le bloquer
            int adversaireId = (this.Id == 1) ? 2 : 1;
            int coupA_Bloquer = TrouverCoupGagnantImmediat(grille, adversaireId, grille.NbColonnes);
            if (coupA_Bloquer != -1) return coupA_Bloquer;

            // 4. Sinon, lancer l'exploration Minimax Alpha-Bêta
            int meilleurCoup = -1;
            int meilleurScore = int.MinValue;

            for (int c = 0; c < grille.NbColonnes; c++)
            {
                if (!grille.EstColonnePleine(c))
                {
                    Pion simulationPion = new Pion(this.Id);
                    grille.PlacerPion(c, simulationPion);

                    // Appel du nœud MIN (le tour de l'adversaire)
                    int scoreCoup = Minimax(grille, Profondeur - 1, int.MinValue, int.MaxValue, false);

                    grille.RetirerPion(c); // Annulation

                    if (scoreCoup > meilleurScore)
                    {
                        meilleurScore = scoreCoup;
                        meilleurCoup = c;
                    }
                }
            }

            return (meilleurCoup != -1) ? meilleurCoup : centre;
        }

        // L'algorithme Minimax avec élagage Alpha-Bêta (traduit de votre livrable 3)
        private int Minimax(Grille grille, int profondeur, int alpha, int beta, bool maximisant)
        {
            int adversaireId = (this.Id == 1) ? 2 : 1;

            // TODO pour Thomas : implémenter la condition d'arrêt et la récursion
            // Si profondeur == 0 ou fin de partie -> retourner EvaluerGrille(grille)

            if (maximisant)
            {
                int maxEval = int.MinValue;
                for (int c = 0; c < grille.NbColonnes; c++)
                {
                    if (!grille.EstColonnePleine(c))
                    {
                        grille.PlacerPion(c, new Pion(this.Id));
                        int eval = Minimax(grille, profondeur - 1, alpha, beta, false);
                        grille.RetirerPion(c);
                        maxEval = Math.Max(maxEval, eval);
                        alpha = Math.Max(alpha, eval);
                        if (beta <= alpha) break; // Élagage Beta
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
                        grille.PlacerPion(c, new Pion(adversaireId));
                        int eval = Minimax(grille, profondeur - 1, alpha, beta, true);
                        grille.RetirerPion(c);
                        minEval = Math.Min(minEval, eval);
                        beta = Math.Min(beta, eval);
                        if (beta <= alpha) break; // Élagage Alpha
                    }
                }
                return minEval;
            }
        }

        // Méthode d'évaluation heuristique des fenêtres de la grille
        private int EvaluerGrille(Grille grille)
        {
            int scoreTotal = 0;
            // TODO pour Thomas : Traduire vos boucles de détection des fenêtres de 4 cases 
            // (Horizontales, Verticales, Diagonales) pour attribuer les points (+100, +10, etc.)
            return scoreTotal;
        }

        // Fonctions utilitaires annexes
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
            // Code de simulation rapide pour détecter une victoire en 1 coup (Livrable 3 Python)
            for (int c = 0; c < nbColonnes; c++)
            {
                if (!grille.EstColonnePleine(c))
                {
                    Pion p = new Pion(joueurId);
                    int l = grille.PlacerPion(c, p);
                    bool gagne = grille.VerifierVictoire(l, c, 4); // Remplacer 4 par une variable globale si besoin
                    grille.RetirerPion(c);
                    if (gagne) return c;
                }
            }
            return -1;
        }
    }
}