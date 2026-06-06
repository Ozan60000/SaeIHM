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
                if (_maPartie.EstTerminee)
                {
                    if (_maPartie.EstEgalite)
                    {
                        MessageBox.Show("Match nul ! La grille est pleine.", "Fin de partie", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Victoire de {_maPartie.Gagnant!.Nom} !", "Fin de partie", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
                else
                {
                    // On met à jour le texte du tour suivant
                    LblTourJoueur.Text = "C'est au tour de " + (_maPartie.JoueurCourant.Id == 1 ? LblJoueur1.Text : LblJoueur2.Text);

                    // TODO: Si on joue contre l'IA, c'est ici qu'on appellera la méthode de Thomas !
                }
            }
        }
    }
}