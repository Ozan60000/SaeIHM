using System;
using System.Windows;
using System.Windows.Controls;

namespace SAE_IHM
{
    public partial class Parametres : Window
    {
        // Propriétés accessibles depuis la MainWindow pour lire les réglages
        public int NbLignes { get; private set; }
        public int NbColonnes { get; private set; }
        public int NbAAligner { get; private set; }
        public bool ModeContreIA { get; private set; }

        public Parametres()
        {
            InitializeComponent();
        }

        private void BtnValider_Click(object sender, RoutedEventArgs e)
        {
            // On récupère les valeurs sélectionnées dans les ComboBox
            NbLignes = Convert.ToInt32(((ComboBoxItem)CboLignes.SelectedItem).Content);
            NbColonnes = Convert.ToInt32(((ComboBoxItem)CboColonnes.SelectedItem).Content);
            NbAAligner = Convert.ToInt32(((ComboBoxItem)CboAlignement.SelectedItem).Content);
            ModeContreIA = RadIA.IsChecked == true;

            // La propriété DialogResult à 'true' indique que l'utilisateur a bien validé ses choix
            this.DialogResult = true;
        }

        private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            // On ferme simplement la fenêtre sans valider
            this.DialogResult = false;
        }
    }
}