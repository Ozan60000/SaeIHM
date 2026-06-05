using System;
using System.Windows;
using System.Windows.Controls;

namespace SAE_IHM
{
    public partial class Regles : Window
    {
        // Propriétés lues par la MainWindow après validation
        public int NbLignes { get; private set; }
        public int NbColonnes { get; private set; }
        public int NbAAligner { get; private set; }

        public Regles()
        {
            InitializeComponent();
        }

        private void BtnValider_Click(object sender, RoutedEventArgs e)
        {
            // Récupération des valeurs sélectionnées
            NbLignes = Convert.ToInt32(((ComboBoxItem)CboLignes.SelectedItem).Content);
            NbColonnes = Convert.ToInt32(((ComboBoxItem)CboColonnes.SelectedItem).Content);
            NbAAligner = Convert.ToInt32(((ComboBoxItem)CboAlignement.SelectedItem).Content);

            this.DialogResult = true;
        }

        private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}