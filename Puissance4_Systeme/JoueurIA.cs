using System;
using System.Collections.Generic;

namespace Puissance4_Systeme
{
    public class JoueurIA : Joueur
    {
        // profondeur de recherche
        public int Profondeur { get; set; }

        // score max pour la victoire
        private const int SCORE_VICTOIRE = 1000000;

        public JoueurIA(int id, string nom, int profondeur = 5) : base(id, nom)
        {
            Profondeur = profondeur;
        }

        // methode principale appelee par le jeu
        public override int ChoisirCoup(Grille grille)
        {
            int centre = grille.NbColonnes / 2;

            // on force le premier coup au centre
            if (CompterPions(grille) < 2 && !grille.EstColonnePleine(centre))
                return centre;

            // check s'il y a un coup pour gagner ou bloquer direct
            int coupGagnant = TrouverCoupGagnantImmediat(grille, this.Id);
            if (coupGagnant != -1) return coupGagnant;

            int adversaireId = (this.Id == 1) ? 2 : 1;
            int coupBlocage = TrouverCoupGagnantImmediat(grille, adversaireId);
            if (coupBlocage != -1) return coupBlocage;

            // creation de l'arbre (livrable 1)
            Arbre arbre = new Arbre();
            int meilleurCoup = centre;
            int meilleurScore = int.MinValue;

            // bornes alpha beta
            int alpha = int.MinValue;
            int beta = int.MaxValue;

            // on recupere les colonnes triees (du centre vers les bords)
            List<int> colonnesOrdonnees = GenererColonnesOrdonnees(grille.NbColonnes);

            foreach (int c in colonnesOrdonnees)
            {
                if (!grille.EstColonnePleine(c))
                {
                    // on teste la colonne
                    grille.PlacerPion(c, new Pion(this.Id));
                    Noeud enfant = new Noeud(c);
                    arbre.Racine.Enfants.Add(enfant);

                    // appel recursif
                    int score = Minimax(grille, enfant, Profondeur - 1, alpha, beta, false);

                    // on retire le pion pour tester la suite
                    grille.RetirerPion(c);

                    // maj du meilleur score
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

        // algo minimax
        private int Minimax(Grille grille, Noeud noeudCourant, int profondeur, int alpha, int beta, bool estMax)
        {
            int adversaireId = (this.Id == 1) ? 2 : 1;

            // check victoire/defaite
            if (VerifierVictoireBrute(grille, this.Id)) return SCORE_VICTOIRE + profondeur;
            if (VerifierVictoireBrute(grille, adversaireId)) return -SCORE_VICTOIRE - profondeur;

            // limite de profondeur atteinte on evalue
            if (profondeur == 0)
            {
                int eval = EvaluerGrille(grille, this.Id, adversaireId);
                noeudCourant.Score = eval;
                return eval;
            }

            List<int> colonnesOrdonnees = GenererColonnesOrdonnees(grille.NbColonnes);

            // tour de l'IA (noeud MAX)
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

                        // coupure alpha beta
                        if (beta <= alpha) break;
                    }
                }
                noeudCourant.Score = maxEval;
                return maxEval;
            }
            // tour de l'adversaire (noeud MIN)
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

                        // coupure alpha beta
                        if (beta <= alpha) break;
                    }
                }
                noeudCourant.Score = minEval;
                return minEval;
            }
        }

        // heuristique
        private int EvaluerGrille(Grille grille, int iaId, int advId)
        {
            int score = 0;

            // on check les fenetres de 4 cases dans toutes les directions
            // horizontales
            for (int l = 0; l < grille.NbLignes; l++)
                for (int c = 0; c < grille.NbColonnes - 3; c++)
                    score += EvaluerFenetre(grille, l, c, 0, 1, iaId, advId);

            // verticales
            for (int l = 0; l < grille.NbLignes - 3; l++)
                for (int c = 0; c < grille.NbColonnes; c++)
                    score += EvaluerFenetre(grille, l, c, 1, 0, iaId, advId);

            // diagonales 1
            for (int l = 0; l < grille.NbLignes - 3; l++)
                for (int c = 0; c < grille.NbColonnes - 3; c++)
                    score += EvaluerFenetre(grille, l, c, 1, 1, iaId, advId);

            // diagonales 2
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

            // on compte qui a des pions dans la fenetre
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

            // fenetre bloquee par les deux joueurs
            if (nbIA > 0 && nbAdv > 0) return 0;

            // scores d'attaque
            if (nbIA == 3 && nbVide == 1) return 80;
            if (nbIA == 2 && nbVide == 2) return 15;

            // scores de defense (valeurs plus fortes pour obliger a bloquer)
            if (nbAdv == 3 && nbVide == 1) return -120;
            if (nbAdv == 2 && nbVide == 2) return -20;

            return 0;
        }

        // methodes utiles

        // regarde si un coup permet de gagner a ce tour
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

        // verifie si la grille contient un gagnant
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

        // genere la liste des colonnes du milieu vers les cotes
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