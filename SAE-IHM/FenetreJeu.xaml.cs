using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Puissance4_Systeme; // <-- TRÈS IMPORTANT : On appelle votre moteur système !

namespace SAE_IHM
{
    public partial class FenetreJeu : Window
    {
        // Paramètres reçus depuis la MainWindow
        public string TypeAdversaire { get; set; } = "Local";
        public int NbLignes { get; set; } = 6;
        public int NbColonnes { get; set; } = 7;
        public int NbAAligner { get; set; } = 4;

        // --- LE VRAI MOTEUR DE JEU ---
        private Partie _maPartie = null!;
        private Button[,] _grilleBoutons = null!;
        private int _scoreJ1 = 0;
        private int _scoreJ2 = 0;

        public FenetreJeu()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (TypeAdversaire == "Virtuel") LblJoueur2.Text = "Ordinateur";
            else LblJoueur2.Text = "Joueur 2";

            LblTourJoueur.Text = "C'est au tour de " + LblJoueur1.Text;

            // 1. On crée les joueurs avec votre modèle objet
            Joueur j1 = new JoueurHumain(1, LblJoueur1.Text);
            Joueur j2;
            if (TypeAdversaire == "Virtuel")
                j2 = new JoueurIA(2, LblJoueur2.Text);
            else
                j2 = new JoueurHumain(2, LblJoueur2.Text);

            // 2. On démarre la VRAIE partie dans le système !
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

                    // Étirement du bouton (la correction d'avant !)
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
        }

        private void Case_Click(object sender, RoutedEventArgs e)
        {
            // Si la partie est déjà finie, on bloque les clics
            if (_maPartie == null || _maPartie.EstTerminee) return;

            Button btn = (Button)sender;
            int colonne = (int)btn.Tag;

            // On cherche la ligne visuellement en lisant le moteur
            int ligneCible = -1;
            for (int ligne = NbLignes - 1; ligne >= 0; ligne--)
            {
                if (_maPartie.GrilleJeu.GetPion(ligne, colonne) == null)
                {
                    ligneCible = ligne;
                    break;
                }
            }

            if (ligneCible == -1) return; // Colonne pleine, on ne fait rien

            // On mémorise l'ID du joueur avant que le moteur ne change de tour
            int idJoueurActuel = _maPartie.JoueurCourant.Id;

            // --- ON ENVOIE L'ACTION AU VRAI MOTEUR DE JEU ! ---
            bool coupReussi = _maPartie.JouerCoup(colonne);

            if (coupReussi)
            {
                // On met à jour l'interface visuelle
                Ellipse rond = (Ellipse)_grilleBoutons[ligneCible, colonne].Content;
                rond.Fill = (idJoueurActuel == 1) ? Brushes.Red : Brushes.Gold;

                // --- VÉRIFICATION DE LA VICTOIRE ---
                // --- VÉRIFICATION DE LA VICTOIRE ---
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
                        // On incrémente le bon score
                        if (_maPartie.Gagnant.Id == 1) _scoreJ1++;
                        else _scoreJ2++;
                    }

                    string texteScore = $"Score total des parties : (J1) {_scoreJ1} - {_scoreJ2} (J2)";

                    // On ouvre la nouvelle fenêtre de fin
                    FinPartie fenetreFin = new FinPartie(messageTitre, texteScore);
                    fenetreFin.ShowDialog();

                    // On regarde ce que le joueur a choisi
                    if (fenetreFin.ActionChoisie == "Relancer")
                    {
                        RelancerPartie();
                    }
                    else if (fenetreFin.ActionChoisie == "Quitter")
                    {
                        Application.Current.Shutdown(); // Coupe tout le programme
                    }
                    else // "Menu"
                    {
                        this.Close(); // Ferme la fenêtre de jeu, ce qui ramènera au menu principal
                    }
                }
                else
                {
                    // On met à jour le texte du tour suivant
                    LblTourJoueur.Text = "C'est au tour de " + (_maPartie.JoueurCourant.Id == 1 ? LblJoueur1.Text : LblJoueur2.Text);
                }
            }
        }
        private void RelancerPartie()
        {
            // 1. On recrée une partie neuve côté moteur
            Joueur j1 = _maPartie.Joueur1;
            Joueur j2 = _maPartie.Joueur2;
            _maPartie = new Partie(NbLignes, NbColonnes, NbAAligner, j1, j2);

            // 2. On blanchit tous les cercles côté visuel
            for (int ligne = 0; ligne < NbLignes; ligne++)
            {
                for (int colonne = 0; colonne < NbColonnes; colonne++)
                {
                    Ellipse rond = (Ellipse)_grilleBoutons[ligne, colonne].Content;
                    rond.Fill = Brushes.White;
                }
            }

            // 3. On remet l'affichage du tour à zéro
            LblTourJoueur.Text = "C'est au tour de " + LblJoueur1.Text;
        }
    }
}