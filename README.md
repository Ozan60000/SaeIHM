#  Puissance 4 - Application WPF (SAE 2.01, 2.02, 2.04)

Bienvenue sur le dépôt de notre projet transversal de fin d'année ! 

Ce projet est une implémentation du jeu **Puissance 4**, développée en **C#** avec une interface graphique **WPF**. Il met en pratique les concepts d'ergonomie (IHM), de conception orientée objet et d'intelligence artificielle.

##  Fonctionnalités principales

* **Parties sur mesure :**
    * Taille de la grille dynamique et modifiable (de 4x4 jusqu'à 10x10).
    * Conditions de victoire ajustables (alignement de 3, 4, 5 ou 6 jetons).
* **Adversaires multiples :**
    * Mode multijoueur local (Humain vs Humain).
    * Mode contre l'IA avec algorithme Minimax et élagage Alpha-Bêta.
* **Gestion avancée du temps (Pendule type échecs) :**
    * Chronomètre global par joueur (défaite par forfait si le temps est écoulé).
    * Temps de réflexion limité par tour (passage de tour automatique en cas de dépassement).
* **Personnalisation & Accessibilité :**
    * Modification de la couleur des jetons pour chaque joueur.
    * Modification de la forme des jetons (Cercle, Carré, Losange, Étoile).
* **Historique et Scores :**
    * Suivi du score global des joueurs sur la session en cours.
    * Écrans de fin de partie interactifs avec options de relance rapide.

##  Architecture du Projet

Le projet respecte une séparation entre le moteur logique et l'interface utilisateur :

1. **`Puissance4_Systeme` (Bibliothèque de classes) :** Le "cerveau" de l'application. Contient la logique mathématique, la physique de la grille (gravité des pions), la détection générique des victoires et les algorithmes de l'Intelligence Artificielle.
2. **`SAE_IHM` (Application WPF) :** L'interface graphique. Gère l'affichage XAML, la navigation entre les menus en cascade (Menu Principal ➔ Adversaire ➔ Règles ➔ Jeu), les pop-ups et la boucle temporelle (`DispatcherTimer`).

## 🛠️ Technologies Utilisées

* **Langage :** C#
* **Interface Graphique :** WPF (Windows Presentation Foundation) / XAML
* **IDE :** Visual Studio
* **Gestion de version :** Git & GitHub

##  Comment lancer le projet ?

1. Clonez ce dépôt sur votre machine locale : 
   `git clone https://github.com/Ozan60000/SaeIHM.git`
2. Ouvrez le fichier `SaeIHM.sln` avec Visual Studio.
3. Dans l'Explorateur de solutions, assurez-vous que le projet **SAE-IHM** est défini comme projet de démarrage (clic droit ➔ *Définir comme projet de démarrage*).
4. Lancez la compilation et l'exécution (raccourci `F5` ou bouton "Démarrer").
