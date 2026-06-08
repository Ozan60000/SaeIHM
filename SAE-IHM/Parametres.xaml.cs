using System.Windows;

namespace SAE_IHM
{
    public partial class Parametres : Window
    {
        public Parametres()
        {
            InitializeComponent();
            ConfigurationGlobale.AppliquerTheme(this);

            SldTailleTexte.Value = ConfigurationGlobale.TailleTexte;
            SldContraste.Value = ConfigurationGlobale.Contraste;
        }

        private void SldTailleTexte_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (LblApercu != null)
            {
                LblApercu.FontSize = e.NewValue;
                ConfigurationGlobale.TailleTexte = e.NewValue;
                AppliquerPartout();
            }
        }

        private void SldContraste_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IsLoaded)
            {
                ConfigurationGlobale.Contraste = e.NewValue;
                AppliquerPartout();
            }
        }

        private void AppliquerPartout()
        {
            foreach (Window fenetre in Application.Current.Windows)
            {
                ConfigurationGlobale.AppliquerTheme(fenetre);
            }
        }

        private void BtnCouleurs_Click(object sender, RoutedEventArgs e)
        {
            ChoixCouleur fenetreCouleur = new ChoixCouleur();
            fenetreCouleur.ShowDialog();
        }

        private void BtnFormes_Click(object sender, RoutedEventArgs e)
        {
            ChoixForme fenetreForme = new ChoixForme();
            fenetreForme.ShowDialog();
        }

        private void BtnFermer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}