1.Tout d'abord, sur terminal cmd ou powershell, vérifier si vous avez dotnet installez
  Tapez la commande "dotnet-version"
  
Cas où vous n'avez pas encore dotnet installez sur votre machine
  Télécharger dotnet sur https://dotnet.microsoft.com/fr-fr/download
  Installer dotnet et vérifier que dotnet est bien installer en exécutant la commande "dotnet-version"
  
Cas où vous avez déjà dotnet installé
  1. télécharger le dépot, puis unziper le "ASP.NET-SALLE-ET-EMPLOI-DU-TEMPS-EMIT.zip" téléchargé
  2. aller dans le dossier "ASP.NET-SALLE-ET-EMPLOI-DU-TEMPS-EMIT" puis dans "GestionSalleEmit" puis encore dans "GestionSalleEmit
  3. Entrer dans le dossier en utilisant votre terminal cmd ou powershell
  4. Exécutez la commande "dotnet restore"
  5. installer "dotnet tool install --global dotnet-ef"
  6.  puis "dotnet ef database update"
  7.  lancer le projet avec "dotnet run" ou "dotnet watch run"
