using System.ComponentModel;
using System.Windows;

namespace SAE_IHM
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // --- ÉVÉNEMENTS DES BOUTONS ---

        private void BtnJouer_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Lancement de la partie en cours de développement...", "Jouer", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // On ne garde que la BONNE version du bouton Paramètres !
        private void BtnParametres_Click(object sender, RoutedEventArgs e)
        {
            // On instancie la fenêtre
            Parametres fenetreParam = new Parametres();

            // On l'ouvre en mode "Modal" (bloquant)
            fenetreParam.ShowDialog();
        }

        private void BtnQuitter_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // --- GESTION DE LA FERMETURE ---

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            MessageBoxResult resultat = MessageBox.Show(
                "Êtes-vous sûr de vouloir quitter le Puissance 4 ?",
                "Confirmation de sortie",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultat == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}