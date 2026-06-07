using System.Windows;
using System.Windows.Controls;

namespace SAE_IHM
{
    public partial class ChoixCouleur : Window
    {
        public ChoixCouleur()
        {
            InitializeComponent();
            // On charge les couleurs actuelles à l'ouverture
            LblJ1.Text = "Joueur 1 : " + ConfigurationGlobale.CouleurJ1;
            LblJ2.Text = "Joueur 2 : " + ConfigurationGlobale.CouleurJ2;
        }

        private void BtnCouleur_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string nomCouleur = btn.Tag?.ToString() ?? "Rouge";

            // Si c'est aléatoire, on triche un peu en mettant une couleur fixe pour l'exemple
            if (nomCouleur == "Aléatoire") nomCouleur = "Bleu";

            if (RadJ1.IsChecked == true)
                ConfigurationGlobale.CouleurJ1 = nomCouleur;
            else
                ConfigurationGlobale.CouleurJ2 = nomCouleur;

            LblJ1.Text = "Joueur 1 : " + ConfigurationGlobale.CouleurJ1;
            LblJ2.Text = "Joueur 2 : " + ConfigurationGlobale.CouleurJ2;
        }

        private void BtnFermer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}