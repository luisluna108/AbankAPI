Prueba Tecnica Desarrollo ABANK 🚀

Pasos para ejecutar el proyecto

1- Dentro de la carpeta Base de Datos encontrara un archivo sql llamado Query Tablas SQL este se debe ejecutar para crear la tabla y registros.

2- Luego de descargar el proyecto de GIT debe de dirigirse a archivo appsettings.json y configurar DefaultConnection para lograr conectarse a su base de datos local 

EndPoints que se encuentra dentro del proyecto

1- Endpoint Login POST /api/V1/auth/login en donde debe ingresar con el siguientes credenciales. 

{
   "email": "prueba@gmail.com",
   "password": "123456"
}

Obtendra un token el cual debera copiar y pegar (si usa swagger) en la parte superior donde se encuentra un candado de esta forma poder acceder a su funcionalidad. Se debera colocar la palabra Bearer seguido del token generado. Por ejemplo 
Beaver $2a$11$cakTi3daBD4fOteOFYVveellrmv6EO7sya4ZrrpT/ca4ZbZiyuAxG

2- Endpoint Obtener usuarios GET /api/V1/users en donde solo se debe ejecutar para obtener los registros de los usuarios

3- Endpoint Guardar usuarios POST /api/V1/users en donde se agregan usuarios

4- Endpoint Obtener usuarios por ID GET /api/V1/users/{id} se obtiene los usuarios dependiendo del valor id colocado dicho registros se vera afectado

5- Endpoint Actualizar usuarios PUT /api/V1/users/{id} se actualiza los usuarios dependiendo del valor id colocado dicho registros se vera afectado

Tambien se agrega una carpeta llamada "Capturas de funcionalidad" para visualizar los endpoint en ejecucion en Swagger.

6- Endpoint Eliminar usuarios DELETE /api/V1/users/{id} se elimina los registros dependiendo del valor id colocado dicho registros se vera afectado
