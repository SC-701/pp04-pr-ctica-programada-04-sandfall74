# Autorización. Middleware
ASP.NET Core 8

#Paso 1 - configurar el feed de GitHub packages
Crea un PAT en tu cuenta GitHub con scope `read:packages`, luego ejecuta:

```powershell
dotnet nuget add source https://nuget.pkg.github.com/SC-701/index.json `
  --name github `
  --username TU_USUARIO_GITHUB `
  --password TU_PERSONAL_ACCESS_TOKEN `
  --store-password-in-clear-text