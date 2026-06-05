using System.Windows;
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

            // Construction visuelle de la grille
            GrillePlateau.Rows = NbLignes;
            GrillePlateau.Columns = NbColonnes;

            for (int ligne = 0; ligne < NbLignes; ligne++)
            {
                for (int colonne = 0; colonne < NbColonnes; colonne++)
                {
                    Ellipse rond = new Ellipse();
                    rond.Fill = Brushes.White;
                    rond.Stroke = Brushes.Black;
                    rond.StrokeThickness = 1;
                    rond.Margin = new Thickness(3);
                    GrillePlateau.Children.Add(rond);
                }
            }
        }
    }
}