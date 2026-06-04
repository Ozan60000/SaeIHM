using System.Windows;

namespace SAE_IHM
{
    public partial class Parametres : Window
    {
        public Parametres()
        {
            InitializeComponent();
        }

        private void BtnValider_Click(object sender, RoutedEventArgs e)
        {
            // La propriété DialogResult à 'true' indique à la fenêtre principale 
            // que l'utilisateur a bien validé ses choix avant la fermeture
            this.DialogResult = true;
        }

        private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            // On ferme simplement la fenêtre sans valider
            this.DialogResult = false;
        }
    }
}