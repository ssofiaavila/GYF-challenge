# Sobre el proyecto

En este documento se indica el contenido y logica general del proyecto.
## GYF-challenge 
Contiene la aplicacion y la configuracion principal del proyecto junto con los Controllers a utilizar.
## Aplicacion
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
Gestion de productos con los siguientes endpoints, los cuales están todos protegidos con JTW por lo cual para accederlos será necesario previamente estar registrado y con una sesion abierta.

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
	Request: presupuesto - Presupuesto máximo para filtrar productos.
	Response: 200 OK con una lista de productos que se ajustan al presupuesto o 404 Not Found si no se encuentran suficientes productos.
## UsuarioController
Gestion de usuarios con los siguientes endpoints:

- **POST `/api/usuario/registro`:** registra un nuevo usuario en el sistema.
	```
	Request: UsuarioDTO - Datos del usuario a registrar.
	Response: 200 OK con un mensaje de éxito o 400 Bad Request si hay un error en la solicitud.
- **POST `/api/usuario/inicio-sesion`:** autentica a un usuario y devuelve un token JWT.
	```
	Request: LoginRequest - Nombre de usuario y contraseña.
	Response: 200 OK con el token JWT si la autenticación es exitosa o 401 Unauthorized si las credenciales son incorrectas.
# Modelos

A continuacion se describen los modelos utilizados en el proyecto:

### DTOs

- **`LoginRequest`**
  - **Descripcion**: Representa la solicitud de inicio de sesion de un usuario.
  - **Propiedades**:
    - `Username` (string): Nombre de usuario del usuario que intenta iniciar sesion.
    - `Password` (string): Password del usuario.

- **`ProductoRequest`**
  - **Descripcion**: Representa los datos necesarios para crear o actualizar un producto.
  - **Propiedades**:
    - `ExternalId` (string): Identificador externo del producto.
    - `Categoria` (CategoriaProducto): Categoria del producto, utilizando un enumerado.
    - `Precio` (double): Precio del producto.

- **`UsuarioDTO`**
  - **Descripcion**: Representa los datos del usuario para operaciones de registro.
  - **Propiedades**:
    - `ExternalId` (string): Identificador externo del usuario.
    - `Username` (string): Nombre de usuario del usuario.
    - `Password` (string): Password del usuario.
    - `Nombre` (string, opcional): Nombre del usuario.
    - `Apellido` (string, opcional): Apellido del usuario.

### Entidades

- **`Producto`**
  - **Descripcion**: Representa un producto en el sistema.
  - **Propiedades**:
    - `Id` (string): Identificador unico del producto.
    - `ExternalId` (string): Identificador externo del producto.
    - `Precio` (double): Precio del producto.
    - `FechaCarga` (string): Fecha en la que el producto fue cargado al sistema.
    - `Categoria` (CategoriaProducto): Categoria del producto, utilizando un enumerado.

- **`Usuario`**
  - **Descripcion**: Representa un usuario en el sistema.
  - **Propiedades**:
    - `Id` (int): Identificador unico del usuario.
    - `ExternalId` (string): Identificador externo del usuario.
    - `Username` (string): Nombre de usuario del usuario.
    - `Password` (string): Password del usuario.
    - `Nombre` (string, opcional): Nombre del usuario.
    - `Apellido` (string, opcional): Apellido del usuario.

### Enumeraciones

- **`CategoriaProducto`**
  - **Descripcion**: Enum que representa las posibles categorias de un producto.
  - **Valores**:
    - `ProductoUno`: Representa la primera categoria de producto.
    - `ProductoDos`: Representa la segunda categoria de producto.


## Aclaracion sobre el uso de `ExternalId`
`ExternalId` es un identificador que se utiliza para referirse a registros en el sistema con un valor que proviene de sistemas externos. Es diferente al identificador interno (`Id`), que es generado y utilizado unicamente dentro del sistema. Su principal obtetivo es la protección.

# Tests

### Test de `ProductoService`

- **`CrearProducto_DeberiaAgregarProducto`**: Verifica que un producto se agregue correctamente a la base de datos y que sus propiedades se asignen adecuadamente. Tambien asegura que el producto se guarda correctamente en la base de datos.

- **`ObtenerProductoPorExternalId_DeberiaRetornarProducto`**: Confirma que un producto pueda ser recuperado correctamente utilizando su `ExternalId`. 

- **`ObtenerProductoPorExternalId_DeberiaRetornarNull_ProductoNoExiste`**: Asegura que se retorne `null` cuando se busca un producto con un `ExternalId` que no existe en la base de datos.

- **`ActualizarProducto_DeberiaActualizarProducto`**: Verifica que un producto existente pueda ser actualizado correctamente y que los cambios se reflejen en la base de datos.

- **`EliminarProducto_DeberiaEliminarProducto`**: Asegura que un producto pueda ser eliminado de la base de datos y que no quede disponible para busquedas posteriores.

- **`ObtenerProductosPorPresupuesto_DeberiaRetornarProductosCorrectos`**: Verifica que se obtengan productos correctos que se ajusten a un presupuesto dado y que la suma de sus precios coincida con el presupuesto especificado.

- **`ObtenerProductosPorPresupuesto_DeberiaRetornarVacio_SinProductos`**: Confirma que se retorne una lista vacia cuando no hay productos que cumplan con el presupuesto dado.

- **`ObtenerProductosPorPresupuesto_DeberiaLanzarExcepcion_ParaPresupuestoInvalido`**: Asegura que se lance una excepcion cuando se pasa un presupuesto invalido (por ejemplo, cero).

### Test de `UsuarioService`

- **`CrearUsuario_DeberiaCrearUsuarioExitosamente`**: Verifica que un nuevo usuario sea creado correctamente y que sus credenciales se almacenen de manera segura.

- **`CrearUsuario_DeberiaRetornarMensajeUsuarioExistente`**: Confirma que se retorne un mensaje indicando que el usuario ya existe si se intenta crear un usuario con un nombre de usuario ya registrado.

- **`Authenticate_DeberiaRetornarTokenCuandoUsuarioEsValido`**: Asegura que se retorne un token de autenticacion valido cuando el usuario proporciona credenciales correctas.

- **`Authenticate_DeberiaRetornarNullCuandoUsuarioEsInvalido`**: Verifica que se retorne `null` cuando el usuario proporciona credenciales incorrectas.




