using System;
using System.Windows;
using System.Windows.Controls;

namespace SAE_IHM
{
    public partial class ChoixCouleur : Window
    {
        // 1. On liste les couleurs qui sont gérées par ta méthode ObtenirCouleur() dans FenetreJeu
        private readonly string[] _couleursDisponibles = { "Rouge", "Jaune", "Bleu", "Violet", "Vert" };

        // 2. On prépare le générateur de nombres aléatoires
        private Random _rnd = new Random();

        public ChoixCouleur()
        {
            InitializeComponent();

            ConfigurationGlobale.AppliquerTheme(this);

            LblJ1.Text = "Joueur 1 : " + ConfigurationGlobale.CouleurJ1;
            LblJ2.Text = "Joueur 2 : " + ConfigurationGlobale.CouleurJ2;
        }

        private void BtnCouleur_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string nomCouleur = btn.Tag?.ToString() ?? "Rouge";

            // 1. On regarde quelle est la couleur de l'adversaire
            string couleurAdversaire = (RadJ1.IsChecked == true) ? ConfigurationGlobale.CouleurJ2 : ConfigurationGlobale.CouleurJ1;

            if (nomCouleur == "Aléatoire")
            {
                // 2. On pioche en boucle TANT QUE la couleur tirée est celle de l'adversaire
                do
                {
                    nomCouleur = _couleursDisponibles[_rnd.Next(_couleursDisponibles.Length)];
                } while (nomCouleur == couleurAdversaire);
            }
            else
            {
                // 3. Si le joueur a cliqué sur une couleur déjà prise
                if (nomCouleur == couleurAdversaire)
                {
                    MessageBox.Show("Cette couleur est déjà utilisée par l'autre joueur. Veuillez en choisir une autre.",
                                    "Couleur indisponible",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    return; // On arrête tout, on ne valide pas le choix
                }
            }

            // 4. Si on arrive ici, c'est que la couleur est valide !
            if (RadJ1.IsChecked == true)
                ConfigurationGlobale.CouleurJ1 = nomCouleur;
            else
                ConfigurationGlobale.CouleurJ2 = nomCouleur;

            LblJ1.Text = "Joueur 1 : " + ConfigurationGlobale.CouleurJ1;
            LblJ2.Text = "Joueur 2 : " + ConfigurationGlobale.CouleurJ2;
        }

        private void BtnFermer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}