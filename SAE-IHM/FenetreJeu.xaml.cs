using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Puissance4_Systeme;

namespace SAE_IHM
{
    public partial class FenetreJeu : Window
    {
        public string TypeAdversaire { get; set; } = "Local";
        public int NbLignes { get; set; } = 6;
        public int NbColonnes { get; set; } = 7;
        public int NbAAligner { get; set; } = 4;
        public int TempsChronoGlobale { get; set; } = 0;
        public int TempsReflexion { get; set; } = 0;

        private Partie _maPartie = null!;
        private Button[,] _grilleBoutons = null!;
        private int _scoreJ1 = 0;
        private int _scoreJ2 = 0;

        private DispatcherTimer _timer = null!;
        private DispatcherTimer _timerIA = null!;
        private int _tempsRestantGlobalJ1;
        private int _tempsRestantGlobalJ2;
        private int _tempsRestantReflexion;
        private bool _verrouPopup = false;
        private readonly object _verrouMoteur = new object();

        public FenetreJeu()
        {
            InitializeComponent();

            ConfigurationGlobale.AppliquerTheme(this);

            this.Closed += (s, e) => { _timer?.Stop(); _timerIA?.Stop(); };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LblJoueur2.Text = (TypeAdversaire == "Virtuel") ? "Ordinateur" : "Joueur 2";
            LblTourJoueur.Text = "C'est au tour de " + LblJoueur1.Text;

            Joueur j1 = new JoueurHumain(1, LblJoueur1.Text);
            Joueur j2 = (TypeAdversaire == "Virtuel") ? new JoueurIA(2, LblJoueur2.Text) : new JoueurHumain(2, LblJoueur2.Text);

            _maPartie = new Partie(NbLignes, NbColonnes, NbAAligner, j1, j2);
            _grilleBoutons = new Button[NbLignes, NbColonnes];

            GrillePlateau.Rows = NbLignes;
            GrillePlateau.Columns = NbColonnes;

            for (int ligne = 0; ligne < NbLignes; ligne++)
            {
                for (int colonne = 0; colonne < NbColonnes; colonne++)
                {
                    Button btn = new Button { Background = Brushes.Transparent, BorderThickness = new Thickness(0), Tag = colonne };
                    btn.Click += Case_Click;
                    btn.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                    btn.VerticalContentAlignment = VerticalAlignment.Stretch;

                    // C'est beaucoup plus propre comme ça !
                    btn.Content = CreerForme("Cercle", Brushes.White);

                    _grilleBoutons[ligne, colonne] = btn;
                    GrillePlateau.Children.Add(btn);
                }
            }

            IndicateurJ1.Content = CreerForme(ConfigurationGlobale.FormeJ1, ObtenirCouleur(ConfigurationGlobale.CouleurJ1));
            IndicateurJ2.Content = CreerForme(ConfigurationGlobale.FormeJ2, ObtenirCouleur(ConfigurationGlobale.CouleurJ2));

            _timerIA = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            _timerIA.Tick += TimerIA_Tick;

            InitialiserChronos();

            // On donne le focus au bouton central de la première ligne pour démarrer au clavier
            _grilleBoutons[0, NbColonnes / 2].Focus();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && BtnPause.IsEnabled) MettreEnPause();
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            MettreEnPause();
        }

        private void MettreEnPause()
        {
            if (_maPartie == null || _maPartie.EstTerminee || _verrouPopup) return;

            _verrouPopup = true;
            _timer?.Stop();

            Pause fenetrePause = new Pause();
            fenetrePause.ShowDialog();

            if (fenetrePause.ActionChoisie == "Menu") this.Close();
            else if (fenetrePause.ActionChoisie == "Quitter") Application.Current.Shutdown();
            else { _verrouPopup = false; _timer?.Start(); }
        }

        private SolidColorBrush ObtenirCouleur(string nomCouleur)
        {
            return nomCouleur switch
            {
                "Rouge" => Brushes.Red,
                "Jaune" => Brushes.Gold,
                "Bleu" => Brushes.MidnightBlue,
                "Violet" => Brushes.MediumPurple,
                "Vert" => Brushes.ForestGreen,
                _ => Brushes.Gray,
            };
        }

        // On change le retour "Shape" en "UIElement"
        private UIElement CreerForme(string nomForme, SolidColorBrush couleur)
        {
            // 1. On donne une taille fixe (40x40) à toutes nos formes pour qu'elles aient un ratio 1:1 parfait
            Shape forme = nomForme switch
            {
                "Carré" => new Rectangle { Width = 40, Height = 40 },
                "Losange" => new Path { Data = Geometry.Parse("M 20,0 L 40,20 L 20,40 L 0,20 Z"), Width = 40, Height = 40 },
                "Étoile" => new Path { Data = Geometry.Parse("M 20,0 L 25,15 L 40,15 L 28,25 L 32,40 L 20,30 L 8,40 L 12,25 L 0,15 L 15,15 Z"), Width = 40, Height = 40 },
                "Hexagone" => new Path { Data = Geometry.Parse("M 10,0 L 30,0 L 40,20 L 30,40 L 10,40 L 0,20 Z"), Width = 40, Height = 40 },
                _ => new Ellipse { Width = 40, Height = 40 }
            };

            forme.Fill = couleur;
            forme.Stroke = Brushes.Black;
            forme.StrokeThickness = 1;
            forme.Stretch = Stretch.Uniform;

            // 2. On met notre forme dans un Viewbox magique !
            // C'est lui qui va s'occuper d'agrandir l'étoile dans la grille (ex: 80x80)
            // ou de la rétrécir dans les indicateurs en bas (22x22) avec un centrage parfait.
            Viewbox vb = new Viewbox();
            vb.Margin = new Thickness(3);
            vb.Child = forme;

            return vb;
        }

        private void InitialiserChronos()
        {
            _verrouPopup = false;
            _tempsRestantGlobalJ1 = TempsChronoGlobale;
            _tempsRestantGlobalJ2 = TempsChronoGlobale;
            _tempsRestantReflexion = TempsReflexion;
            MettreAJourAffichageTemps();

            if (TempsChronoGlobale > 0 || TempsReflexion > 0)
            {
                if (_timer == null) { _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) }; _timer.Tick += Timer_Tick; }
                _timer.Start();
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (_maPartie == null || _maPartie.EstTerminee || _verrouPopup) return;

            if (TempsChronoGlobale > 0)
            {
                if (_maPartie.JoueurCourant.Id == 1) _tempsRestantGlobalJ1--; else _tempsRestantGlobalJ2--;
                if (_tempsRestantGlobalJ1 <= 0 || _tempsRestantGlobalJ2 <= 0)
                {
                    AfficherEcranFin($"Victoire de {(_tempsRestantGlobalJ1 <= 0 ? LblJoueur2.Text : LblJoueur1.Text)} (Temps écoulé)");
                    return;
                }
            }

            if (TempsReflexion > 0)
            {
                _tempsRestantReflexion--;
                if (_tempsRestantReflexion <= 0)
                {
                    _timer.Stop();
                    MessageBox.Show("Temps de réflexion écoulé !");
                    lock (_verrouMoteur) { _maPartie.ChangerJoueur(); }
                    _tempsRestantReflexion = TempsReflexion;
                    _timer.Start();
                    if (_maPartie.JoueurCourant is JoueurIA) { _verrouPopup = true; _timerIA.Start(); }
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
                string t = TimeSpan.FromSeconds(_tempsRestantReflexion).ToString(@"mm\:ss");
                LblReflexionJ1.Text = $"Réflexion : {(_maPartie.JoueurCourant.Id == 1 ? t : "--:--")}";
                LblReflexionJ2.Text = $"Réflexion : {(_maPartie.JoueurCourant.Id == 2 ? t : "--:--")}";
            }
        }

        private void Case_Click(object sender, RoutedEventArgs e)
        {
            if (_maPartie == null || _maPartie.EstTerminee || _verrouPopup || _maPartie.JoueurCourant is JoueurIA) return;
            lock (_verrouMoteur) { TraiterCoup((int)((Button)sender).Tag); }
        }

        private void TimerIA_Tick(object? sender, EventArgs e)
        {
            _timerIA.Stop();
            lock (_verrouMoteur)
            {
                if (_maPartie != null && !_maPartie.EstTerminee && _maPartie.JoueurCourant is JoueurIA ia)
                {
                    int col = ia.ChoisirCoup(_maPartie.GrilleJeu);
                    if (col != -1) TraiterCoup(col);
                }
            }
        }

        private void TraiterCoup(int colonne)
        {
            int ligneCible = -1;
            for (int l = NbLignes - 1; l >= 0; l--) if (_maPartie.GrilleJeu.GetPion(l, colonne) == null) { ligneCible = l; break; }
            if (ligneCible == -1) return;

            int idActuel = _maPartie.JoueurCourant.Id;
            if (_maPartie.JouerCoup(colonne))
            {
                SolidColorBrush couleurJoueur = (idActuel == 1) ? ObtenirCouleur(ConfigurationGlobale.CouleurJ1) : ObtenirCouleur(ConfigurationGlobale.CouleurJ2);
                string formeJoueur = (idActuel == 1) ? ConfigurationGlobale.FormeJ1 : ConfigurationGlobale.FormeJ2;

                _grilleBoutons[ligneCible, colonne].Content = CreerForme(formeJoueur, couleurJoueur);
                _tempsRestantReflexion = TempsReflexion;

                if (_maPartie.EstTerminee) AfficherEcranFin(_maPartie.EstEgalite ? "Égalité" : $"Victoire de {_maPartie.Gagnant!.Nom}");
                else
                {
                    LblTourJoueur.Text = "C'est au tour de " + (_maPartie.JoueurCourant.Id == 1 ? LblJoueur1.Text : LblJoueur2.Text);
                    MettreAJourAffichageTemps();

                    if (_maPartie.JoueurCourant is JoueurIA) { _verrouPopup = true; _timerIA.Start(); }
                    else { _verrouPopup = false; }
                }
            }
        }

        private void AfficherEcranFin(string titre)
        {
            _verrouPopup = true; _timer?.Stop(); _timerIA?.Stop();
            if (titre.Contains("Victoire")) { if (_maPartie.Gagnant?.Id == 1) _scoreJ1++; else _scoreJ2++; }
            FinPartie fin = new FinPartie(titre, $"Score : J1 {_scoreJ1} - {_scoreJ2} J2");
            fin.ShowDialog();
            if (fin.ActionChoisie == "Relancer") RelancerPartie();
            else if (fin.ActionChoisie == "Quitter") Application.Current.Shutdown();
            else this.Close();
        }

        private void RelancerPartie()
        {
            _maPartie = new Partie(NbLignes, NbColonnes, NbAAligner, _maPartie.Joueur1, _maPartie.Joueur2);
            for (int ligne = 0; ligne < NbLignes; ligne++)
            {
                for (int colonne = 0; colonne < NbColonnes; colonne++)
                {
                    _grilleBoutons[ligne, colonne].Content = CreerForme("Cercle", Brushes.White);
                }
            }
            LblTourJoueur.Text = "C'est au tour de " + LblJoueur1.Text;
            InitialiserChronos();
            if (_maPartie.JoueurCourant is JoueurIA) { _verrouPopup = true; _timerIA.Start(); }
        }
    }
}