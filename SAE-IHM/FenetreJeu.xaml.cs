using System.Windows;
using System.Windows.Controls;

namespace SAE_IHM
{
    public partial class FenetreJeu : Window
    {
        public FenetreJeu()
        {
            InitializeComponent();
        }

        // Cet événement se déclenche quand un joueur clique sur une case de la grille
        private void Case_Click(object sender, RoutedEventArgs e)
        {
            // Grâce au sender, on pourra identifier dans quelle colonne le joueur a cliqué !
            // (La logique sera ajoutée ici)
        }
    }
}