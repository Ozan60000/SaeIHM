using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SAE_IHM
{
    public partial class FenetreJeu : Window
    {
        // Paramètres reçus depuis la MainWindow
        public string TypeAdversaire { get; set; } = "Local";
        public int NbLignes { get; set; } = 6;
        public int NbColonnes { get; set; } = 7;
        public int NbAAligner { get; set; } = 4;

        // État interne de la partie
        private int[,] _grilleEtat = null!;   // 0 = vide, 1 = J1, 2 = J2
        private Button[,] _grilleBoutons = null!; // Pour récupérer les boutons par position
        private int _joueurCourant = 1;   // 1 ou 2

        public FenetreJeu()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Nom du joueur 2 selon le type d'adversaire choisi
            if (TypeAdversaire == "Virtuel")
            {
                LblJoueur2.Text = "Ordinateur";
            }
            else
            {
                LblJoueur2.Text = "Joueur 2";
            }

            // Affichage initial du tour
            LblTourJoueur.Text = "C'est au tour de " + LblJoueur1.Text;

            // Initialisation des tableaux d'état
            _grilleEtat = new int[NbLignes, NbColonnes];
            _grilleBoutons = new Button[NbLignes, NbColonnes];

            // Construction visuelle de la grille
            GrillePlateau.Rows = NbLignes;
            GrillePlateau.Columns = NbColonnes;

            for (int ligne = 0; ligne < NbLignes; ligne++)
            {
                for (int colonne = 0; colonne < NbColonnes; colonne++)
                {
                    // Un Button transparent contient une Ellipse
                    Button btn = new Button();
                    btn.Background = Brushes.Transparent;
                    btn.BorderThickness = new Thickness(0);
                    btn.Tag = colonne; // On stocke le numéro de colonne pour le retrouver au clic
                    btn.Click += Case_Click;

                    Ellipse rond = new Ellipse();
                    rond.Fill = Brushes.White;
                    rond.Stroke = Brushes.Black;
                    rond.StrokeThickness = 1;
                    rond.Margin = new Thickness(3);
                    btn.Content = rond;

                    _grilleBoutons[ligne, colonne] = btn;
                    GrillePlateau.Children.Add(btn);
                }
            }
        }

        private void Case_Click(object sender, RoutedEventArgs e)
        {
            // On récupère la colonne sur laquelle le joueur a cliqué
            Button btn = (Button)sender;
            int colonne = (int)btn.Tag;

            // On cherche la première case vide en partant du bas de la colonne
            int ligneCible = -1;
            for (int ligne = NbLignes - 1; ligne >= 0; ligne--)
            {
                if (_grilleEtat[ligne, colonne] == 0)
                {
                    ligneCible = ligne;
                    break;
                }
            }

            // Si la colonne est pleine, on ne fait rien
            if (ligneCible == -1)
            {
                return;
            }

            // On place le pion dans la matrice d'état
            _grilleEtat[ligneCible, colonne] = _joueurCourant;

            // On colore l'Ellipse de la case correspondante
            Ellipse rond = (Ellipse)_grilleBoutons[ligneCible, colonne].Content;
            if (_joueurCourant == 1)
            {
                rond.Fill = Brushes.Red;
            }
            else
            {
                rond.Fill = Brushes.Gold;
            }

            // On change de joueur et on met à jour le titre
            if (_joueurCourant == 1)
            {
                _joueurCourant = 2;
                LblTourJoueur.Text = "C'est au tour de " + LblJoueur2.Text;
            }
            else
            {
                _joueurCourant = 1;
                LblTourJoueur.Text = "C'est au tour de " + LblJoueur1.Text;
            }
        }
    }
}