using System.Windows;

namespace SAE_IHM
{
    public partial class FinPartie : Window
    {
        // Permet de savoir quel bouton a été cliqué ("Relancer", "Menu", ou "Quitter")
        public string ActionChoisie { get; private set; } = "Menu";

        // Le constructeur prend les textes à afficher en paramètres
        public FinPartie(string messageFin, string messageScore)
        {
            InitializeComponent();

            ConfigurationGlobale.AppliquerTheme(this);

            LblTitreFin.Text = messageFin;
            LblScore.Text = messageScore;
        }

        private void BtnRelancer_Click(object sender, RoutedEventArgs e)
        {
            ActionChoisie = "Relancer";
            this.Close();
        }

        private void BtnMenu_Click(object sender, RoutedEventArgs e)
        {
            ActionChoisie = "Menu";
            this.Close();
        }

        private void BtnQuitter_Click(object sender, RoutedEventArgs e)
        {
            // Le fameux pop-up de confirmation de la maquette
            MessageBoxResult resultat = MessageBox.Show(
                "Êtes vous sûr de vouloir quitter le jeu ?",
                "Quitter",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultat == MessageBoxResult.Yes)
            {
                ActionChoisie = "Quitter";
                this.Close();
            }
            // Si "Non", on ne fait rien, la pop-up se ferme et on reste sur la fenêtre de fin
        }
    }
}