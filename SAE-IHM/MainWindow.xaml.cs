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
            // 1. Menu Jouer
            MenuJouer menuJouer = new MenuJouer();
            bool? veutNouvellePartie = menuJouer.ShowDialog();
            if (veutNouvellePartie != true) return;

            // 2. Choix de l'adversaire
            ChoixAdversaire fenetreChoix = new ChoixAdversaire();
            bool? aChoisi = fenetreChoix.ShowDialog();
            if (aChoisi != true) return;

            // 3. Règles de la partie (taille grille, alignement)
            Regles fenetreRegles = new Regles();
            bool? reglesOk = fenetreRegles.ShowDialog();
            if (reglesOk != true) return;

            // 4. Lancement de la partie avec les paramètres choisis
            FenetreJeu ecranJeu = new FenetreJeu();
            ecranJeu.TypeAdversaire = fenetreChoix.TypeAdversaire;
            ecranJeu.NbLignes = fenetreRegles.NbLignes;
            ecranJeu.NbColonnes = fenetreRegles.NbColonnes;
            ecranJeu.NbAAligner = fenetreRegles.NbAAligner;

            // --- NOUVEAU : Transmission des chronos ---
            ecranJeu.TempsChronoGlobale = fenetreRegles.TempsChronoGlobale;
            ecranJeu.TempsReflexion = fenetreRegles.TempsReflexion;

            this.Hide();
            ecranJeu.ShowDialog();

            // On essaie de réafficher le menu. 
            // Si le joueur a cliqué sur "Quitter", la fenêtre est détruite et ça va échouer.
            // On attrape l'erreur en silence pour une fermeture propre !
            try
            {
                this.Show();
            }
            catch
            {
                // L'application est en train de se fermer, on ne fait rien.
            }
        }

        private void BtnParametres_Click(object sender, RoutedEventArgs e)
        {
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