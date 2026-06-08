using System.Windows;

namespace SAE_IHM
{
    public partial class MenuJouer : Window
    {
        public MenuJouer()
        {
            InitializeComponent();
            ConfigurationGlobale.AppliquerTheme(this);
        }

        private void BtnNouvellePartie_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void BtnReprendre_Click(object sender, RoutedEventArgs e)
        {
            SelectionPartie fenetreSelection = new SelectionPartie();
            fenetreSelection.ShowDialog();
        }

        private void BtnHistorique_Click(object sender, RoutedEventArgs e)
        {
            Historique fenetreHistorique = new Historique();
            fenetreHistorique.ShowDialog();
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}