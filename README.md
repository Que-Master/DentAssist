───────────────────────────────────────────────

# DentAssist - Sistema de Gestión Odontológica
**DentAssist es una aplicación web desarrollada en ASP.NET Core MVC para facilitar la gestión clínica de centros odontológicos. Permite a administradores, odontólogos y recepcionistas gestionar pacientes, turnos, tratamientos y planes de tratamiento de manera eficiente.**

───────────────────────────────────────────────
# 🧰 Tecnologías utilizadas

ASP.NET Core MVC

#Entity Framework Core

#SQL Server 2022

#SQL Server Management Studio (SSMS)

Bootstrap 5

Razor Pages

C#

───────────────────────────────────────────────

# 🚀 Instalación y ejecución

Requisitos:

Visual Studio 2022 o superior

.NET 6 SDK

SQL Server 2022

SQL Server Management Studio (SSMS)

Pasos:

Clona el repositorio o descarga el proyecto:

git clone https://github.com/tuusuario/dentassist.git

Abre el proyecto con Visual Studio.

Configura la cadena de conexión en el archivo appsettings.json:

───────────────────────────────────────────────────────────────────────────────

"ConnectionStrings": {
"DefaultConnection": "Server=NOMBRE_DEL_SERVIDOR;Database=DentAssistDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}

───────────────────────────────────────────────────────────────────────────────

**Reemplaza NOMBRE_DEL_SERVIDOR por el nombre de tu servidor SQL. Puedes encontrarlo al abrir SSMS en el campo “Servidor”.**

Ejecuta las migraciones para crear la base de datos:

Desde la consola (CLI):

dotnet ef database update

O desde la Consola del Administrador de Paquetes en Visual Studio:

Update-Database

Ejecuta el proyecto desde Visual Studio (F5 o Ctrl+F5).

───────────────────────────────────────────────

# 🔐 Datos iniciales

El sistema incluye un usuario administrador por defecto para facilitar la puesta en marcha.

Credenciales iniciales:

Email: admin@dentassist.cl

Contraseña: admin123

Este administrador puede crear odontólogos y recepcionistas desde el panel de administrador.

───────────────────────────────────────────────

# 🧭 Uso

Accede a la URL local (ejemplo: https://localhost:5001/).

Selecciona el tipo de usuario: Administrador, Odontólogo o Recepcionista.

Inicia sesión.

⚠️ En el primer uso, debes iniciar sesión como Administrador con las credenciales por defecto para poder crear los usuarios Odontólogo y Recepcionista. Estos usuarios no existen hasta que los registre el Administrador.

Utiliza el menú según tu rol para acceder a las funcionalidades disponibles.

───────────────────────────────────────────────
# 📋 Funcionalidades por rol

**👨‍💼 Administrador:**

Gestión de odontólogos

Gestión de recepcionistas

Gestión de pacientes

Gestión de tratamientos

Gestión de turnos

**🦷 Odontólogo:**

Consulta de turnos asignados

Creación de planes de tratamiento personalizados por paciente

Visualización de historial clínico

Avance de tratamientos por pasos

**📋 Recepcionista:**

Administración de turnos

Registro y edición de pacientes

Visualización de turnos próximos

───────────────────────────────────────────────

**📫 Contacto**

Si tienes dudas o sugerencias, puedes escribirnos a:
contacto@dentassist.cl

───────────────────────────────────────────────
