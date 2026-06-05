using System.Windows;
using System.Windows.Controls;

namespace SAE_IHM
{
    public partial class ChoixCouleur : Window
    {
        // Couleurs sélectionnées par chaque joueur (par défaut)
        private string _couleurJ1 = "Rouge";
        private string _couleurJ2 = "Jaune";

        public ChoixCouleur()
        {
            InitializeComponent();
        }

        private void BtnCouleur_Click(object sender, RoutedEventArgs e)
        {
            // On récupère le nom de la couleur via le Tag du bouton
            Button btn = (Button)sender;
            string nomCouleur = btn.Tag.ToString();

            // On applique la couleur au joueur actuellement sélectionné
            if (RadJ1.IsChecked == true)
            {
                _couleurJ1 = nomCouleur;
            }
            else
            {
                _couleurJ2 = nomCouleur;
            }

            // On met à jour l'affichage en bas
            LblJ1.Text = "Joueur 1 : " + _couleurJ1;
            LblJ2.Text = "Joueur 2 : " + _couleurJ2;
        }

        private void BtnFermer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}