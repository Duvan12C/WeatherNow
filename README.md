#!/bin/bash

# =========================================
# Proyecto API de Clima - ASP.NET Core 8 🌦️
# =========================================

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
- GET /weather/current?lat={latitud}&lon={longitud}
  Devuelve el clima actual para las coordenadas dadas.

- GET /weather/forecast?lat={latitud}&lon={longitud}
  Devuelve el pronóstico de los próximos 7 días.

Los resultados siempre vienen en un objeto estándar que contiene:
- status: Si la operación fue exitosa.
- message: Mensaje adicional o de error.
- data: El cuerpo de la respuesta (puede ser el clima actual o la lista de pronósticos).

# =========================================
# Cómo correr el proyecto
# =========================================
1. Clona el repositorio:
git clone <tu-repo>

2. Configura tu cadena de conexión a la base de datos en appsettings.json.

3. Configura tu API Key de Weatherbit.io también en appsettings.json.

4. Ejecuta las migraciones si es necesario:
update-database

5. Corre el proyecto:
dotnet run

# =========================================
# Notas importantes
# =========================================
- La información del clima también se guarda automáticamente en base de datos (tabla WeatherLogs).
- Se usa MemoryCache para no llamar a la API externa si los datos ya están almacenados por 10 minutos.
- La API ya viene con un sistema básico de logs con NLog (errores y actividad normal).
- Todos los controladores usan una respuesta base ApiResponse<T>, con Status, Message y Data.

# =========================================
# Nota de Disculpa 🙏🏻
# =========================================
Nunca antes había implementado manejo de archivos de texto o string largos en C#. Intenté hacerlo por el requerimiento solicitado, pero me topé con un error que no pude solucionar completamente.
Agradezco la oportunidad de intentarlo y mejorar. ¡Gracias por su comprensión!

