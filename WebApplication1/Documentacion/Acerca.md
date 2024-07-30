# Sobre el proyecto

En este documento se indica el contenido y l�gica general del proyecto.
## GYF-challenge 
Contiene la aplicaci�n y la configuraci�n principal del proyecto junto con los Controllers a utilizar.
## Aplicaci�n
Biblioteca de clases la cual contiene
- **DTOs**
- **Servicios**

## Datos
Contiene el contexto de la base de datos y las migraciones, incluyendo
- `AppDbContext.cs`
- `Migrations` donde almacenara todas las migraciones generadas gracias a Entity Framework

## Dominio
Contiene las entidades del dominio y otros elementos relacionados
- Entidades
- Enums


# Controllers
El proyecto incluye dos controladores principales:

### ProductoController
Gestion de productos con los siguientes endpoints, los cuales est�n todos protegidos con JTW por lo cual para accederlos ser� necesario previamente estar registrado y con una sesi�n abierta.

- **POST `/api/producto/crear-producto`:** crea un nuevo producto.
	```
	Request: ProductoRequest - Datos del producto a crear.
	Response: 201 Created con el nuevo producto en el cuerpo de la respuesta.
- **GET `/api/producto/obtener-producto/{id}`:** obtiene un producto por su ID externo.
	```
		Request: id - ID del producto.
		Response: 200 OK con los detalles del producto o 404 Not Found si el producto no existe.
- **GET `/api/producto/obtener-todos`:** obtiene todos los productos.	
	```
	Response: 200 OK con una lista de productos.
- **PUT `/api/producto/actualizar-producto/{id}`:** actualiza los datos de un producto existente.		
	```
	Request: ProductoRequest - Datos actualizados del producto.
	Response: 200 OK con el producto actualizado o 404 Not Found si el producto no existe.
- **DELETE `/api/producto/eliminar-producto/{id}`:** elimina un producto por su ID externo.
	```
	Request: id - ID del producto a eliminar.
	Response: 204 No Content si el producto fue eliminado exitosamente o 404 Not Found si el producto no existe.
- **GET `/api/producto/filtrar-productos-por-presupuesto`:** filtra productos en base a un presupuesto dado.
	```
	Request: presupuesto - Presupuesto m�ximo para filtrar productos.
	Response: 200 OK con una lista de productos que se ajustan al presupuesto o 404 Not Found si no se encuentran suficientes productos.
## UsuarioController
Gestion de usuarios con los siguientes endpoints:

- **POST `/api/usuario/registro`:** registra un nuevo usuario en el sistema.
	```
	Request: UsuarioDTO - Datos del usuario a registrar.
	Response: 200 OK con un mensaje de �xito o 400 Bad Request si hay un error en la solicitud.
- **POST `/api/usuario/inicio-sesion`:** autentica a un usuario y devuelve un token JWT.
	```
	Request: LoginRequest - Nombre de usuario y contrase�a.
	Response: 200 OK con el token JWT si la autenticaci�n es exitosa o 401 Unauthorized si las credenciales son incorrectas.
# Modelos

A continuaci�n se describen los modelos utilizados en el proyecto:

### DTOs

- **`LoginRequest`**
  - **Descripci�n**: Representa la solicitud de inicio de sesi�n de un usuario.
  - **Propiedades**:
    - `Username` (string): Nombre de usuario del usuario que intenta iniciar sesi�n.
    - `Password` (string): Password del usuario.

- **`ProductoRequest`**
  - **Descripci�n**: Representa los datos necesarios para crear o actualizar un producto.
  - **Propiedades**:
    - `ExternalId` (string): Identificador externo del producto.
    - `Categoria` (CategoriaProducto): Categor�a del producto, utilizando un enumerado.
    - `Precio` (double): Precio del producto.

- **`UsuarioDTO`**
  - **Descripci�n**: Representa los datos del usuario para operaciones de registro.
  - **Propiedades**:
    - `ExternalId` (string): Identificador externo del usuario.
    - `Username` (string): Nombre de usuario del usuario.
    - `Password` (string): Password del usuario.
    - `Nombre` (string, opcional): Nombre del usuario.
    - `Apellido` (string, opcional): Apellido del usuario.

### Entidades

- **`Producto`**
  - **Descripci�n**: Representa un producto en el sistema.
  - **Propiedades**:
    - `Id` (string): Identificador �nico del producto.
    - `ExternalId` (string): Identificador externo del producto.
    - `Precio` (double): Precio del producto.
    - `FechaCarga` (string): Fecha en la que el producto fue cargado al sistema.
    - `Categoria` (CategoriaProducto): Categor�a del producto, utilizando un enumerado.

- **`Usuario`**
  - **Descripci�n**: Representa un usuario en el sistema.
  - **Propiedades**:
    - `Id` (int): Identificador �nico del usuario.
    - `ExternalId` (string): Identificador externo del usuario.
    - `Username` (string): Nombre de usuario del usuario.
    - `Password` (string): Password del usuario.
    - `Nombre` (string, opcional): Nombre del usuario.
    - `Apellido` (string, opcional): Apellido del usuario.

### Enumeraciones

- **`CategoriaProducto`**
  - **Descripci�n**: Enum que representa las posibles categor�as de un producto.
  - **Valores**:
    - `ProductoUno`: Representa la primera categor�a de producto.
    - `ProductoDos`: Representa la segunda categor�a de producto.


## Aclaracion sobre el uso de `ExternalId`
`ExternalId` es un identificador que se utiliza para referirse a registros en el sistema con un valor que proviene de sistemas externos. Es diferente al identificador interno (`Id`), que es generado y utilizado �nicamente dentro del sistema. Su principal objetivo es la protecci�n.

# Tests

### Test de `ProductoService`

- **`CrearProducto_DeberiaAgregarProducto`**: Verifica que un producto se agregue correctamente a la base de datos y que sus propiedades se asignen adecuadamente. Tambien asegura que el producto se guarda correctamente en la base de datos.

- **`ObtenerProductoPorExternalId_DeberiaRetornarProducto`**: Confirma que un producto pueda ser recuperado correctamente utilizando su `ExternalId`. 

- **`ObtenerProductoPorExternalId_DeberiaRetornarNull_ProductoNoExiste`**: Asegura que se retorne `null` cuando se busca un producto con un `ExternalId` que no existe en la base de datos.

- **`ActualizarProducto_DeberiaActualizarProducto`**: Verifica que un producto existente pueda ser actualizado correctamente y que los cambios se reflejen en la base de datos.

- **`EliminarProducto_DeberiaEliminarProducto`**: Asegura que un producto pueda ser eliminado de la base de datos y que no quede disponible para b�squedas posteriores.

- **`ObtenerProductosPorPresupuesto_DeberiaRetornarProductosCorrectos`**: Verifica que se obtengan productos correctos que se ajusten a un presupuesto dado y que la suma de sus precios coincida con el presupuesto especificado.

- **`ObtenerProductosPorPresupuesto_DeberiaRetornarVacio_SinProductos`**: Confirma que se retorne una lista vac�a cuando no hay productos que cumplan con el presupuesto dado.

- **`ObtenerProductosPorPresupuesto_DeberiaLanzarExcepcion_ParaPresupuestoInvalido`**: Asegura que se lance una excepci�n cuando se pasa un presupuesto invalido (por ejemplo, cero).

### Test de `UsuarioService`

- **`CrearUsuario_DeberiaCrearUsuarioExitosamente`**: Verifica que un nuevo usuario sea creado correctamente y que sus credenciales se almacenen de manera segura.

- **`CrearUsuario_DeberiaRetornarMensajeUsuarioExistente`**: Confirma que se retorne un mensaje indicando que el usuario ya existe si se intenta crear un usuario con un nombre de usuario ya registrado.

- **`Authenticate_DeberiaRetornarTokenCuandoUsuarioEsValido`**: Asegura que se retorne un token de autenticaci�n v�lido cuando el usuario proporciona credenciales correctas.

- **`Authenticate_DeberiaRetornarNullCuandoUsuarioEsInvalido`**: Verifica que se retorne `null` cuando el usuario proporciona credenciales incorrectas.




