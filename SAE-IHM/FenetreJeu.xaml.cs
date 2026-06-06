using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Puissance4_Systeme;

namespace SAE_IHM
{
    public partial class FenetreJeu : Window
    {
        // Paramètres reçus depuis la MainWindow
        public string TypeAdversaire { get; set; } = "Local";
        public int NbLignes { get; set; } = 6;
        public int NbColonnes { get; set; } = 7;
        public int NbAAligner { get; set; } = 4;

        public int TempsChronoGlobale { get; set; } = 0;
        public int TempsReflexion { get; set; } = 0;

        // --- LE VRAI MOTEUR DE JEU ---
        private Partie _maPartie = null!;
        private Button[,] _grilleBoutons = null!;
        private int _scoreJ1 = 0;
        private int _scoreJ2 = 0;

        // --- GESTION DU TEMPS ---
        private DispatcherTimer _timer = null!;
        private int _tempsRestantGlobalJ1;
        private int _tempsRestantGlobalJ2;
        private int _tempsRestantReflexion;

        // --- LE FAMEUX VERROU ---
        // Empêche le chronomètre de tourner en boucle pendant que les pop-ups s'affichent
        private bool _verrouPopup = false;

        public FenetreJeu()
        {
            InitializeComponent();
            // Sécurité : on tue l'horloge si l'utilisateur quitte violemment avec la croix rouge
            this.Closed += FenetreJeu_Closed;
        }

        private void FenetreJeu_Closed(object? sender, EventArgs e)
        {
            _timer?.Stop();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (TypeAdversaire == "Virtuel") LblJoueur2.Text = "Ordinateur";
            else LblJoueur2.Text = "Joueur 2";

            LblTourJoueur.Text = "C'est au tour de " + LblJoueur1.Text;

            // 1. On crée les joueurs
            Joueur j1 = new JoueurHumain(1, LblJoueur1.Text);
            Joueur j2;
            if (TypeAdversaire == "Virtuel")
                j2 = new JoueurIA(2, LblJoueur2.Text);
            else
                j2 = new JoueurHumain(2, LblJoueur2.Text);

            // 2. On démarre la VRAIE partie
            _maPartie = new Partie(NbLignes, NbColonnes, NbAAligner, j1, j2);
            _grilleBoutons = new Button[NbLignes, NbColonnes];

            // 3. Construction visuelle de la grille
            GrillePlateau.Rows = NbLignes;
            GrillePlateau.Columns = NbColonnes;

            for (int ligne = 0; ligne < NbLignes; ligne++)
            {
                for (int colonne = 0; colonne < NbColonnes; colonne++)
                {
                    Button btn = new Button();
                    btn.Background = Brushes.Transparent;
                    btn.BorderThickness = new Thickness(0);
                    btn.Tag = colonne;
                    btn.Click += Case_Click;
                    btn.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                    btn.VerticalContentAlignment = VerticalAlignment.Stretch;

                    Ellipse rond = new Ellipse();
                    rond.Fill = Brushes.White;
                    rond.Stroke = Brushes.Black;
                    rond.StrokeThickness = 1;
                    rond.Margin = new Thickness(3);
                    rond.Stretch = Stretch.Uniform;

                    btn.Content = rond;

                    _grilleBoutons[ligne, colonne] = btn;
                    GrillePlateau.Children.Add(btn);
                }
            }

            // 4. On démarre les chronos
            InitialiserChronos();
        }

        // --- MÉTHODES DU CHRONOMÈTRE ---
        private void InitialiserChronos()
        {
            _verrouPopup = false; // On déverrouille le jeu
            _tempsRestantGlobalJ1 = TempsChronoGlobale;
            _tempsRestantGlobalJ2 = TempsChronoGlobale;
            _tempsRestantReflexion = TempsReflexion;

            MettreAJourAffichageTemps();

            if (TempsChronoGlobale > 0 || TempsReflexion > 0)
            {
                if (_timer == null)
                {
                    _timer = new DispatcherTimer();
                    _timer.Interval = TimeSpan.FromSeconds(1);
                    _timer.Tick += Timer_Tick;
                }
                _timer.Start();
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            // Si le jeu est fini, ou qu'une popup est déjà ouverte, on coupe tout de suite ! (Fin de la boucle infinie)
            if (_maPartie == null || _maPartie.EstTerminee || _verrouPopup)
            {
                _timer?.Stop();
                return;
            }

            int idJoueurActuel = _maPartie.JoueurCourant.Id;

            // 1. Décompte du temps global
            if (TempsChronoGlobale > 0)
            {
                if (idJoueurActuel == 1) _tempsRestantGlobalJ1--;
                else _tempsRestantGlobalJ2--;

                // Temps écoulé pour l'un des joueurs
                if (_tempsRestantGlobalJ1 <= 0 || _tempsRestantGlobalJ2 <= 0)
                {
                    _verrouPopup = true; // On verrouille pour empêcher le fond de continuer
                    _timer?.Stop();

                    string perdant = _tempsRestantGlobalJ1 <= 0 ? LblJoueur1.Text : LblJoueur2.Text;
                    string gagnant = _tempsRestantGlobalJ1 <= 0 ? LblJoueur2.Text : LblJoueur1.Text;

                    MessageBox.Show($"Temps global écoulé pour {perdant} !\nVictoire de {gagnant} par forfait.", "Temps écoulé", MessageBoxButton.OK, MessageBoxImage.Warning);

                    if (idJoueurActuel == 1) _scoreJ2++; else _scoreJ1++;
                    AfficherEcranFin($"Victoire de {gagnant} (Temps écoulé)");
                    return;
                }
            }

            // 2. Décompte du temps de réflexion
            if (TempsReflexion > 0)
            {
                _tempsRestantReflexion--;

                if (_tempsRestantReflexion <= 0)
                {
                    _verrouPopup = true; // On verrouille
                    _timer?.Stop();

                    MessageBox.Show($"Temps de réflexion écoulé ! Le joueur passe son tour.", "Temps écoulé", MessageBoxButton.OK, MessageBoxImage.Information);

                    _maPartie.ChangerJoueur();
                    _tempsRestantReflexion = TempsReflexion; // Remise à zéro pour le joueur suivant
                    LblTourJoueur.Text = "C'est au tour de " + (_maPartie.JoueurCourant.Id == 1 ? LblJoueur1.Text : LblJoueur2.Text);

                    _verrouPopup = false; // On déverrouille
                    _timer?.Start();

                    MettreAJourAffichageTemps();
                    return;
                }
            }

            MettreAJourAffichageTemps();
        }

        private void MettreAJourAffichageTemps()
        {
            if (TempsChronoGlobale > 0)
            {
                LblChronoJ1.Text = $"Chronomètre : {TimeSpan.FromSeconds(_tempsRestantGlobalJ1):mm\\:ss}";
                LblChronoJ2.Text = $"Chronomètre : {TimeSpan.FromSeconds(_tempsRestantGlobalJ2):mm\\:ss}";
            }

            if (TempsReflexion > 0 && _maPartie != null)
            {
                string tRef = TimeSpan.FromSeconds(_tempsRestantReflexion).ToString(@"mm\:ss");

                // Le temps de réflexion ne s'affiche que du côté de celui qui doit jouer
                if (_maPartie.JoueurCourant.Id == 1)
                {
                    LblReflexionJ1.Text = $"Réflexion : {tRef}";
                    LblReflexionJ2.Text = $"Réflexion : --:--";
                }
                else
                {
                    LblReflexionJ1.Text = $"Réflexion : --:--";
                    LblReflexionJ2.Text = $"Réflexion : {tRef}";
                }
            }
        }

        // --- ACTIONS EN JEU ---
        private void Case_Click(object sender, RoutedEventArgs e)
        {
            // On empêche le clic si une popup est en train de s'ouvrir
            if (_maPartie == null || _maPartie.EstTerminee || _verrouPopup) return;

            Button btn = (Button)sender;
            int colonne = (int)btn.Tag;

            int ligneCible = -1;
            for (int ligne = NbLignes - 1; ligne >= 0; ligne--)
            {
                if (_maPartie.GrilleJeu.GetPion(ligne, colonne) == null)
                {
                    ligneCible = ligne;
                    break;
                }
            }

            if (ligneCible == -1) return;

            int idJoueurActuel = _maPartie.JoueurCourant.Id;
            bool coupReussi = _maPartie.JouerCoup(colonne);

            if (coupReussi)
            {
                Ellipse rond = (Ellipse)_grilleBoutons[ligneCible, colonne].Content;
                rond.Fill = (idJoueurActuel == 1) ? Brushes.Red : Brushes.Gold;

                // Réinitialiser le temps de réflexion pour le joueur suivant
                _tempsRestantReflexion = TempsReflexion;

                if (_maPartie.EstTerminee)
                {
                    string messageTitre;
                    if (_maPartie.EstEgalite)
                    {
                        messageTitre = "Égalité";
                    }
                    else
                    {
                        messageTitre = $"Victoire de {_maPartie.Gagnant!.Nom}";
                        if (_maPartie.Gagnant.Id == 1) _scoreJ1++;
                        else _scoreJ2++;
                    }

                    AfficherEcranFin(messageTitre);
                }
                else
                {
                    LblTourJoueur.Text = "C'est au tour de " + (_maPartie.JoueurCourant.Id == 1 ? LblJoueur1.Text : LblJoueur2.Text);
                    MettreAJourAffichageTemps();
                }
            }
        }

        private void AfficherEcranFin(string messageTitre)
        {
            _verrouPopup = true; // Verrouille définitivement le timer pour la fin de la partie
            _timer?.Stop();

            string texteScore = $"Score total des parties : (J1) {_scoreJ1} - {_scoreJ2} (J2)";

            FinPartie fenetreFin = new FinPartie(messageTitre, texteScore);
            fenetreFin.ShowDialog();

            if (fenetreFin.ActionChoisie == "Relancer")
            {
                RelancerPartie();
            }
            else if (fenetreFin.ActionChoisie == "Quitter")
            {
                Application.Current.Shutdown();
            }
            else
            {
                this.Close();
            }
        }

        private void RelancerPartie()
        {
            Joueur j1 = _maPartie.Joueur1;
            Joueur j2 = _maPartie.Joueur2;
            _maPartie = new Partie(NbLignes, NbColonnes, NbAAligner, j1, j2);

            for (int ligne = 0; ligne < NbLignes; ligne++)
            {
                for (int colonne = 0; colonne < NbColonnes; colonne++)
                {
                    Ellipse rond = (Ellipse)_grilleBoutons[ligne, colonne].Content;
                    rond.Fill = Brushes.White;
                }
            }

            LblTourJoueur.Text = "C'est au tour de " + LblJoueur1.Text;
            InitialiserChronos(); // Va automatiquement déverrouiller le jeu
        }
    }
}