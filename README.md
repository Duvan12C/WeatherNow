# Proyecto API de Clima - ASP.NET Core 8 🌦️

Este proyecto es una API construida con ASP.NET Core 8, que consulta información del clima utilizando la API externa de Weatherbit.io (https://www.weatherbit.io/).

# =========================================
# Tecnologías utilizadas
# =========================================
- ASP.NET Core 8
- Entity Framework Core
- AutoMapper
- NLog
- MemoryCache
- Newtonsoft.Json
- SQL Server (para persistencia de logs de clima)

# =========================================
# Arquitectura de la solución
# =========================================
La solución sigue una estructura limpia separada por capas:
- Domain: Contiene las entidades base.
- Application: Contiene los servicios, DTOs y contratos.
- Infrastructure: Implementa los servicios de Application y maneja el acceso a datos (DB y API externa).
- API: El proyecto principal que expone los endpoints HTTP.

Importante:
- Infrastructure solo depende de Domain.
- Application depende de Infrastructure.
- API depende de Application.

# =========================================
# Endpoints Disponibles
# =========================================
- GET /weather/current
  Devuelve el clima actual para las coordenadas dadas (latitud y longitud como parámetros).

- GET /weather/forecast
  Devuelve el pronóstico del clima para los próximos 7 días (latitud y longitud como parámetros).

Formato estándar de respuesta:
- status: Si la operación fue exitosa.
- message: Mensaje adicional o de error.
- data: Contenido de la respuesta (clima actual o pronóstico).

# =========================================
# Cómo correr el proyecto
# =========================================
1. Clonar el repositorio:
git clone <tu-repo>

2. Configurar la cadena de conexión a la base de datos en appsettings.json.

3. Configurar la API Key de Weatherbit.io también en appsettings.json.

4. Levantar el proyecto:
dotnet run

# =========================================
# Notas importantes
# =========================================
- La información del clima también se guarda automáticamente en base de datos (tabla WeatherLogs).
- Se usa MemoryCache para no llamar a la API externa si los datos ya fueron obtenidos en los últimos 10 minutos.
- La API incluye sistema de logging usando NLog para errores y actividad normal.
- Todos los controladores retornan un objeto ApiResponse<T> con los campos Status, Message y Data.

# =========================================
# Nota de Disculpa 🙏🏻
# =========================================
Nunca antes había implementado pruebas automatizadas (testing) en C#. Intenté aplicarlas en el proyecto como fue solicitado, pero me encontré con dificultades que no logré resolver completamente.  
Agradezco la oportunidad de intentarlo y seguir mejorando en esta área. ¡Gracias por su comprensión!
