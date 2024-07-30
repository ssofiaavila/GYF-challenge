# API Productos | Desafio GYF

Este documento proporciona instrucciones sobre como configurar y ejecutar el proyecto localmente.

## Requisitos previos

- [Visual Studio Community](https://visualstudio.microsoft.com/es/thank-you-downloading-visual-studio/?sku=Community&channel=Release&version=VS2022&source=VSLandingPage&cid=2030&passive=false)
- [.NET SDK 6](https://dotnet.microsoft.com/es-es/download/dotnet/6.0)
- [SQL Server Developer Edition](https://go.microsoft.com/fwlink/p/?linkid=2215158&clcid=0x2c0a&culture=es-ar&country=ar)
- [SQL Server Management Studio (SSMS)](https://aka.ms/ssmsfullsetup)

## Ejecucion del proyecto

Una vez clonado el repositorio e instalado lo anteriormente listado, se debe:

1. **Abrir la solucion en Visual Studio**

   La ruta en el repositorio es ``GYF-challenge/WebApplication1/GYF-challenge.sln``
2. **Restaurar los paquetes NuGet**

	Una vez abierta la solucion, abrir la Consola de administracion de paquetes, ubicada en `Herramientas > Administrador de paquetes NuGet> Consola de administracion de paquetes` y ejecutar el siguiente comando
	```bash
	dotnet restore
3. **Configurar la cadena de conexion**

	En el archivo `appsettings.json` debe indicarse el nombre del servidor de la base de datos.

		
		"ConnectionStrings": {
			"DefaultConnection": "Server={SERVER_NAME};Database=gyf_challenge;Trusted_Connection=True;TrustServerCertificate=True;"
		},


	Donde SERVER_NAME debera ser reemplazado por el nombre correcto. Ademas, en el archivo `AppDbContext.cs` tambien debera indicarse el nombre correcto del servidor
		
			private const string SERVER_NAME = "SERVER_NAME";


	Donde SERVER_NAME debera ser reemplazado por el nombre correcto igual que en `appsettings.json`.

4. **Crear base de datos**

	Acceder con SSMS al servidor que se usa y crear una nueva base de datos llamada `gyf_challenge`.
5. **Generar primera migracion**

	Abrir nuevamente la Consola de administracion de paquetes, donde el proyecto predeterminado debe ser `Datos` y ejecutar `.\DataBaseInitializer.ps1`, esto hace que se genere la primera migracion la cual genera las tablas. La populacion se generaran a la hora de ejecutar el proyecto, ya que cuenta con el archivo `InitializeDbObjects.cs` al cual genera datos en caso de que en primera instancia las tablas esten vacias.
6. **Ejecutar el proyecto**

	En la terminal indicar los comandos `dotnet build` para construir y seguido `dotnet run` para ejecutarlo.
7. **Acceso a los end-point con Swagger**

	Abrir cualquier navegador y dirigirse a la siguiente URL `http://localhost:5041/swagger`.

## Ejecucion de pruebas unitarias

Ubicarse en la ruta `GYF-challenge\ServicesTests` y ejecutar el siguiente comando en la terminal
```bash
dotnet test
