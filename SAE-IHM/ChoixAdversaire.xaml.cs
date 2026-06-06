using System.Windows;

namespace SAE_IHM
{
    public partial class ChoixAdversaire : Window
    {
        // Propriété lue par la MainWindow après fermeture pour savoir quoi faire ensuite
        // Valeurs possibles : "Virtuel", "Local", "Ligne"
        public string TypeAdversaire { get; private set; } = "Local";

        public ChoixAdversaire()
        {
            InitializeComponent();
        }

        private void BtnAdvVirtuel_Click(object sender, RoutedEventArgs e)
        {
            TypeAdversaire = "Virtuel";
            this.DialogResult = true;
        }

        private void BtnAmiLocal_Click(object sender, RoutedEventArgs e)
        {
            TypeAdversaire = "Local";
            this.DialogResult = true;
        }

        private void BtnAdvLigne_Click(object sender, RoutedEventArgs e)
        {
            // Pas demandé dans la SAE, on prévient juste l'utilisateur
            MessageBox.Show("Le mode en ligne n'est pas disponible pour le moment.",
                            "Information",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}