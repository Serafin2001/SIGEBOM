using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGEBOM.Datos.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Capacitaciones",
                columns: table => new
                {
                    IdCapacitacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Institucion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Capacitaciones", x => x.IdCapacitacion);
                });

            migrationBuilder.CreateTable(
                name: "Cargos",
                columns: table => new
                {
                    IdCargo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCargo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Activo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cargos", x => x.IdCargo);
                });

            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    IdProveedor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedores", x => x.IdProveedor);
                });

            migrationBuilder.CreateTable(
                name: "Rangos",
                columns: table => new
                {
                    IdRango = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreRango = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    OrdenJerarquico = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Activo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rangos", x => x.IdRango);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreRol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Activo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "TiposIncidentes",
                columns: table => new
                {
                    IdTipoIncidente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Activo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposIncidentes", x => x.IdTipoIncidente);
                });

            migrationBuilder.CreateTable(
                name: "Turnos",
                columns: table => new
                {
                    IdTurno = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreTurno = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HoraEntrada = table.Column<TimeSpan>(type: "time", nullable: false),
                    HoraSalida = table.Column<TimeSpan>(type: "time", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Activo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turnos", x => x.IdTurno);
                });

            migrationBuilder.CreateTable(
                name: "Vehiculos",
                columns: table => new
                {
                    IdVehiculo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoVehiculo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Placa = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Modelo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Año = table.Column<int>(type: "int", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Activo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehiculos", x => x.IdVehiculo);
                });

            migrationBuilder.CreateTable(
                name: "Herramientas",
                columns: table => new
                {
                    IdHerramienta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    StockMinimo = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Modelo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FechaAdquisicion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdProveedor = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Herramientas", x => x.IdHerramienta);
                    table.ForeignKey(
                        name: "FK_Herramientas_Proveedores_IdProveedor",
                        column: x => x.IdProveedor,
                        principalTable: "Proveedores",
                        principalColumn: "IdProveedor",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bomberos",
                columns: table => new
                {
                    IdBombero = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cedula = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Sexo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TipoSangre = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Activo"),
                    MotivoEstado = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Correo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IdCargo = table.Column<int>(type: "int", nullable: true),
                    IdRango = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bomberos", x => x.IdBombero);
                    table.ForeignKey(
                        name: "FK_Bomberos_Cargos_IdCargo",
                        column: x => x.IdCargo,
                        principalTable: "Cargos",
                        principalColumn: "IdCargo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bomberos_Rangos_IdRango",
                        column: x => x.IdRango,
                        principalTable: "Rangos",
                        principalColumn: "IdRango",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mantenimientos",
                columns: table => new
                {
                    IdMantenimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoMantenimiento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Costo = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pendiente"),
                    IdVehiculo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mantenimientos", x => x.IdMantenimiento);
                    table.ForeignKey(
                        name: "FK_Mantenimientos_Vehiculos_IdVehiculo",
                        column: x => x.IdVehiculo,
                        principalTable: "Vehiculos",
                        principalColumn: "IdVehiculo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inventarios",
                columns: table => new
                {
                    IdInventario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdHerramienta = table.Column<int>(type: "int", nullable: false),
                    TipoMovimiento = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventarios", x => x.IdInventario);
                    table.ForeignKey(
                        name: "FK_Inventarios_Herramientas_IdHerramienta",
                        column: x => x.IdHerramienta,
                        principalTable: "Herramientas",
                        principalColumn: "IdHerramienta",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BomberosCapacitaciones",
                columns: table => new
                {
                    IdBomberoCapacitacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdBombero = table.Column<int>(type: "int", nullable: false),
                    IdCapacitacion = table.Column<int>(type: "int", nullable: false),
                    FechaRealizacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BomberosCapacitaciones", x => x.IdBomberoCapacitacion);
                    table.ForeignKey(
                        name: "FK_BomberosCapacitaciones_Bomberos_IdBombero",
                        column: x => x.IdBombero,
                        principalTable: "Bomberos",
                        principalColumn: "IdBombero",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BomberosCapacitaciones_Capacitaciones_IdCapacitacion",
                        column: x => x.IdCapacitacion,
                        principalTable: "Capacitaciones",
                        principalColumn: "IdCapacitacion",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InspeccionesVehiculos",
                columns: table => new
                {
                    IdInspeccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdVehiculo = table.Column<int>(type: "int", nullable: false),
                    IdBombero = table.Column<int>(type: "int", nullable: false),
                    FechaHoraInspeccion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EstadoGeneral = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Apto")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspeccionesVehiculos", x => x.IdInspeccion);
                    table.ForeignKey(
                        name: "FK_InspeccionesVehiculos_Bomberos_IdBombero",
                        column: x => x.IdBombero,
                        principalTable: "Bomberos",
                        principalColumn: "IdBombero",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspeccionesVehiculos_Vehiculos_IdVehiculo",
                        column: x => x.IdVehiculo,
                        principalTable: "Vehiculos",
                        principalColumn: "IdVehiculo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Contraseña = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Activo"),
                    MotivoEstado = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IdBombero = table.Column<int>(type: "int", nullable: false),
                    IdRol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Bomberos_IdBombero",
                        column: x => x.IdBombero,
                        principalTable: "Bomberos",
                        principalColumn: "IdBombero",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_IdRol",
                        column: x => x.IdRol,
                        principalTable: "Roles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetallesInspecciones",
                columns: table => new
                {
                    IdDetalleInspeccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdInspeccion = table.Column<int>(type: "int", nullable: false),
                    ElementoRevisado = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesInspecciones", x => x.IdDetalleInspeccion);
                    table.ForeignKey(
                        name: "FK_DetallesInspecciones_InspeccionesVehiculos_IdInspeccion",
                        column: x => x.IdInspeccion,
                        principalTable: "InspeccionesVehiculos",
                        principalColumn: "IdInspeccion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LlamadasEmergencia",
                columns: table => new
                {
                    IdLlamada = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreReportante = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdTipoIncidente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LlamadasEmergencia", x => x.IdLlamada);
                    table.ForeignKey(
                        name: "FK_LlamadasEmergencia_TiposIncidentes_IdTipoIncidente",
                        column: x => x.IdTipoIncidente,
                        principalTable: "TiposIncidentes",
                        principalColumn: "IdTipoIncidente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LlamadasEmergencia_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProgramacionTurnos",
                columns: table => new
                {
                    IdProgramacionTurno = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Programado"),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdTurno = table.Column<int>(type: "int", nullable: false),
                    IdEncargado = table.Column<int>(type: "int", nullable: false),
                    IdUsuarioCreador = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramacionTurnos", x => x.IdProgramacionTurno);
                    table.ForeignKey(
                        name: "FK_ProgramacionTurnos_Bomberos_IdEncargado",
                        column: x => x.IdEncargado,
                        principalTable: "Bomberos",
                        principalColumn: "IdBombero",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgramacionTurnos_Turnos_IdTurno",
                        column: x => x.IdTurno,
                        principalTable: "Turnos",
                        principalColumn: "IdTurno",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgramacionTurnos_Usuarios_IdUsuarioCreador",
                        column: x => x.IdUsuarioCreador,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Incidentes",
                columns: table => new
                {
                    IdIncidente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Direccion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FechaHoraIncidente = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraSalida = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HoraLlegada = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HoraFinalizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IdLlamada = table.Column<int>(type: "int", nullable: false),
                    IdTipoIncidente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidentes", x => x.IdIncidente);
                    table.ForeignKey(
                        name: "FK_Incidentes_LlamadasEmergencia_IdLlamada",
                        column: x => x.IdLlamada,
                        principalTable: "LlamadasEmergencia",
                        principalColumn: "IdLlamada",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incidentes_TiposIncidentes_IdTipoIncidente",
                        column: x => x.IdTipoIncidente,
                        principalTable: "TiposIncidentes",
                        principalColumn: "IdTipoIncidente",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetalleProgramacionTurnos",
                columns: table => new
                {
                    IdDetalleProgramacionTurno = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProgramacionTurno = table.Column<int>(type: "int", nullable: false),
                    IdBombero = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Asignado"),
                    Observacion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleProgramacionTurnos", x => x.IdDetalleProgramacionTurno);
                    table.ForeignKey(
                        name: "FK_DetalleProgramacionTurnos_Bomberos_IdBombero",
                        column: x => x.IdBombero,
                        principalTable: "Bomberos",
                        principalColumn: "IdBombero",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetalleProgramacionTurnos_ProgramacionTurnos_IdProgramacionTurno",
                        column: x => x.IdProgramacionTurno,
                        principalTable: "ProgramacionTurnos",
                        principalColumn: "IdProgramacionTurno",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BomberosIncidentes",
                columns: table => new
                {
                    IdBomberoIncidente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Funcion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdBombero = table.Column<int>(type: "int", nullable: false),
                    IdIncidente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BomberosIncidentes", x => x.IdBomberoIncidente);
                    table.ForeignKey(
                        name: "FK_BomberosIncidentes_Bomberos_IdBombero",
                        column: x => x.IdBombero,
                        principalTable: "Bomberos",
                        principalColumn: "IdBombero",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BomberosIncidentes_Incidentes_IdIncidente",
                        column: x => x.IdIncidente,
                        principalTable: "Incidentes",
                        principalColumn: "IdIncidente",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VehiculosIncidentes",
                columns: table => new
                {
                    IdVehiculoIncidente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Observaciones = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IdVehiculo = table.Column<int>(type: "int", nullable: false),
                    IdIncidente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehiculosIncidentes", x => x.IdVehiculoIncidente);
                    table.ForeignKey(
                        name: "FK_VehiculosIncidentes_Incidentes_IdIncidente",
                        column: x => x.IdIncidente,
                        principalTable: "Incidentes",
                        principalColumn: "IdIncidente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehiculosIncidentes_Vehiculos_IdVehiculo",
                        column: x => x.IdVehiculo,
                        principalTable: "Vehiculos",
                        principalColumn: "IdVehiculo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Asistencias",
                columns: table => new
                {
                    IdAsistencia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaHoraRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraSalida = table.Column<TimeSpan>(type: "time", nullable: true),
                    EstadoAsistencia = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Presente"),
                    Observacion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IdDetalleProgramacionTurno = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asistencias", x => x.IdAsistencia);
                    table.ForeignKey(
                        name: "FK_Asistencias_DetalleProgramacionTurnos_IdDetalleProgramacionTurno",
                        column: x => x.IdDetalleProgramacionTurno,
                        principalTable: "DetalleProgramacionTurnos",
                        principalColumn: "IdDetalleProgramacionTurno",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Asistencias_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_IdDetalleProgramacionTurno",
                table: "Asistencias",
                column: "IdDetalleProgramacionTurno",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_IdUsuario",
                table: "Asistencias",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Bomberos_Cedula",
                table: "Bomberos",
                column: "Cedula",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bomberos_Correo",
                table: "Bomberos",
                column: "Correo",
                unique: true,
                filter: "[Correo] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Bomberos_IdCargo",
                table: "Bomberos",
                column: "IdCargo");

            migrationBuilder.CreateIndex(
                name: "IX_Bomberos_IdRango",
                table: "Bomberos",
                column: "IdRango");

            migrationBuilder.CreateIndex(
                name: "IX_BomberosCapacitaciones_IdBombero_IdCapacitacion",
                table: "BomberosCapacitaciones",
                columns: new[] { "IdBombero", "IdCapacitacion" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BomberosCapacitaciones_IdCapacitacion",
                table: "BomberosCapacitaciones",
                column: "IdCapacitacion");

            migrationBuilder.CreateIndex(
                name: "IX_BomberosIncidentes_IdBombero_IdIncidente",
                table: "BomberosIncidentes",
                columns: new[] { "IdBombero", "IdIncidente" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BomberosIncidentes_IdIncidente",
                table: "BomberosIncidentes",
                column: "IdIncidente");

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_NombreCargo",
                table: "Cargos",
                column: "NombreCargo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DetalleProgramacionTurnos_IdBombero",
                table: "DetalleProgramacionTurnos",
                column: "IdBombero");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleProgramacionTurnos_IdProgramacionTurno_IdBombero",
                table: "DetalleProgramacionTurnos",
                columns: new[] { "IdProgramacionTurno", "IdBombero" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DetallesInspecciones_IdInspeccion",
                table: "DetallesInspecciones",
                column: "IdInspeccion");

            migrationBuilder.CreateIndex(
                name: "IX_Herramientas_IdProveedor",
                table: "Herramientas",
                column: "IdProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_Incidentes_IdLlamada",
                table: "Incidentes",
                column: "IdLlamada");

            migrationBuilder.CreateIndex(
                name: "IX_Incidentes_IdTipoIncidente",
                table: "Incidentes",
                column: "IdTipoIncidente");

            migrationBuilder.CreateIndex(
                name: "IX_InspeccionesVehiculos_IdBombero",
                table: "InspeccionesVehiculos",
                column: "IdBombero");

            migrationBuilder.CreateIndex(
                name: "IX_InspeccionesVehiculos_IdVehiculo",
                table: "InspeccionesVehiculos",
                column: "IdVehiculo");

            migrationBuilder.CreateIndex(
                name: "IX_Inventarios_IdHerramienta",
                table: "Inventarios",
                column: "IdHerramienta");

            migrationBuilder.CreateIndex(
                name: "IX_LlamadasEmergencia_IdTipoIncidente",
                table: "LlamadasEmergencia",
                column: "IdTipoIncidente");

            migrationBuilder.CreateIndex(
                name: "IX_LlamadasEmergencia_IdUsuario",
                table: "LlamadasEmergencia",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimientos_IdVehiculo",
                table: "Mantenimientos",
                column: "IdVehiculo");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramacionTurnos_Fecha_IdTurno",
                table: "ProgramacionTurnos",
                columns: new[] { "Fecha", "IdTurno" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProgramacionTurnos_IdEncargado",
                table: "ProgramacionTurnos",
                column: "IdEncargado");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramacionTurnos_IdTurno",
                table: "ProgramacionTurnos",
                column: "IdTurno");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramacionTurnos_IdUsuarioCreador",
                table: "ProgramacionTurnos",
                column: "IdUsuarioCreador");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_Nombre",
                table: "Proveedores",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rangos_NombreRango",
                table: "Rangos",
                column: "NombreRango",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_NombreRol",
                table: "Roles",
                column: "NombreRol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TiposIncidentes_Nombre",
                table: "TiposIncidentes",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_NombreTurno",
                table: "Turnos",
                column: "NombreTurno",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdBombero",
                table: "Usuarios",
                column: "IdBombero",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdRol",
                table: "Usuarios",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_NombreUsuario",
                table: "Usuarios",
                column: "NombreUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_Placa",
                table: "Vehiculos",
                column: "Placa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehiculosIncidentes_IdIncidente",
                table: "VehiculosIncidentes",
                column: "IdIncidente");

            migrationBuilder.CreateIndex(
                name: "IX_VehiculosIncidentes_IdVehiculo_IdIncidente",
                table: "VehiculosIncidentes",
                columns: new[] { "IdVehiculo", "IdIncidente" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Asistencias");

            migrationBuilder.DropTable(
                name: "BomberosCapacitaciones");

            migrationBuilder.DropTable(
                name: "BomberosIncidentes");

            migrationBuilder.DropTable(
                name: "DetallesInspecciones");

            migrationBuilder.DropTable(
                name: "Inventarios");

            migrationBuilder.DropTable(
                name: "Mantenimientos");

            migrationBuilder.DropTable(
                name: "VehiculosIncidentes");

            migrationBuilder.DropTable(
                name: "DetalleProgramacionTurnos");

            migrationBuilder.DropTable(
                name: "Capacitaciones");

            migrationBuilder.DropTable(
                name: "InspeccionesVehiculos");

            migrationBuilder.DropTable(
                name: "Herramientas");

            migrationBuilder.DropTable(
                name: "Incidentes");

            migrationBuilder.DropTable(
                name: "ProgramacionTurnos");

            migrationBuilder.DropTable(
                name: "Vehiculos");

            migrationBuilder.DropTable(
                name: "Proveedores");

            migrationBuilder.DropTable(
                name: "LlamadasEmergencia");

            migrationBuilder.DropTable(
                name: "Turnos");

            migrationBuilder.DropTable(
                name: "TiposIncidentes");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Bomberos");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Cargos");

            migrationBuilder.DropTable(
                name: "Rangos");
        }
    }
}
