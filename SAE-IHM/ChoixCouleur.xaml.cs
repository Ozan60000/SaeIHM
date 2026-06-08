using System;
using System.Windows;
using System.Windows.Controls;

namespace SAE_IHM
{
    public partial class ChoixCouleur : Window
    {
        private readonly string[] _couleursDisponibles = { "Rouge", "Jaune", "Bleu", "Violet", "Vert" };
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

            string couleurAdversaire = (RadJ1.IsChecked == true) ? ConfigurationGlobale.CouleurJ2 : ConfigurationGlobale.CouleurJ1;

            if (nomCouleur == "Aléatoire")
            {
                do
                {
                    nomCouleur = _couleursDisponibles[_rnd.Next(_couleursDisponibles.Length)];
                } while (nomCouleur == couleurAdversaire);
            }
            else
            {
                if (nomCouleur == couleurAdversaire)
                {
                    MessageBox.Show("Cette couleur est déjà utilisée par l'autre joueur. Veuillez en choisir une autre.",
                                    "Couleur indisponible",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    return;
                }
            }

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