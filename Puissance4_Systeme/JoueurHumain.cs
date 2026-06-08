namespace Puissance4_Systeme
{
    public class JoueurHumain : Joueur
    {
        public JoueurHumain(int id, string nom) : base(id, nom)
        {
        }

        public override int ChoisirCoup(Grille grille)
        {
            return -1;
        }
    }
}