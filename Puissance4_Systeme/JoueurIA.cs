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

        public override int ChoisirCoup(Grille grille)
        {
            int centre = grille.NbColonnes / 2;
            
            // 1. OUVERTURE : Jouer au centre
            if (CompterPions(grille) < 2 && !grille.EstColonnePleine(centre))
                return centre;

            // 2. INSTINCT DE SURVIE : Gagner ou bloquer immédiatement (Comme en Python)
            int coupGagnant = TrouverCoupGagnantImmediat(grille, this.Id);
            if (coupGagnant != -1) return coupGagnant;

            int adversaireId = (this.Id == 1) ? 2 : 1;
            int coupBlocage = TrouverCoupGagnantImmediat(grille, adversaireId);
            if (coupBlocage != -1) return coupBlocage;

            // 3. MINIMAX AVEC ARBRE (Pour respecter le Livrable 1)
            Arbre arbre = new Arbre();
            int meilleurCoup = centre;
            int meilleurScore = int.MinValue;
            int alpha = int.MinValue;
            int beta = int.MaxValue;

            List<int> colonnesOrdonnees = GenererColonnesOrdonnees(grille.NbColonnes);

            foreach (int c in colonnesOrdonnees)
            {
                if (!grille.EstColonnePleine(c))
                {
                    grille.PlacerPion(c, new Pion(this.Id));
                    Noeud enfant = new Noeud(c);
                    arbre.Racine.Enfants.Add(enfant);
                    
                    int score = Minimax(grille, enfant, Profondeur - 1, alpha, beta, false);
                    grille.RetirerPion(c);

                    if (score > meilleurScore)
                    {
                        meilleurScore = score;
                        meilleurCoup = c;
                    }
                    alpha = Math.Max(alpha, meilleurScore);
                }
            }
            return meilleurCoup;
        }
        
        private int Minimax(Grille grille, Noeud noeudCourant, int profondeur, int alpha, int beta, bool estMax)
        {
<<<<<<< HEAD
=======
            if (grille.VerifierVictoire(derniereLigne, derniereCol, 4))
                return maximisant ? int.MinValue + 100 : int.MaxValue - 100;

            if (profondeur == 0) return EvaluerGrille(grille);

>>>>>>> 04948de57b67b3ec1f1c96ec1c0bb56b192f3354
            int adversaireId = (this.Id == 1) ? 2 : 1;
            
            // Vérification de victoire dans l'anticipation
            if (VerifierVictoireBrute(grille, this.Id)) return SCORE_VICTOIRE + profondeur;
            if (VerifierVictoireBrute(grille, adversaireId)) return -SCORE_VICTOIRE - profondeur;
            
            if (profondeur == 0)
            {
                int eval = EvaluerGrille(grille, this.Id, adversaireId);
                noeudCourant.Score = eval;
                return eval;
            }

            List<int> colonnesOrdonnees = GenererColonnesOrdonnees(grille.NbColonnes);

            if (estMax)
            {
                int maxEval = int.MinValue;
                foreach (int c in colonnesOrdonnees)
                {
                    if (!grille.EstColonnePleine(c))
                    {
                        grille.PlacerPion(c, new Pion(this.Id));
                        Noeud enfant = new Noeud(c);
                        noeudCourant.Enfants.Add(enfant);
                        
                        int eval = Minimax(grille, enfant, profondeur - 1, alpha, beta, false);
                        grille.RetirerPion(c);
                        
                        maxEval = Math.Max(maxEval, eval);
                        alpha = Math.Max(alpha, eval);
                        if (beta <= alpha) break;
                    }
                }
                noeudCourant.Score = maxEval;
                return maxEval;
            }
            else
            {
                int minEval = int.MaxValue;
                foreach (int c in colonnesOrdonnees)
                {
                    if (!grille.EstColonnePleine(c))
                    {
                        grille.PlacerPion(c, new Pion(adversaireId));
                        Noeud enfant = new Noeud(c);
                        noeudCourant.Enfants.Add(enfant);
                        
                        int eval = Minimax(grille, enfant, profondeur - 1, alpha, beta, true);
                        grille.RetirerPion(c);
                        
                        minEval = Math.Min(minEval, eval);
                        beta = Math.Min(beta, eval);
                        if (beta <= alpha) break;
                    }
                }
                noeudCourant.Score = minEval;
                return minEval;
            }
        }

        // --- HEURISTIQUE ASYMÉTRIQUE (Traduite du Python) ---

        private int EvaluerGrille(Grille grille, int iaId, int advId)
        {
            int score = 0;
            for (int l = 0; l < grille.NbLignes; l++)
                for (int c = 0; c < grille.NbColonnes - 3; c++)
                    score += EvaluerFenetre(grille, l, c, 0, 1, iaId, advId);
            
            for (int l = 0; l < grille.NbLignes - 3; l++)
                for (int c = 0; c < grille.NbColonnes; c++)
                    score += EvaluerFenetre(grille, l, c, 1, 0, iaId, advId);
            
            for (int l = 0; l < grille.NbLignes - 3; l++)
                for (int c = 0; c < grille.NbColonnes - 3; c++)
                    score += EvaluerFenetre(grille, l, c, 1, 1, iaId, advId);
            
            for (int l = 3; l < grille.NbLignes; l++)
                for (int c = 0; c < grille.NbColonnes - 3; c++)
                    score += EvaluerFenetre(grille, l, c, -1, 1, iaId, advId);
            
            return score;
        }

        private int EvaluerFenetre(Grille grille, int l, int c, int dl, int dc, int iaId, int advId)
        {
            int nbIA = 0;
            int nbAdv = 0;
            int nbVide = 0;

            for (int i = 0; i < 4; i++)
            {
                Pion p = grille.GetPion(l + i * dl, c + i * dc);
                if (p != null)
                {
                    if (p.JoueurId == iaId) nbIA++;
                    else if (p.JoueurId == advId) nbAdv++;
                }
                else nbVide++;
            }

            // Si la fenêtre contient des pions des deux couleurs, personne ne peut y faire Puissance 4
            if (nbIA > 0 && nbAdv > 0) return 0; 

            // Valeurs d'attaque (Python)
            if (nbIA == 3 && nbVide == 1) return 80;
            if (nbIA == 2 && nbVide == 2) return 15;
            
            // Valeurs de défense : sanction plus forte pour forcer l'IA à bloquer ! (Python)
            if (nbAdv == 3 && nbVide == 1) return -120;
            if (nbAdv == 2 && nbVide == 2) return -20;
            
            return 0;
        }

        // --- OUTILS ET DÉTECTION ---

        private int TrouverCoupGagnantImmediat(Grille grille, int joueurId)
        {
            for (int c = 0; c < grille.NbColonnes; c++)
            {
                if (!grille.EstColonnePleine(c))
                {
                    grille.PlacerPion(c, new Pion(joueurId));
                    bool gagne = VerifierVictoireBrute(grille, joueurId);
                    grille.RetirerPion(c);
                    if (gagne) return c;
                }
            }
            return -1;
        }

        private bool VerifierVictoireBrute(Grille grille, int joueurId)
        {
            for (int l = 0; l < grille.NbLignes; l++)
                for (int c = 0; c < grille.NbColonnes - 3; c++)
                    if (EstAligne(grille, l, c, 0, 1, joueurId)) return true;
                    
            for (int l = 0; l < grille.NbLignes - 3; l++)
                for (int c = 0; c < grille.NbColonnes; c++)
                    if (EstAligne(grille, l, c, 1, 0, joueurId)) return true;
                    
            for (int l = 0; l < grille.NbLignes - 3; l++)
                for (int c = 0; c < grille.NbColonnes - 3; c++)
                    if (EstAligne(grille, l, c, 1, 1, joueurId)) return true;
                    
            for (int l = 3; l < grille.NbLignes; l++)
                for (int c = 0; c < grille.NbColonnes - 3; c++)
                    if (EstAligne(grille, l, c, -1, 1, joueurId)) return true;
                    
            return false;
        }

        private bool EstAligne(Grille grille, int l, int c, int dl, int dc, int joueurId)
        {
            for (int i = 0; i < 4; i++)
            {
                Pion p = grille.GetPion(l + i * dl, c + i * dc);
                if (p == null || p.JoueurId != joueurId) return false;
            }
            return true;
        }

        private int CompterPions(Grille grille)
        {
            int count = 0;
            for (int l = 0; l < grille.NbLignes; l++)
                for (int c = 0; c < grille.NbColonnes; c++)
                    if (grille.GetPion(l, c) != null) count++;
            return count;
        }

        private List<int> GenererColonnesOrdonnees(int nbColonnes)
        {
            List<int> ordre = new List<int>();
            int centre = nbColonnes / 2;
            ordre.Add(centre);
            for (int i = 1; i <= centre; i++)
            {
                if (centre - i >= 0) ordre.Add(centre - i);
                if (centre + i < nbColonnes) ordre.Add(centre + i);
            }
            return ordre;
        }
    }
}