using System.Windows;

namespace SAE_IHM
{
    public partial class Parametres : Window
    {
        public Parametres()
        {
            InitializeComponent();

            ConfigurationGlobale.AppliquerTheme(this);

            // On positionne les sliders au bon endroit quand on ouvre la fenêtre
            SldTailleTexte.Value = ConfigurationGlobale.TailleTexte;
            SldContraste.Value = ConfigurationGlobale.Contraste;
        }

        private void SldTailleTexte_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (LblApercu != null)
            {
                LblApercu.FontSize = e.NewValue; // Garde l'aperçu du "A"
                ConfigurationGlobale.TailleTexte = e.NewValue;
                AppliquerPartout(); // Met à jour l'application en direct
            }
        }

        private void SldContraste_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IsLoaded) // Empêche le bug de déclenchement à l'ouverture de la fenêtre
            {
                ConfigurationGlobale.Contraste = e.NewValue;
                AppliquerPartout(); // Met à jour l'application en direct
            }
        }

        // Boucle magique : applique le thème à toutes les fenêtres ouvertes (y compris l'arrière-plan)
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