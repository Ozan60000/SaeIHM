using System.Windows;
using System.Windows.Controls;

namespace SAE_IHM
{
    public partial class SelectionPartie : Window
    {
        public SelectionPartie()
        {
            InitializeComponent();

            ConfigurationGlobale.AppliquerTheme(this);

            // On remplit la liste avec quelques parties d'exemple
            LstParties.Items.Add("Partie 1 - 04/06/2026 - 14h32");
            LstParties.Items.Add("Partie 2 - 03/06/2026 - 19h05");
            LstParties.Items.Add("Partie 3 - 02/06/2026 - 09h47");
            LstParties.Items.Add("Partie 4 - 01/06/2026 - 21h14");
        }

        private void LstParties_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Si rien n'est sélectionné on ne fait rien
            if (LstParties.SelectedItem == null)
            {
                return;
            }

            // On affiche un message d'erreur car la reprise n'est pas dispo
            MessageBox.Show("La reprise de cette partie n'est pas disponible pour le moment.",
                            "Information",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);

            // On désélectionne pour que l'utilisateur puisse cliquer à nouveau sur la même partie
            LstParties.SelectedItem = null;
        }

        private void BtnFermer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}