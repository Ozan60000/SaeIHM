"""
Module : joueur_artificiel.py
Projet : SAE 2.02 - Puissance 4 Livrable 3
Auteurs: Aytac Ozan, Bomble Quentin, Dubois Thomas, Petiton Nathan
Groupe : C-01 - IUT d'Amiens - 2025-2026

IA pour le jeu Puissance 4 basée sur l'algorithme Minimax avec élagage Alpha-Beta.
Optimisations : Livre d'ouverture, détection des coups immédiats, tri des coups,
table de transposition et heuristique (fenêtres + parité).
"""

# Constantes
SCORE_VICTOIRE = 1_000_000
VIDE = 0

# Table de transposition pour éviter de recalculer des grilles déjà évaluées
_cache = {}


def choisir_coup(grille, joueur, nb_a_aligner=4, profondeur=6):
    """Point d'entrée de l'IA : retourne l'index de la colonne optimale à jouer."""
    nb_colonnes = len(grille[0])
    adversaire = 2 if joueur == 1 else 1
    centre = nb_colonnes // 2

    # Copie de la grille pour simuler les coups en toute sécurité
    grille = [ligne[:] for ligne in grille]

    # 1. Ouverture : le centre est mathématiquement le meilleur 1er coup
    if _nb_pions(grille) < 2 and not _colonne_pleine(grille, centre):
        return centre

    # 2. Victoire immédiate
    coup = _coup_gagnant(grille, joueur, nb_a_aligner)
    if coup != -1:
        return coup

    # 3. Urgence: bloquer l'adversaire
    coup = _coup_gagnant(grille, adversaire, nb_a_aligner)
    if coup != -1:
        return coup

    # 4. Arbre de recherche Minimax Alpha-Beta
    _cache.clear()
    meilleur_score = float("-inf")
    meilleur_coup = -1
    alpha = float("-inf")
    beta = float("+inf")

    # Trier les colonnes (centre en premier) optimise les coupures Alpha-Beta
    for colonne in _colonnes_ordonnees(grille):
        ligne = _placer_pion(grille, colonne, joueur)
        if ligne == -1:
            continue
            
        score = _minimax(grille, profondeur - 1, alpha, beta, False,
                         joueur, adversaire, nb_a_aligner, profondeur)
        _retirer_pion(grille, colonne)

        if score > meilleur_score:
            meilleur_score = score
            meilleur_coup = colonne
        alpha = max(alpha, meilleur_score)

    # Plan B si rien n'a marché (ne devrait pas arriver sauf bug)
    if meilleur_coup == -1:
        for colonne in range(nb_colonnes):
            if not _colonne_pleine(grille, colonne):
                return colonne
                
    return meilleur_coup


def _minimax(grille, profondeur, alpha, beta, est_max, ia, adv, nb_a_aligner, prof_max):
    """Algorithme Minimax classique avec élagage Alpha-Beta et mémorisation."""
    
    # Cas terminaux
    if _verifier_victoire(grille, ia, nb_a_aligner):
        return SCORE_VICTOIRE - (prof_max - profondeur)  # Privilégie une victoire rapide
    if _verifier_victoire(grille, adv, nb_a_aligner):
        return -SCORE_VICTOIRE + (prof_max - profondeur)
    if _grille_pleine(grille) or profondeur == 0:
        return _heuristique(grille, ia, adv, nb_a_aligner)

    # Cache hit
    cle = (_signature(grille), profondeur, est_max)
    if cle in _cache:
        return _cache[cle]

    pion = ia if est_max else adv
    colonnes = _colonnes_ordonnees(grille)

    if est_max:
        meilleur = float("-inf")
        for c in colonnes:
            if _placer_pion(grille, c, pion) == -1:
                continue
            score = _minimax(grille, profondeur - 1, alpha, beta, False,
                             ia, adv, nb_a_aligner, prof_max)
            _retirer_pion(grille, c)
            meilleur = max(meilleur, score)
            alpha = max(alpha, meilleur)
            if beta <= alpha:
                break  # Alpha-coupure
    else:
        meilleur = float("+inf")
        for c in colonnes:
            if _placer_pion(grille, c, pion) == -1:
                continue
            score = _minimax(grille, profondeur - 1, alpha, beta, True,
                             ia, adv, nb_a_aligner, prof_max)
            _retirer_pion(grille, c)
            meilleur = min(meilleur, score)
            beta = min(beta, meilleur)
            if beta <= alpha:
                break  # Beta-coupure

    # Mise en cache avec limite de RAM
    if len(_cache) < 200_000:
        _cache[cle] = meilleur
        
    return meilleur


def _heuristique(grille, ia, adv, nb_a_aligner):
    """Évalue le plateau. Score positif = favorable à l'IA."""
    score = 0
    nb_lignes = len(grille)
    nb_colonnes = len(grille[0])
    centre = nb_colonnes // 2

    # Bonus de contrôle du centre
    for ligne in range(nb_lignes):
        if grille[ligne][centre] == ia:
            score += 4
        elif grille[ligne][centre] == adv:
            score -= 4

    # Scan des "fenêtres" (segments de nb_a_aligner cases)
    n = nb_a_aligner
    for ligne in range(nb_lignes):
        for col in range(nb_colonnes):
            for (dl, dc) in [(0, 1), (1, 0), (1, 1), (-1, 1)]:
                l_fin = ligne + (n - 1) * dl
                c_fin = col + (n - 1) * dc
                
                if 0 <= l_fin < nb_lignes and 0 <= c_fin < nb_colonnes:
                    fenetre = [grille[ligne + k * dl][col + k * dc] for k in range(n)]
                    
                    # Bonus de parité : une menace sur une ligne impaire est plus dure à contrer
                    bonus = 1.3 if (nb_lignes - 1 - ligne) % 2 == 1 else 1.0
                    score += _score_fenetre(fenetre, ia, adv, bonus)
                    
    return score


def _score_fenetre(fenetre, ia, adv, bonus=1.0):
    """Évalue une ligne de X cases. 0 si elle contient les deux couleurs (bloquée)."""
    nb_ia = fenetre.count(ia)
    nb_adv = fenetre.count(adv)
    nb_vide = fenetre.count(VIDE)

    if nb_ia > 0 and nb_adv > 0:
        return 0

    n = len(fenetre)
    
    # Menaces IA
    if nb_ia == n - 1 and nb_vide == 1:
        return int(80 * bonus)
    if nb_ia == n - 2 and nb_vide == 2:
        return 15
    if nb_ia == n - 3 and nb_vide == 3:
        return 3
        
    # Menaces adverses (pénalisées plus fort pour forcer la défense)
    if nb_adv == n - 1 and nb_vide == 1:
        return -int(120 * bonus)
    if nb_adv == n - 2 and nb_vide == 2:
        return -20
    if nb_adv == n - 3 and nb_vide == 3:
        return -3
        
    return 0


# Manipulation du plateau

def _placer_pion(grille, colonne, joueur):
    if colonne < 0 or colonne >= len(grille[0]):
        return -1
    for ligne in range(len(grille) - 1, -1, -1):
        if grille[ligne][colonne] == VIDE:
            grille[ligne][colonne] = joueur
            return ligne
    return -1


def _retirer_pion(grille, colonne):
    for ligne in range(len(grille)):
        if grille[ligne][colonne] != VIDE:
            grille[ligne][colonne] = VIDE
            return True
    return False


def _colonne_pleine(grille, colonne):
    return grille[0][colonne] != VIDE


def _grille_pleine(grille):
    return all(_colonne_pleine(grille, col) for col in range(len(grille[0])))


def _colonnes_jouables(grille):
    return [c for c in range(len(grille[0])) if not _colonne_pleine(grille, c)]


def _colonnes_ordonnees(grille):
    """Trie les colonnes de la plus centrale aux bords (optimise l'élagage)."""
    centre = len(grille[0]) // 2
    return sorted(_colonnes_jouables(grille), key=lambda c: abs(c - centre))


# Détection

def _verifier_victoire(grille, joueur, nb_a_aligner):
    """Check brut de victoire dans toutes les directions."""
    nb_lignes = len(grille)
    nb_colonnes = len(grille[0])
    n = nb_a_aligner
    directions = [(0, 1), (1, 0), (1, 1), (-1, 1)]

    for ligne in range(nb_lignes):
        for col in range(nb_colonnes):
            if grille[ligne][col] != joueur:
                continue
                
            for (dl, dc) in directions:
                l_fin = ligne + (n - 1) * dl
                c_fin = col + (n - 1) * dc
                
                if not (0 <= l_fin < nb_lignes and 0 <= c_fin < nb_colonnes):
                    continue
                    
                gagne = True
                for k in range(1, n):
                    if grille[ligne + k * dl][col + k * dc] != joueur:
                        gagne = False
                        break
                if gagne:
                    return True
    return False


def _coup_gagnant(grille, joueur, nb_a_aligner):
    """Retourne la colonne pour gagner en un coup, sinon -1."""
    for c in _colonnes_jouables(grille):
        if _placer_pion(grille, c, joueur) != -1:
            gagne = _verifier_victoire(grille, joueur, nb_a_aligner)
            _retirer_pion(grille, c)
            if gagne:
                return c
    return -1


def _nb_pions(grille):
    return sum(case != VIDE for ligne in grille for case in ligne)


def _signature(grille):
    """Convertit la grille en tuple hashable pour le dictionnaire de cache."""
    return tuple(tuple(ligne) for ligne in grille)


# Tests basiques

if __name__ == "__main__":
    # Test 1 : Ouverture
    grille_test = [[0 for _ in range(7)] for _ in range(6)]
    print(f"Test 1 (ouverture)       : col {choisir_coup(grille_test, 1)} (attendu 3)")

    # Test 2 : Victoire immédiate possible
    grille_test[5][1:4] = [1, 1, 1]
    print(f"Test 2 (gagne en 1)      : col {choisir_coup(grille_test, 1)} (attendu 0 ou 4)")

    # Test 3 : Blocage critique
    grille_test = [[0 for _ in range(7)] for _ in range(6)]
    grille_test[5][1:4] = [2, 2, 2]
    grille_test[5][0] = 1
    print(f"Test 3 (bloque l'adv)    : col {choisir_coup(grille_test, 1)} (attendu 4)")

    # Test 4 : Dimension custom
    grille_test = [[0 for _ in range(5)] for _ in range(5)]
    print(f"Test 4 (variante 5x5)    : col {choisir_coup(grille_test, 1, nb_a_aligner=4)} (attendu 2)")