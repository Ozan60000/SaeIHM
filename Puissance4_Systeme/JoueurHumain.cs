namespace Puissance4_Systeme
{
    public class JoueurHumain : Joueur
    {
        public JoueurHumain(int id, string nom) : base(id, nom)
        {
        }

        public override int ChoisirCoup(Grille grille)
        {
            // Le joueur humain clique sur l'interface graphique, l'IA ne fait rien ici
            return -1;
        }
    }
}