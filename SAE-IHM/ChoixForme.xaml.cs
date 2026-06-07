using System;
using System.Windows;
using System.Windows.Controls;

namespace SAE_IHM
{
    public partial class ChoixForme : Window
    {
        private readonly string[] _formesDisponibles = { "Cercle", "Carré", "Losange", "Étoile", "Hexagone" };
        private Random _rnd = new Random();

        public ChoixForme()
        {
            InitializeComponent();
            LblJ1.Text = "Joueur 1 : " + ConfigurationGlobale.FormeJ1;
            LblJ2.Text = "Joueur 2 : " + ConfigurationGlobale.FormeJ2;
        }

        private void BtnForme_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string choix = btn.Tag?.ToString() ?? "Cercle";

            if (choix == "Aléatoire")
            {
                choix = _formesDisponibles[_rnd.Next(_formesDisponibles.Length)];
            }

            if (RadJ1.IsChecked == true)
                ConfigurationGlobale.FormeJ1 = choix;
            else
                ConfigurationGlobale.FormeJ2 = choix;

            LblJ1.Text = "Joueur 1 : " + ConfigurationGlobale.FormeJ1;
            LblJ2.Text = "Joueur 2 : " + ConfigurationGlobale.FormeJ2;
        }

        private void BtnFermer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}