using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SAE_IHM
{
    public static class ConfigurationGlobale
    {
        public static string CouleurJ1 { get; set; } = "Rouge";
        public static string CouleurJ2 { get; set; } = "Jaune";
        public static string FormeJ1 { get; set; } = "Cercle";
        public static string FormeJ2 { get; set; } = "Cercle";

        public static double TailleTexte { get; set; } = 14;
        public static double Contraste { get; set; } = 50;

        public static void AppliquerTheme(Window fenetre)
        {
            if (fenetre.Content is FrameworkElement contenu)
            {
                double ratio = TailleTexte / 14.0;
                contenu.LayoutTransform = new ScaleTransform(ratio, ratio);

                if (contenu is Panel panneau)
                {
                    double baseGris = 236;
                    double ajustement = (Contraste - 50) * 2;

                    double resultat = baseGris + ajustement;
                    if (resultat > 255) resultat = 255;
                    if (resultat < 80) resultat = 80;

                    byte gris = (byte)resultat;
                    panneau.Background = new SolidColorBrush(Color.FromRgb(gris, gris, gris));
                }
            }
        }
    }
}