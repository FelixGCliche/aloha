# Aloha et les 5 artéfacts

*Ceci est un fork du dépôt [suivant](https://gitlab.com/Pensh238/projet_synthese_2020.git). Suivez ce lien si vous voulez voir l'historique complet du projet*

## Description
Petit plateformeur 2D créé avec Unity. 

## Contrôles

### Manette
* **Joystick gauche** Bouger de gauche à droite
* **Bouton du bas** Sauter
* **Bouton du haut** Interagir et activer les geysers
* **Bouton de gauche** Changer de température
* **Bouton Start** Pause 
* **Gâchettes** Grapplin
* **Bouton de droite** Geler les ennemis
* **Bumper de droite** Dasher
* **Bumper de gauche** Activer la glissade
* **Select** Changer de caméra

## Démarrage rapide

Ces instructions vous permettront d'obtenir une copie opérationnelle du projet sur votre machine à des fins de développement.

### Prérequis

* [Git](https://git-scm.com/downloads) - Système de contrôle de version. Utilisez la dernière version.
* [Rider](https://www.jetbrains.com/rider/) ou [Visual Studio](https://www.visualstudio.com/fr/) - IDE. Vous pouvez utiliser 
  également n'importe quel autre IDE: assurez-vous simplement qu'il supporte les projets Unity.
* [Unity 2020.1.15f1](https://unity3d.com/fr/get-unity/download/) - Moteur de jeu. Veuillez utiliser **spécifiquement cette 
  version.** Attention à ne pas installer Visual Studio une seconde fois si vous avez déjà un IDE.

**Attention!** Actuellement, seul le développement sur Windows est complètement supporté.

### Compiler une version de développement

Clonez le projet.

```
git clone https://gitlab.com/Pensh238/projet_synthese_2020.git
```

Ouvrez le projet dans Unity. Ensuite, ouvrez la scène `Main` et appuyez sur le bouton *Play*.

**Attention!** Il se peut que Harmony ait besoin d'être régénéré : Outils -> Harmony -> Générateur de code -> Générer.

### Tester un version stable ou de développement

Ouvrez le projet dans Unity. Ensuite, allez dans `File > Build Settings…` et compilez le projet **dans un dossier vide**.

Si vous rencontrez un bogue, vous êtes priés de le [signaler](https://gitlab.com/Pensh238/projet_synthese_2020/issues/new?issuable_template=Bug/issues/new?issuable_template=Bug).
Veuillez fournir une explication détaillée de votre problème avec les étapes pour reproduire le bogue. Les captures d'écran et 
les vidéos jointes sont les bienvenues.

## Contribuer au projet

Veuillez lire [CONTRIBUTING.md](CONTRIBUTING.md) pour plus de détails sur notre code de conduite.

## Auteurs
    
* **Félix Gagné Cliche** - *Programmeur - Artiste*
* **William Lemelin** - *Programmeur*
* **Félix Bergeron** - *Programmeur*
* **David Dorion** - *Programmeur*
* **Louis Robitaille-Dionne** - *Programmeur*

## Remerciements

* **Benjamin Lemelin**
  * Extensions sur le moteur Unity pour la recherche d'objets et de composants. Générateur de constantes. Gestionnaire de
    chargement des scènes.
