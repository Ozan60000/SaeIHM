using System.Windows;
using System.Windows.Controls;

namespace SAE_IHM
{
    public partial class Historique : Window
    {
        public Historique()
        {
            InitializeComponent();

            ConfigurationGlobale.AppliquerTheme(this);

            // On remplit la liste avec quelques parties d'exemple
            LstHistorique.Items.Add("Partie 1 - 04/06/2026 - Victoire");
            LstHistorique.Items.Add("Partie 2 - 03/06/2026 - Défaite");
            LstHistorique.Items.Add("Partie 3 - 03/06/2026 - Victoire");
            LstHistorique.Items.Add("Partie 4 - 02/06/2026 - Égalité");
            LstHistorique.Items.Add("Partie 5 - 01/06/2026 - Défaite");
            LstHistorique.Items.Add("Partie 6 - 31/05/2026 - Victoire");
        }

        private void LstHistorique_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Si rien n'est sélectionné on ne fait rien
            if (LstHistorique.SelectedItem == null)
            {
                return;
            }

            // On affiche un message d'erreur car le replay n'est pas dispo
            MessageBox.Show("Le replay de cette partie n'est pas disponible pour le moment.",
                            "Information",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);

            // On désélectionne pour que l'utilisateur puisse cliquer à nouveau sur la même partie
            LstHistorique.SelectedItem = null;
        }

        private void BtnFermer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}