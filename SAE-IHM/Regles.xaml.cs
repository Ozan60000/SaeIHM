using System;
using System.Windows;
using System.Windows.Controls;

namespace SAE_IHM
{
    public partial class Regles : Window
    {
        public int NbLignes { get; private set; }
        public int NbColonnes { get; private set; }
        public int NbAAligner { get; private set; }

        public int TempsChronoGlobale { get; private set; }
        public int TempsReflexion { get; private set; }

        public Regles()
        {
            InitializeComponent();
            ConfigurationGlobale.AppliquerTheme(this);
        }

        private void BtnValider_Click(object sender, RoutedEventArgs e)
        {
            NbLignes = Convert.ToInt32(((ComboBoxItem)CboLignes.SelectedItem).Content);
            NbColonnes = Convert.ToInt32(((ComboBoxItem)CboColonnes.SelectedItem).Content);
            NbAAligner = Convert.ToInt32(((ComboBoxItem)CboAlignement.SelectedItem).Content);

            TempsChronoGlobale = 0;
            if (ChkChrono.IsChecked == true)
            {
                if (RadChrono1.IsChecked == true) TempsChronoGlobale = 60;
                else if (RadChrono5.IsChecked == true) TempsChronoGlobale = 300;
                else if (RadChrono10.IsChecked == true) TempsChronoGlobale = 600;
            }

            TempsReflexion = 0;
            if (ChkReflexion.IsChecked == true)
            {
                if (RadReflexion30.IsChecked == true) TempsReflexion = 30;
                else if (RadReflexion1.IsChecked == true) TempsReflexion = 60;
                else if (RadReflexion2.IsChecked == true) TempsReflexion = 120;
            }

            this.DialogResult = true;
        }

        private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}