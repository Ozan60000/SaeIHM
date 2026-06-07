using System.Windows;

namespace SAE_IHM
{
    public partial class Parametres : Window
    {
        public Parametres()
        {
            InitializeComponent();
        }

        private void SldTailleTexte_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // On change la taille du "A" d'aperçu dans le XAML si le slider bouge
            if (LblApercu != null)
            {
                LblApercu.FontSize = e.NewValue;
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