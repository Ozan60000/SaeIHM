using System.ComponentModel;
using System.Windows;

namespace SAE_IHM
{
    public partial class MainWindow : Window
    {
        // Mode contre l'IA (utilisé lors du lancement de la partie)
        private bool _modeContreIA = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        // --- ÉVÉNEMENTS DES BOUTONS ---

        private void BtnJouer_Click(object sender, RoutedEventArgs e)
        {
            // 1. Menu Jouer
            MenuJouer menuJouer = new MenuJouer();
            bool? veutNouvellePartie = menuJouer.ShowDialog();
            if (veutNouvellePartie != true)
            {
                return;
            }

            // 2. Choix de l'adversaire
            ChoixAdversaire fenetreChoix = new ChoixAdversaire();
            bool? aChoisi = fenetreChoix.ShowDialog();
            if (aChoisi != true)
            {
                return;
            }

            // 3. Règles de la partie (taille grille, alignement)
            Regles fenetreRegles = new Regles();
            bool? reglesOk = fenetreRegles.ShowDialog();
            if (reglesOk != true)
            {
                return;
            }

            // 4. Configuration du mode contre IA
            if (fenetreChoix.TypeAdversaire == "Virtuel")
            {
                _modeContreIA = true;
            }
            else
            {
                _modeContreIA = false;
            }

            // 5. Lancement de la partie avec les paramètres choisis
            FenetreJeu ecranJeu = new FenetreJeu();
            ecranJeu.TypeAdversaire = fenetreChoix.TypeAdversaire;
            ecranJeu.NbLignes = fenetreRegles.NbLignes;
            ecranJeu.NbColonnes = fenetreRegles.NbColonnes;
            ecranJeu.NbAAligner = fenetreRegles.NbAAligner;

            this.Hide();
            ecranJeu.ShowDialog();
            this.Show();
        }

        private void BtnParametres_Click(object sender, RoutedEventArgs e)
        {
            // L'écran Paramètres servira plus tard pour l'accessibilité et la personnalisation
            Parametres fenetreParam = new Parametres();
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