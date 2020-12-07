Le nouveau système d'intéraction reprend en quelque sorte l'architecture MVC :

	- Les inputs provenants de l'utilisateur représentent la partie "model"
	- Les controllers sont spécifiques au type d'input utilisé, et commandent les vues
	- Les vues sont des scripts très génériques (rotation, changement de matériau...) qui régissent les objets de la scène 
	
	
Ce système fonctionne par le biais des interfaces suivantes :

	IBaseInteract : Interface de base pour chaque intéraction
		- bool DisableInteraction : défini si cette intéraction est utilisable ou non
	
Pour chaque type d'input, une interface qui défini toutes les actions possibles de l'intéraction

	IMouseInteract : Interface pour Clavier/Souris
	
		- void LeftClick : Comportement lors d'un clic gauche
		- void RightClick : Comportement lors d'un clic droit
		- void HoverEnter : Comportement à l'entrée du survol du curseur
		- void HoverStay : Comportement à chaque frame où le curseur reste sur l'objet
		- void HoverExit : Comportement à la sortie du curseur de l'objet