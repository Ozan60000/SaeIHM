using System.Windows;
using System.Windows.Controls;

namespace SAE_IHM
{
    public partial class ChoixForme : Window
    {
        // Formes sélectionnées par défaut
        private string _formeJ1 = "Cercle";
        private string _formeJ2 = "Cercle";

        public ChoixForme()
        {
            InitializeComponent();
        }

        private void BtnForme_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string nomForme = btn.Tag.ToString();

            if (RadJ1.IsChecked == true)
            {
                _formeJ1 = nomForme;
            }
            else
            {
                _formeJ2 = nomForme;
            }

            LblJ1.Text = "Joueur 1 : " + _formeJ1;
            LblJ2.Text = "Joueur 2 : " + _formeJ2;
        }

        private void BtnFermer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}