using System.Windows;

namespace SAE_IHM
{
    public partial class SelectionPartie : Window
    {
        public SelectionPartie()
        {
            InitializeComponent();
        }

        private void BtnFermer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}