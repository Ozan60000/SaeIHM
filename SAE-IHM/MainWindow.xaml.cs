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
    // 1. On ouvre d'abord le menu Jouer
    MenuJouer menuJouer = new MenuJouer();
    bool? veutNouvellePartie = menuJouer.ShowDialog();

    // Si l'utilisateur n'a pas choisi de faire une nouvelle partie (clic Retour
    // ou fenêtre fermée), on revient au menu principal
    if (veutNouvellePartie != true)
    {
        return;
    }

    // 2. On enchaîne sur le choix d'adversaire
    ChoixAdversaire fenetreChoix = new ChoixAdversaire();
    bool? aChoisi = fenetreChoix.ShowDialog();

    if (aChoisi != true)
    {
        return;
    }

    // 3. Configuration du mode contre IA
    if (fenetreChoix.TypeAdversaire == "Virtuel")
    {
        _modeContreIA = true;
    }
    else
    {
        _modeContreIA = false;
    }

            // On instancie la fenêtre de jeu et on lui transmet les paramètres
            FenetreJeu ecranJeu = new FenetreJeu();
            ecranJeu.TypeAdversaire = fenetreChoix.TypeAdversaire;
            ecranJeu.NbLignes = _nbLignes;
            ecranJeu.NbColonnes = _nbColonnes;
            ecranJeu.NbAAligner = _nbAAligner;

            this.Hide();
            ecranJeu.ShowDialog();
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