using System.Windows;

namespace SAE_IHM
{
    public partial class FinPartie : Window
    {
        public string ActionChoisie { get; private set; } = "Menu";

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
        }
    }
}