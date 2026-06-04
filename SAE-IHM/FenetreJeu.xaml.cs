using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SAE_IHM
{
    public partial class FenetreJeu : Window
    {
        // Propriétés publiques qu'on alimente depuis MainWindow avant ShowDialog
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
            // Titre avec la date du jour
            LblTitre.Text = "Partie 1 du " + DateTime.Now.ToString("dd/MM/yyyy");

            // Nom du joueur 2 selon l'adversaire choisi
            if (TypeAdversaire == "Virtuel")
            {
                LblJoueur2.Text = "Ordinateur";
            }
            else
            {
                LblJoueur2.Text = "Joueur 2";
            }

            // Construction de la grille de jeu
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