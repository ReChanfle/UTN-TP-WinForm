Catálogo de Artículos - WinForms .NET Framework
📌 Descripción del proyecto

Esta aplicación de escritorio permite la gestión de artículos de un catálogo comercial, diseñada para ser genérica y reutilizable en distintos tipos de comercios.

La información administrada puede ser consumida posteriormente por otros sistemas como:

Webs
- E-commerce
- Apps móviles
- Catálogos digitales

Funcionalidades

La aplicación permite:

- Listar artículos
- Buscar artículos por distintos criterios
- Agregar artículos
- Modificar artículos
- Eliminar artículos
- Ver detalle de un artículo
- Entidades del sistema

Cada artículo contiene:

- Código
- Nombre
- Descripción
- Marca (selección desde lista)
- Categoría (selección desde lista)
- Precio
- Imágenes (uno o múltiples)

Además, el sistema permite administrar:

- Marcas
- Categorías

Arquitectura del proyecto

El proyecto está organizado en capas:

- Presentación (WinForms) → Forms
- Servicios → lógica de negocio
- Repositorios → acceso a datos
- Modelo → entidades (Articulo, Marca, Categoria, Imagen)
- Infraestructura → conexión a base de datos

Base de datos

La aplicación utiliza una base de datos SQL Server ya existente.

Tablas principales:
- ARTICULOS
- MARCAS
- CATEGORIAS
- IMAGENES

⚙️ Configuración de la base de datos

Configurar connection string:

<configuration>
  <connectionStrings>
    <add name="DefaultConnection"
         connectionString="Data Source=.;Initial Catalog=CATALOGO_DB;Integrated Security=True"
         providerName="System.Data.SqlClient" />
  </connectionStrings>
</configuration>
