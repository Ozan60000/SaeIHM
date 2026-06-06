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

        public FenetreJeu()
        {
            InitializeComponent();
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

                    Ellipse rond = new Ellipse { Fill = Brushes.White, Stroke = Brushes.Black, StrokeThickness = 1, Margin = new Thickness(3), Stretch = Stretch.Uniform };
                    btn.Content = rond;

                    _grilleBoutons[ligne, colonne] = btn;
                    GrillePlateau.Children.Add(btn);
                }
            }

            _timerIA = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            _timerIA.Tick += TimerIA_Tick;

            InitialiserChronos();
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
                    _verrouPopup = true; _timer.Stop();
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
                    _maPartie.ChangerJoueur();
                    _tempsRestantReflexion = TempsReflexion;
                    _timer.Start();
                    if (_maPartie.JoueurCourant is JoueurIA) _timerIA.Start();
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
            TraiterCoup((int)((Button)sender).Tag);
        }

        private void TimerIA_Tick(object? sender, EventArgs e)
        {
            _timerIA.Stop();
            _verrouPopup = false;
            if (_maPartie != null && !_maPartie.EstTerminee && _maPartie.JoueurCourant is JoueurIA ia)
                TraiterCoup(ia.ChoisirCoup(_maPartie.GrilleJeu));
        }

        private void TraiterCoup(int colonne)
        {
            int ligneCible = -1;
            for (int l = NbLignes - 1; l >= 0; l--) if (_maPartie.GrilleJeu.GetPion(l, colonne) == null) { ligneCible = l; break; }
            if (ligneCible == -1) return;

            int idActuel = _maPartie.JoueurCourant.Id;
            if (_maPartie.JouerCoup(colonne))
            {
                ((Ellipse)_grilleBoutons[ligneCible, colonne].Content).Fill = (idActuel == 1) ? Brushes.Red : Brushes.Gold;
                _tempsRestantReflexion = TempsReflexion;

                if (_maPartie.EstTerminee) AfficherEcranFin(_maPartie.EstEgalite ? "Égalité" : $"Victoire de {_maPartie.Gagnant!.Nom}");
                else
                {
                    LblTourJoueur.Text = "C'est au tour de " + (_maPartie.JoueurCourant.Id == 1 ? LblJoueur1.Text : LblJoueur2.Text);
                    MettreAJourAffichageTemps();
                    if (_maPartie.JoueurCourant is JoueurIA) { _verrouPopup = true; _timerIA.Start(); }
                }
            }
        }

        private void AfficherEcranFin(string titre)
        {
            _verrouPopup = true; _timer?.Stop(); _timerIA?.Stop();
            FinPartie fin = new FinPartie(titre, $"Score : J1 {_scoreJ1} - {_scoreJ2} J2");
            fin.ShowDialog();
            if (fin.ActionChoisie == "Relancer") RelancerPartie();
            else if (fin.ActionChoisie == "Quitter") Application.Current.Shutdown();
            else this.Close();
        }

        private void RelancerPartie()
        {
            _maPartie = new Partie(NbLignes, NbColonnes, NbAAligner, _maPartie.Joueur1, _maPartie.Joueur2);
            foreach (var b in _grilleBoutons) ((Ellipse)b.Content).Fill = Brushes.White;
            LblTourJoueur.Text = "C'est au tour de " + LblJoueur1.Text;
            InitialiserChronos();
        }
    }
}