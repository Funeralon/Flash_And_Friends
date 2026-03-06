# Flash & Friends

**Flash & Friends** est un jeu d'exploration et de photographie à la première personne développé sous Unity (URP). Incarnez le photographe officiel d'un festival de musique, capturez l'énergie de la foule, accomplissez des quêtes dynamiques et maximisez votre score de "Good Vibes" !

---

## Fonctionnalités Principales (Features)

Ce projet a été conçu pour mettre en pratique des concepts avancés de développement de jeux vidéo sur Unity :

### 📷 Gameplay & Mécaniques de Photographie
* **Contrôleur FPS personnalisé :** Déplacements fluides et gestion de la caméra à la souris.
* **Système de prise de vue (Raycast) :** Détection intelligente des cibles (obstruction, distance, centrage) pour valider les clichés.
* **Score "Good Vibes" :** Algorithme calculant la qualité de la photo en temps réel avec un retour visuel (Feedback UI).
* **Quêtes Data-Driven :** Utilisation de `ScriptableObjects` pour créer des missions modulables, affichées dans un journal de quêtes dynamique (Touche J).

### Sauvegarde & Gestion des Données
* **Exportation PNG :** Les photos prises en jeu sont générées et sauvegardées physiquement sur le disque dur avec leurs métadonnées.
* **Album Dynamique :** Interface chargeant dynamiquement les fichiers PNG depuis le dossier de sauvegarde pour afficher une galerie de miniatures interactives.

### Intelligence Artificielle & PNJ
* **Navigation Robuste :** Utilisation du `NavMesh` pour le déplacement autonome d'une foule diversifiée dans le festival.
* **Routines de Points d'Intérêt (POI) :** Les PNJ choisissent aléatoirement des destinations, s'y rendent, et jouent des animations de pause.
* **Animation Avancée :** Intégration de modèles et d'animations Mixamo via l'`Animator` d'Unity (Blend Trees, transitions fluides entre Idle, Walk et Poses).

### Réalisation & Esthétique (URP)
* **Cinématiques :** Utilisation de **Cinemachine** (Virtual Cameras, Camera Blending) et de la **Timeline** pour des séquences d'introduction et de fin fluides.
* **Level Design & Shaders :** Environnement rendu avec l'Universal Render Pipeline (URP), incluant des lumières dynamiques, du Post-Processing et des `Decal Projectors` pour les détails visuels (graffitis).
* **Interface Utilisateur (UI) :** Menus responsifs avec ancres (Anchors) et TextMeshPro.
* **Audio Dynamique :** Mixage audio (AudioMixer) avec gestion du volume (Master/Musique/SFX) via les paramètres, et musique persistante (`DontDestroyOnLoad`) entre les scènes.

---

## Stack Technique

* **Moteur de jeu :** Unity 6 (URP)
* **Langage :** C#
* **Outils Unity :** Cinemachine, Timeline, NavMesh, Input System, UI Canvas, TextMeshPro, AudioMixer.
* **Assets externes :** Modèles 3D et Animations via Mixamo.

---

## Installation & Lancement du projet (Source)

Si vous souhaitez ouvrir le projet dans l'éditeur Unity :

1. Clonez ce dépôt sur votre machine locale :
   ```bash
   git clone [https://github.com/VOTRE_PSEUDO/NOM_DU_DEPOT.git](https://github.com/VOTRE_PSEUDO/NOM_DU_DEPOT.git)
   ```

    Ouvrez Unity Hub et cliquez sur Add > Add project from disk.

    Sélectionnez le dossier cloné.

    Assurez-vous d'utiliser une version d'Unity compatible (Unity 6 / 6000.0.x).

    Dans le projet, ouvrez la scène Main_Menu située dans le dossier Assets/Scenes/ et appuyez sur Play.

## Architecture du Projet

Aperçu des scripts clés situés dans Assets/Scripts/ :

    PlayerController.cs : Gestion des inputs et mouvements du joueur.

    PhotoMechanic.cs : Cœur du gameplay, gère le Raycast et la sauvegarde des images.

    QuestManager.cs : Vérifie les conditions de victoire et gère le journal de quêtes.

    AlbumManager.cs : Charge les fichiers locaux pour générer la galerie photo en jeu.

    NPCBehavior.cs & NPC_Animator.cs : Logique de déplacement NavMesh et gestion de l'arbre d'animation des festivaliers.

## Crédits

Projet développé par Dumas Mathieu.
