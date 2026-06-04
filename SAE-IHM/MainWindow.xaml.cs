using System.ComponentModel;
using System.Windows;

namespace SAE_IHM
{
    public partial class MainWindow : Window
    {
        // Variables de configuration globales (par défaut : grille classique)
        private int _nbLignes = 6;
        private int _nbColonnes = 7;
        private int _nbAAligner = 4;
        private bool _modeContreIA = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        // --- ÉVÉNEMENTS DES BOUTONS ---

        private void BtnJouer_Click(object sender, RoutedEventArgs e)
        {
            // 1. On instancie la fenêtre de jeu
            FenetreJeu ecranJeu = new FenetreJeu();

            // 2. On masque le menu principal
            this.Hide();

            // 3. On ouvre l'écran de jeu en mode bloquant
            ecranJeu.ShowDialog();

            // 4. Une fois la partie terminée ou la fenêtre fermée, on réaffiche le menu
            this.Show();
        }

        private void BtnParametres_Click(object sender, RoutedEventArgs e)
        {
            // On instancie la fenêtre
            Parametres fenetreParam = new Parametres();

            // On l'ouvre en mode "Modal" (bloquant) et on regarde s'il a cliqué sur Valider
            bool? aValide = fenetreParam.ShowDialog();

            // Si l'utilisateur a validé, on met à jour nos variables internes
            if (aValide == true)
            {
                _nbLignes = fenetreParam.NbLignes;
                _nbColonnes = fenetreParam.NbColonnes;
                _nbAAligner = fenetreParam.NbAAligner;
                _modeContreIA = fenetreParam.ModeContreIA;

                MessageBox.Show($"Paramètres enregistrés !\nGrille : {_nbLignes}x{_nbColonnes} | Aligner : {_nbAAligner}",
                                "Configuration",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
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