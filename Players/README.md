Ce package est réponsable de la gestion des 3C (Camera, Character, Controls).

Un PlayerController est une manière bien défini de jouer, faire l'interface entre l'utilisateur (Inputs) et le jeu.

Il est composé de :

	- Un script PlayerController avec les réglages suivants :
	    - IsDefaultPlayerInScene : défini si ce player est utilisé par défaut dans la scène, un seul player par scène doit avoir cette option
		- ClampRotationX : restreint la rotation de la camera sur l'axe X, avec l'option cochée, une valeur min/max
		- ClampRotationY : restreint la rotation du corps sur l'axe Y, avec l'option cochée, une valeur min/max
		- Gravity : la valeur en m/s de la gravité à appliquer (par défaut, gravité terrestre soit (0, -9.81, 0))
        - AllowThirdAxisMovement : défini si ce player peut se déplacer sur l'axe Y
        - ThirdAxisSpeedMultiplier : défini le multiplicateur de vitesse du troisième axe (si option précédente cochée)
		- LimitMovementX : restreint les déplacements sur l'axe X, avec l'option cochée, une valeur min/max en mètres
		- LimitMovementY : restreint les déplacements sur l'axe Y, avec l'option cochée, une valeur min/max en mètres
		- LimitMovementZ : restreint les déplacements sur l'axe Z, avec l'option cochée, une valeur min/max en mètres
		- MovementSpeedMultiplier : valeur de multiplication pour la vitesse de déplacement X/Z
		- RotationSpeedMultiplier : valeur de multiplication pour la vitesse de rotation
		- LockedCursor : bloque ou non le curseur au centre de l'écran (genre FPS) pour ce player
		- RaycastLength : Distance maximale en mètres pour intéragir
		- Deux listes d'event OnPlayerEnter/OnPlayerExit qui sont appelées lorsqu'on rentre/sort de ce player
		- LockMovement : Bloque les déplacements de ce player
		- LockRotation : Bloque la rotation de ce player
		- LockInteractions : Bloque les intéractions de ce player
		
	- Un CharacterController qui est utilisé pour gérer la physique
	
	- Une caméra virtuelle Cinemachine


Un player se déplace selon les axes LOCAUX suivants :

	X : positif à droite et négatif à gauche
	Y : positif en haut et négatif en bas
	Z : positif en avant et négatif en arrière

Pour changer le sens de déplacement, il suffit d'appliquer une rotation sur la caméra virtuelle du player.

Les collisions sont toujours activées par défaut (comportement du CharacterController). 
Si on veut que le player ignore les collisions, le mettre sur un layer créé pour et décocher les collisions avec les autres layers dans la matrice de collision (ProjectSetting/Physics).