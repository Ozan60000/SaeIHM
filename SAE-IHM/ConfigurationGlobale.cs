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

        // Nouvelles variables pour stocker les paramètres du slider
        public static double TailleTexte { get; set; } = 14;
        public static double Contraste { get; set; } = 50;

        // Cette méthode va être appelée pour appliquer le visuel à une fenêtre
        public static void AppliquerTheme(Window fenetre)
        {
            if (fenetre.Content is FrameworkElement contenu)
            {
                // 1. TAILLE DU TEXTE (On fait un Zoom global sur la fenêtre)
                // 14 est la valeur par défaut (Zoom = 100%)
                double ratio = TailleTexte / 14.0;
                contenu.LayoutTransform = new ScaleTransform(ratio, ratio);

                // 2. CONTRASTE (On modifie la couleur de fond)
                if (contenu is Panel panneau)
                {
                    // 50 = Couleur par défaut (#ECF0F1 -> 236 en RGB)
                    double baseGris = 236;
                    double ajustement = (Contraste - 50) * 2; // Si > 50 on éclarcit vers le blanc pur, si < 50 on assombrit

                    double resultat = baseGris + ajustement;
                    if (resultat > 255) resultat = 255;
                    if (resultat < 80) resultat = 80; // Limite pour ne pas rendre le texte noir illisible sur fond noir

                    byte gris = (byte)resultat;
                    panneau.Background = new SolidColorBrush(Color.FromRgb(gris, gris, gris));
                }
            }
        }
    }
}