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

            LstParties.Items.Add("Partie 1 - 04/06/2026 - 14h32");
            LstParties.Items.Add("Partie 2 - 03/06/2026 - 19h05");
            LstParties.Items.Add("Partie 3 - 02/06/2026 - 09h47");
            LstParties.Items.Add("Partie 4 - 01/06/2026 - 21h14");
        }

        private void LstParties_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LstParties.SelectedItem == null)
            {
                return;
            }

            MessageBox.Show("La reprise de cette partie n'est pas disponible pour le moment.",
                            "Information",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);

            LstParties.SelectedItem = null;
        }

        private void BtnFermer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}