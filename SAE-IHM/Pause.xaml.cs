using System.Windows;

namespace SAE_IHM
{
    public partial class Pause : Window
    {
        public string ActionChoisie { get; private set; } = "Reprendre";

        public Pause()
        {
            InitializeComponent();

            ConfigurationGlobale.AppliquerTheme(this);
        }

        private void BtnReprendre_Click(object sender, RoutedEventArgs e)
        {
            ActionChoisie = "Reprendre";
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
                "Êtes-vous sûr de vouloir quitter le jeu ?",
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