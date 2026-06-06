# Análisis del Proyecto - API de Productos

## a. Decisiones sobre estructuras de datos y entidades

Se creó la entidad `Producto` con los siguientes campos:
- `Id`: llave primaria autogenerada
- `Nombre`: nombre del producto (texto)
- `Descripcion`: descripción del producto (texto)
- `Precio`: precio decimal
- `Stock`: cantidad disponible (entero)

Se utilizó Entity Framework Core con patrón Repository a través de `AppDbContext`.

## b. Comandos para generar migraciones

```bash
Add-Migration InitialCreate
Update-Database
```

## c. Comandos de Docker utilizados

```bash
docker run --name productos-db -e POSTGRES_PASSWORD=admin123 -e POSTGRES_USER=admin -e POSTGRES_DB=productosdb -p 5433:5432 -d postgres
docker ps
docker start productos-db
```

## d. Partes creadas manualmente y con IA

### Manualmente:
- Configuración del proyecto en Visual Studio
- Conexión de DBeaver a PostgreSQL
- Pruebas en Swagger
- crear la imagen en docker y ejecutar el contenedor

### Con asistencia de IA:
- Estructura del modelo Producto
- Configuración del AppDbContext
- Implementación del controlador con CRUD
- Configuración del Program.cs

### promts utilizados para generar código con IA:
"hazlo paso a paso bien estructurado, en la web api dice mysql pero usaremos postgress "

"ahora la serie III paso a paso please para configurar todo"

"ya va una parte Continuamos con el codigo"

"como instalo los paquetes NuGet