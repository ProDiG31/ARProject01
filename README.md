Furious Bunny - Projet AR dévéloppé dans le cadre du cours époyme (Ynov - Toulouse 2019/2020)

UNITY Version : 2019.3

Les technologies et librairies utilisées : 
  - ARFoundation
  - NavMeshComponent (https://github.com/Unity-Technologies/NavMeshComponents)
  - NavMeshLinks Generator (https://forum.unity.com/threads/navmesh-links-generator-for-navmeshcomponents.515143/)
  

Configuration Unity. 

	Le projet utilisé le nouveau Renderer Pipeline URP (Universal Renderer Pipeline) disponible uniquement depuis la version 2019.3 d' Unity. 
  L'utilisation de L'URP permet l'utilisation de ShaderGraph. 
  Une configuration specifique de l'URP a du etre fonctionné dans un environement en RA. 

La structure du projet se répsente de la manière suivante : 
  - _Project
    - Images / Materials / Sound ... : Ressources diverses 
    - Modele : L'ensemble des objets et des ressources utilisés pour la construction des niveaux 
      - AR
      - Bunny
      - LevelDesign 
      - ...
    - Scripts : L'ensemble des scripts utilisés pour induire le comportement de nos ressources
    
Le travail s'est répartie de la manière suivante : 

  Romain Ducros :
    - Réalisation des modèles 3D sous Blender
    - Instantiation des niveaux et de la mécanique de jeu
    - Interactions entre les inputs sur le téléphone et le monde RA
 
  Jason Liebault :
    - Réalisation de l'interface du menu de démarrage
    - Travail sur la création des niveaux de type TimeAttack : IA du lapin, Path finding et NavMesh (résultats non probants)
    - Détection des plans
