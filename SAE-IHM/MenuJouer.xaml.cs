using System.Windows;

namespace SAE_IHM
{
    public partial class MenuJouer : Window
    {
        public MenuJouer()
        {
            InitializeComponent();
        }

        private void BtnNouvellePartie_Click(object sender, RoutedEventArgs e)
        {
            // On ferme cette fenêtre en signalant que l'utilisateur veut une nouvelle partie
            // La MainWindow s'occupera de la suite (choix adversaire + lancement)
            this.DialogResult = true;
        }

        private void BtnReprendre_Click(object sender, RoutedEventArgs e)
        {
            // On ouvre la fenêtre de sélection des parties à reprendre
            SelectionPartie fenetreSelection = new SelectionPartie();
            fenetreSelection.ShowDialog();
        }

        private void BtnHistorique_Click(object sender, RoutedEventArgs e)
        {
            // On ouvre la fenêtre de l'historique
            Historique fenetreHistorique = new Historique();
            fenetreHistorique.ShowDialog();
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}