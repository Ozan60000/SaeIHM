using System.Windows;

namespace SAE_IHM
{
    public partial class Historique : Window
    {
        public Historique()
        {
            InitializeComponent();
        }

        private void BtnFermer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}