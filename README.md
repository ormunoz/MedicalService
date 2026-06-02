# MedicalApi - Sistema de Gestión de Atenciones Médicas

Este es el backend para un sistema de gestión de atenciones médicas desarrollado en **C# (.NET 8)**. Utiliza **Dapper** como ORM ligero para realizar consultas y ejecutar procedimientos almacenados en **SQL Server**.

El sistema expone endpoints RESTful documentados mediante **Swagger UI** y protegidos con autenticación basada en **API Key** mediante el header `X-API-KEY`.

---

## Arquitectura del Proyecto

*   **Controllers/**: Capa de presentación que expone los endpoints HTTP.
*   **Services/**: Capa de lógica de negocio inyectable en el contenedor de DI de .NET.
*   **Interfaces/**: Abstracción de contratos para los servicios.
*   **Models/**: Modelos de dominio y estructuras de respuesta comunes (como `ApiResponse`).
*   **DTOs/**: Objetos de transferencia de datos utilizados para filtros y peticiones.
*   **Middleware/**: Capa de seguridad (`ApiKeyMiddleware`) para verificar las solicitudes.
*   **Helpers/**: Clases de utilidad para respuestas estandarizadas.

---

## Base de Datos (SQL Server)

El modelo de dominio está compuesto por las siguientes tablas:
*   `Paciente`
*   `Doctor`
*   `Especialidad`
*   `Atencion`

### Paso 1: Inicialización
Ejecuta el script unificado  para configurar toda tu base de datos (tablas, datos semilla y todos los stored procedures) de una sola vez:

*   **`database_setup.sql`**: Ejecuta este script entregado como parte del proyecto en tu servidor SQL Server.

---

## Configuración y Ejecución

### Paso 2: Configuración del proyecto
Edita el archivo [appsettings.json] para configurar tu cadena de conexión y la API Key de seguridad:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "TuCadenaDeConexionSQLServer"
  },
  "ApiKey": "[ENCRYPTION_KEY]"
}
```

### Paso 3: Restaurar y Levantar el Servidor
Desde la consola en el directorio de `MedicalApi`, ejecuta los siguientes comandos:

```bash
# Restaurar dependencias
dotnet restore

# Compilar el proyecto
dotnet build

# Ejecutar el backend
dotnet run
```

Una vez en ejecución, la API estará disponible en la URL indicada por la consola (generalmente `https://localhost:7196` o `http://localhost:5076`).

---

## Seguridad (Autenticación)

Todos los endpoints (con excepción de Swagger y OpenAPI) requieren autenticación por cabecera. Debes incluir el header `X-API-KEY` en cada solicitud HTTP:

*   **Header Name**: `X-API-KEY`
*   **Header Value**: El valor configurado en tu `appsettings.json`.

---

## Documentación de la API (Swagger UI)

Una vez que el proyecto esté corriendo en modo de desarrollo (`Development`), puedes ingresar a la interfaz gráfica de Swagger para probar interactivamente todos los endpoints:

*   **URL de Swagger**: `https://localhost:<puerto>/swagger` o `http://localhost:<puerto>/swagger`

Para autenticarte dentro de la interfaz de Swagger UI:
1.  Haz clic en el botón **Authorize** en la esquina superior derecha.
2.  Introduce tu API Key (por ejemplo, `123456789`).
3.  Haz clic en **Authorize** y luego cierra el modal.

---

## Pruebas y Validación

La API incluye las siguientes funcionalidades clave validadas en su lógica:
1.  **Evitar solapamientos de horario**: Al registrar una atención médica, el servicio valida que el doctor no tenga otra atención programada dentro del rango de fecha y hora provistos.
2.  **No repetir RUT**: Valida que no se dupliquen registros de pacientes bajo el mismo identificador de RUT.
3.  **Cálculo de Promedios**: Endpoint dedicado a calcular el tiempo promedio de atención por especialidad en minutos.
