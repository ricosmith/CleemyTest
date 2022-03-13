Bonjour,

Je vais essayer de faire une petite explication des choix et orientation qui ont été fait.
Je n'ai pas tout developpé au maximum possible car étant sur l'objectif du test certaines choses mériteraient un certain nombres d'améliorations et un travail plus approfondi.
Je détaillerai ces points en fin de document.

Concernant les technos j'avais déjà fait 2 api en net core sur mon temps personnel mais j'ai peu pratiqué cette techno.
C'était la première fois que je testais xUnit dans la mesure où les tests unitaires ne sont pas encore automatisés dans le cadre de mon travail actuel
(manque de temps pour la mise en place).
C'était pour finir également ma première fois avec l'entity framework, j'utilise actuellement plutot ado.net et les views qui sont je pense moins performantes mais
qui permettent une maitrise très poussée du moteur de donnée (sql server).
Je vais approfondir cette techno qui est vraiment beaucoup plus pratique à utiliser au quotidien.

Description :

Dossiers :
_ Data : Classes de l'entity Framework, Base de donnée
_ Controllers : Controlleurs de l'API
_ Models : Classes de traitements pour les controllers
_ Docs : Documentation


Base de donnée :
_ Les tables Devises et Natures sont plutôt créées en tant que tables propres afin de pouvoir être paramétrées dans l'espace admin du Front
_ Les tables Devises et Natures n'effectuent pas de modification ou suppresion en cascade car ce sont des paramètres et que par convention j'ai pour habitude de ne pas autoriser la suppression de paramètre
Si nécéssaire j'ajoute plutôt un attribut Actif afin de permettre la désactivation si nécéssaire.
_ Les tables Utilisateurs et Depenses suivent l'énoncé. 

Controlleurs :
j'ai utilisé le générateur pour créer les controllers puis ils ont été modifiés en fonction des besoins
_ les Controlleurs Devises et Natures n'ont pas de fonction delete afin d'éviter des problèmes de cascade avec les clés étrangères.
_ Le Controlleur Utilisateurs n'a rien de particulier, suit l'énoncé
_ Le Controlleur Dépenses n'a rien de particulier, suit l'énoncé

Models :
_ Le Model Utilisateur sert à l'insertion / Mise à jour des utilisateurs
_ Le Model Dépenses  sert à l'insertion / Mise à jour, il contient également la fonction de vérification des contraintes

Tests :
_ Les tests portent sur chaque opération de chaque controlleurs

Questions en suspens : 
_ Dans l'énoncé il est indiqué que l'API doit "Afficher toutes les propriétés de la dépense ; l'utilisateur de la dépense doit apparaitre sous la forme {FirstName} {LastName} (eg: "Anthony Stark")."
Sur ce point j'ai créé une fonction GetAffichDesc dans le controlleur qui renvoi un string mais l'idée était peut être plutôt d'avoir un affichage de l'utilisateur sur ce format dans les Get. 
Si tel est le cas, j'aurais créé une nouvelle classe UtilisateurAffichage en Model que j'aurais utilisée pour remonter les infos en retour de reponse.

Améliorations :
_ Meilleure organisation des dossiers : avec le temps j'ajouterai probablement plus de sous-dossiers pour faciliter les recherches et les accès aux classes dans l'architecture.
_ Doc technique complète : j'en ai redigé un début ici, mérite approfondissement 
_ Gestion des contraintes pour les tables de paramêtres
_ Securisation mise en place authentification sql : Pas de gestion d'utilisateur en sql 
_ Securisation mise en place authentification API : Pas d'authentfication ou autre système de sécurité
_ MultiLangue : l'API repond en français selon le besoin il peut être utile de la passer en multilingue