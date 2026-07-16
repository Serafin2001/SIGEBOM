using SIGEBOM.Datos.Models;
using Microsoft.EntityFrameworkCore;
namespace SIGEBOM.Datos.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tablas del sistema


        public DbSet<Asistencia> Asistencias { get; set; }

        public DbSet<Rol> Roles { get; set; }

        public DbSet<Cargo> Cargos { get; set; }

        public DbSet<Turno> Turnos { get; set; }

        public DbSet<Bombero> Bomberos { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<DetalleProgramacionTurno> DetalleProgramacionTurnos { get; set; }
        public DbSet<ProgramacionTurno> ProgramacionTurnos { get; set; }
        public DbSet<Rango> Rangos { get; set; }

        // Vehículos


        public DbSet<Vehiculo> Vehiculos { get; set; }

        public DbSet<Mantenimiento> Mantenimientos { get; set; }

        public DbSet<InspeccionVehiculo> InspeccionesVehiculos { get; set; }

        public DbSet<DetalleInspeccion> DetallesInspecciones { get; set; }



        // Emergencias e incidentes


        public DbSet<LlamadaEmergencia> LlamadasEmergencia { get; set; }

        public DbSet<Incidente> Incidentes { get; set; }

        public DbSet<TipoIncidente> TiposIncidentes { get; set; }


        // Relaciones muchos a muchos


        public DbSet<BomberoIncidente> BomberosIncidentes { get; set; }

        public DbSet<VehiculoIncidente> VehiculosIncidentes { get; set; }

        public DbSet<BomberoCapacitacion> BomberosCapacitaciones { get; set; }



        // Capacitación


        public DbSet<Capacitacion> Capacitaciones { get; set; }



        // Inventario

        public DbSet<Herramienta> Herramientas { get; set; }

        public DbSet<Inventario> Inventarios { get; set; }

        public DbSet<Proveedor> Proveedores { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

     
            // CONFIGURACIÓN DE ROL
      

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.IdRol);

                entity.Property(e => e.NombreRol)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.Estado)
                      .IsRequired()
                      .HasMaxLength(20)
                      .HasDefaultValue("Activo");

                entity.HasIndex(e => e.NombreRol)
                      .IsUnique();

                entity.HasMany(e => e.Usuarios)
                      .WithOne(u => u.Rol)
                      .HasForeignKey(u => u.IdRol)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            // CONFIGURACIÓN DE CARGO

            modelBuilder.Entity<Cargo>(entity =>
            {
                entity.HasKey(e => e.IdCargo);

                entity.Property(e => e.NombreCargo)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.Descripcion)
                      .HasMaxLength(255);

                entity.Property(e => e.Estado)
                      .IsRequired()
                      .HasMaxLength(20)
                      .HasDefaultValue("Activo");

                entity.HasIndex(e => e.NombreCargo)
                      .IsUnique();

                entity.HasMany(e => e.Bomberos)
                      .WithOne(b => b.Cargo)
                      .HasForeignKey(b => b.IdCargo)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            // CONFIGURACIÓN DE RANGO


            modelBuilder.Entity<Rango>(entity =>
            {
                entity.HasKey(e => e.IdRango);

                entity.Property(e => e.NombreRango)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.Descripcion)
                      .HasMaxLength(255);

                entity.Property(e => e.OrdenJerarquico)
                      .IsRequired();

                entity.Property(e => e.Estado)
                      .IsRequired()
                      .HasMaxLength(20)
                      .HasDefaultValue("Activo");

                entity.HasIndex(e => e.NombreRango)
                      .IsUnique();

                entity.HasMany(e => e.Bomberos)
                      .WithOne(b => b.Rango)
                      .HasForeignKey(b => b.IdRango)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            // CONFIGURACIÓN DE BOMBERO


            modelBuilder.Entity<Bombero>(entity =>
            {
                entity.HasKey(e => e.IdBombero);

                entity.Property(e => e.Cedula)
                      .IsRequired()
                      .HasMaxLength(13);

                entity.HasIndex(e => e.Cedula)
                      .IsUnique();

                entity.Property(e => e.Nombre)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.Apellido)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.Sexo)
                      .HasMaxLength(15);

                entity.Property(e => e.Telefono)
                      .IsRequired()
                      .HasMaxLength(15);

                entity.Property(e => e.Direccion)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(e => e.TipoSangre)
                      .IsRequired()
                      .HasMaxLength(5);

                entity.Property(e => e.Estado)
                      .IsRequired()
                      .HasMaxLength(20)
                      .HasDefaultValue("Activo");

                entity.Property(e => e.MotivoEstado)
                      .HasMaxLength(255);

                entity.Property(e => e.Correo)
                      .HasMaxLength(100);

                entity.HasIndex(e => e.Correo)
                      .IsUnique();

                // Cargo (opcional)
                entity.HasOne(e => e.Cargo)
                      .WithMany(c => c.Bomberos)
                      .HasForeignKey(e => e.IdCargo)
                      .OnDelete(DeleteBehavior.Restrict);

                // Rango (obligatorio)
                entity.HasOne(e => e.Rango)
                      .WithMany(r => r.Bomberos)
                      .HasForeignKey(e => e.IdRango)
                      .OnDelete(DeleteBehavior.Restrict);

                // Usuario
                entity.HasOne(e => e.Usuario)
                      .WithOne(u => u.Bombero)
                      .HasForeignKey<Usuario>(u => u.IdBombero)
                      .OnDelete(DeleteBehavior.Restrict);

                // Capacitaciones
                entity.HasMany(e => e.BomberosCapacitaciones)
                      .WithOne(bc => bc.Bombero)
                      .HasForeignKey(bc => bc.IdBombero)
                      .OnDelete(DeleteBehavior.Restrict);

                // Incidentes
                entity.HasMany(e => e.BomberosIncidentes)
                      .WithOne(bi => bi.Bombero)
                      .HasForeignKey(bi => bi.IdBombero)
                      .OnDelete(DeleteBehavior.Restrict);

                // Inspecciones
                entity.HasMany(e => e.InspeccionesVehiculos)
                      .WithOne(iv => iv.Bombero)
                      .HasForeignKey(iv => iv.IdBombero)
                      .OnDelete(DeleteBehavior.Restrict);

                // Programaciones donde es encargado
                entity.HasMany(e => e.ProgramacionesComoEncargado)
                      .WithOne(p => p.Encargado)
                      .HasForeignKey(p => p.IdEncargado)
                      .OnDelete(DeleteBehavior.Restrict);

                // Programaciones donde participa
                entity.HasMany(e => e.ProgramacionesAsignadas)
                      .WithOne(d => d.Bombero)
                      .HasForeignKey(d => d.IdBombero)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            // CONFIGURACIÓN DE TURNO


            modelBuilder.Entity<Turno>(entity =>
            {
                entity.HasKey(e => e.IdTurno);

                entity.Property(e => e.NombreTurno)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.HoraEntrada)
                      .IsRequired();

                entity.Property(e => e.HoraSalida)
                      .IsRequired();

                entity.Property(e => e.Descripcion)
                      .HasMaxLength(255);

                entity.Property(e => e.Estado)
                      .IsRequired()
                      .HasMaxLength(20)
                      .HasDefaultValue("Activo");

                // Evita turnos duplicados
                entity.HasIndex(e => e.NombreTurno)
                      .IsUnique();

                // Relación Turno -> ProgramacionTurno
                entity.HasMany(e => e.ProgramacionesTurno)
                      .WithOne(p => p.Turno)
                      .HasForeignKey(p => p.IdTurno)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            // CONFIGURACIÓN DE PROGRAMACION TURNO


            modelBuilder.Entity<ProgramacionTurno>(entity =>
            {
                entity.HasKey(e => e.IdProgramacionTurno);

                entity.Property(e => e.Fecha)
                      .IsRequired();

                entity.Property(e => e.Observacion)
                      .HasMaxLength(255);

                entity.Property(e => e.Estado)
                      .IsRequired()
                      .HasMaxLength(20)
                      .HasDefaultValue("Programado");

                entity.Property(e => e.FechaCreacion)
                      .IsRequired();

                // Evita programar dos veces el mismo turno en la misma fecha
                entity.HasIndex(e => new
                {
                    e.Fecha,
                    e.IdTurno
                })
                .IsUnique();

                entity.HasOne(e => e.Turno)
                      .WithMany(t => t.ProgramacionesTurno)
                      .HasForeignKey(e => e.IdTurno)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Encargado)
                      .WithMany(b => b.ProgramacionesComoEncargado)
                      .HasForeignKey(e => e.IdEncargado)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.UsuarioCreador)
                      .WithMany(u => u.ProgramacionesCreadas)
                      .HasForeignKey(e => e.IdUsuarioCreador)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.DetallesProgramacion)
                      .WithOne(d => d.ProgramacionTurno)
                      .HasForeignKey(d => d.IdProgramacionTurno)
                      .OnDelete(DeleteBehavior.Cascade);
            });

        
            // CONFIGURACIÓN DETALLE PROGRAMACION TURNO
     

            modelBuilder.Entity<DetalleProgramacionTurno>(entity =>
            {
                entity.HasKey(e => e.IdDetalleProgramacionTurno);

                entity.Property(e => e.Estado)
                      .IsRequired()
                      .HasMaxLength(20)
                      .HasDefaultValue("Asignado");

                entity.Property(e => e.Observacion)
                      .HasMaxLength(255);

                // Evita asignar dos veces el mismo bombero
                entity.HasIndex(e => new
                {
                    e.IdProgramacionTurno,
                    e.IdBombero
                })
                .IsUnique();

                entity.HasOne(e => e.ProgramacionTurno)
                      .WithMany(p => p.DetallesProgramacion)
                      .HasForeignKey(e => e.IdProgramacionTurno)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Bombero)
                      .WithMany(b => b.ProgramacionesAsignadas)
                      .HasForeignKey(e => e.IdBombero)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // CONFIGURACIÓN DE ASISTENCIA
         

            modelBuilder.Entity<Asistencia>(entity =>
            {
                entity.HasKey(e => e.IdAsistencia);

                entity.Property(e => e.FechaHoraRegistro)
                      .IsRequired();

                entity.Property(e => e.HoraSalida);

                entity.Property(e => e.EstadoAsistencia)
                      .IsRequired()
                      .HasMaxLength(20)
                      .HasDefaultValue("Presente");

                entity.Property(e => e.Observacion)
                      .HasMaxLength(255);

                entity.HasOne(e => e.DetalleProgramacionTurno)
                      .WithOne()
                      .HasForeignKey<Asistencia>(e => e.IdDetalleProgramacionTurno)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Usuario)
                      .WithMany()
                      .HasForeignKey(e => e.IdUsuario)
                      .OnDelete(DeleteBehavior.Restrict);
            });




            // CONFIGURACIÓN DE USUARIO


            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);

                entity.Property(e => e.NombreUsuario)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.Contraseña)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(e => e.Estado)
                      .IsRequired()
                      .HasMaxLength(20)
                      .HasDefaultValue("Activo");

                entity.Property(e => e.MotivoEstado)
                      .HasMaxLength(255);

                entity.HasIndex(e => e.NombreUsuario)
                      .IsUnique();

                // Usuario -> Bombero
                entity.HasOne(e => e.Bombero)
                      .WithOne(b => b.Usuario)
                      .HasForeignKey<Usuario>(e => e.IdBombero)
                      .OnDelete(DeleteBehavior.Restrict);

                // Usuario -> Rol
                entity.HasOne(e => e.Rol)
                      .WithMany(r => r.Usuarios)
                      .HasForeignKey(e => e.IdRol)
                      .OnDelete(DeleteBehavior.Restrict);

                // Usuario -> Llamadas
                entity.HasMany(e => e.LlamadasRegistradas)
                      .WithOne(l => l.Usuario)
                      .HasForeignKey(l => l.IdUsuario)
                      .OnDelete(DeleteBehavior.Restrict);

                // Usuario -> Programaciones
                entity.HasMany(e => e.ProgramacionesCreadas)
                      .WithOne(p => p.UsuarioCreador)
                      .HasForeignKey(p => p.IdUsuarioCreador)
                      .OnDelete(DeleteBehavior.Restrict);
            });



            // CONFIGURACIÓN DE TIPO INCIDENTE


            modelBuilder.Entity<TipoIncidente>(entity =>
            {
                entity.HasKey(e => e.IdTipoIncidente);

                entity.Property(e => e.Nombre)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Descripcion)
                      .HasMaxLength(255);

                entity.Property(e => e.Estado)
                      .IsRequired()
                      .HasMaxLength(20)
                      .HasDefaultValue("Activo");

                entity.HasIndex(e => e.Nombre)
                      .IsUnique();

                entity.HasMany(e => e.Llamadas)
                      .WithOne(l => l.TipoIncidente)
                      .HasForeignKey(l => l.IdTipoIncidente)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Incidentes)
                      .WithOne(i => i.TipoIncidente)
                      .HasForeignKey(i => i.IdTipoIncidente)
                      .OnDelete(DeleteBehavior.Restrict);
            });



            // CONFIGURACIÓN DE LLAMADA EMERGENCIA


            modelBuilder.Entity<LlamadaEmergencia>(entity =>
            {
                entity.HasKey(e => e.IdLlamada);

                entity.Property(e => e.NombreReportante)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Telefono)
                      .IsRequired()
                      .HasMaxLength(15);

                entity.Property(e => e.Direccion)
                      .HasMaxLength(150);

                entity.Property(e => e.FechaHora)
                      .IsRequired();

                entity.Property(e => e.Observacion)
                      .HasMaxLength(255);

                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.LlamadasRegistradas)
                      .HasForeignKey(e => e.IdUsuario)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.TipoIncidente)
                      .WithMany(t => t.Llamadas)
                      .HasForeignKey(e => e.IdTipoIncidente)
                      .OnDelete(DeleteBehavior.Restrict);
            });



            // CONFIGURACIÓN DE INCIDENTE


            modelBuilder.Entity<Incidente>(entity =>
            {
                entity.HasKey(e => e.IdIncidente);

                entity.Property(e => e.Direccion)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(e => e.FechaHoraIncidente)
                      .IsRequired();

                entity.Property(e => e.Descripcion)
                      .HasMaxLength(500);

                entity.HasOne(e => e.LlamadaEmergencia)
                      .WithMany()
                      .HasForeignKey(e => e.IdLlamada)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.TipoIncidente)
                      .WithMany(t => t.Incidentes)
                      .HasForeignKey(e => e.IdTipoIncidente)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.BomberosIncidentes)
                      .WithOne(bi => bi.Incidente)
                      .HasForeignKey(bi => bi.IdIncidente)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.VehiculosIncidentes)
                      .WithOne(vi => vi.Incidente)
                      .HasForeignKey(vi => vi.IdIncidente)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            // CONFIGURACIÓN DE VEHÍCULO


            modelBuilder.Entity<Vehiculo>(entity =>
            {
                entity.HasKey(e => e.IdVehiculo);

                entity.Property(e => e.TipoVehiculo)
                      .IsRequired()
                      .HasMaxLength(40);

                entity.Property(e => e.Placa)
                      .IsRequired()
                      .HasMaxLength(15);

                entity.Property(e => e.Marca)
                      .HasMaxLength(50);

                entity.Property(e => e.Modelo)
                      .HasMaxLength(50);

                entity.Property(e => e.Estado)
                      .IsRequired()
                      .HasMaxLength(20)
                      .HasDefaultValue("Activo");

                entity.HasIndex(e => e.Placa)
                      .IsUnique();

                entity.HasMany(e => e.Mantenimientos)
                      .WithOne(m => m.Vehiculo)
                      .HasForeignKey(m => m.IdVehiculo)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.InspeccionesVehiculo)
                      .WithOne(i => i.Vehiculo)
                      .HasForeignKey(i => i.IdVehiculo)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.VehiculosIncidentes)
                      .WithOne(v => v.Vehiculo)
                      .HasForeignKey(v => v.IdVehiculo)
                      .OnDelete(DeleteBehavior.Restrict);
            });


          
            // CONFIGURACIÓN DE MANTENIMIENTO
         

            modelBuilder.Entity<Mantenimiento>(entity =>
            {
                entity.HasKey(e => e.IdMantenimiento);

                entity.Property(e => e.TipoMantenimiento)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.Descripcion)
                      .HasMaxLength(500);

                entity.Property(e => e.Costo)
                      .HasColumnType("decimal(10,2)");

                entity.Property(e => e.Estado)
                      .HasMaxLength(20)
                      .HasDefaultValue("Pendiente");

                entity.HasOne(e => e.Vehiculo)
                      .WithMany(v => v.Mantenimientos)
                      .HasForeignKey(e => e.IdVehiculo)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            //====================================================
            // CONFIGURACIÓN DE INSPECCIÓN VEHÍCULO
            //====================================================

            modelBuilder.Entity<InspeccionVehiculo>(entity =>
            {
                entity.HasKey(e => e.IdInspeccion);

                entity.Property(e => e.Observaciones)
                      .HasMaxLength(255);

                entity.Property(e => e.EstadoGeneral)
                      .HasMaxLength(20)
                      .HasDefaultValue("Apto");

                entity.HasOne(e => e.Vehiculo)
                      .WithMany(v => v.InspeccionesVehiculo)
                      .HasForeignKey(e => e.IdVehiculo)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Bombero)
                      .WithMany(b => b.InspeccionesVehiculos)
                      .HasForeignKey(e => e.IdBombero)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.DetallesInspeccion)
                      .WithOne(d => d.InspeccionVehiculo)
                      .HasForeignKey(d => d.IdInspeccion)
                      .OnDelete(DeleteBehavior.Cascade);
            });


            //====================================================
            // CONFIGURACIÓN DETALLE INSPECCIÓN
            //====================================================

            modelBuilder.Entity<DetalleInspeccion>(entity =>
            {
                entity.HasKey(e => e.IdDetalleInspeccion);

                entity.Property(e => e.ElementoRevisado)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Estado)
                      .HasMaxLength(20);

                entity.Property(e => e.Observacion)
                      .HasMaxLength(255);
            });


            //====================================================
            // CONFIGURACIÓN PROVEEDOR
            //====================================================

            modelBuilder.Entity<Proveedor>(entity =>
            {
                entity.HasKey(e => e.IdProveedor);

                entity.Property(e => e.Nombre)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Telefono)
                      .HasMaxLength(15);

                entity.Property(e => e.Correo)
                      .HasMaxLength(100);

                entity.Property(e => e.Direccion)
                      .HasMaxLength(150);

                entity.HasIndex(e => e.Nombre)
                      .IsUnique();

                entity.HasMany(e => e.Herramientas)
                      .WithOne(h => h.Proveedor)
                      .HasForeignKey(h => h.IdProveedor)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            //====================================================
            // CONFIGURACIÓN HERRAMIENTA
            //====================================================

            modelBuilder.Entity<Herramienta>(entity =>
            {
                entity.HasKey(e => e.IdHerramienta);

                entity.Property(e => e.Nombre)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Descripcion)
                      .HasMaxLength(255);

                entity.Property(e => e.Estado)
                      .HasMaxLength(20);

                entity.Property(e => e.Marca)
                      .HasMaxLength(50);

                entity.Property(e => e.Modelo)
                      .HasMaxLength(50);

                entity.HasOne(e => e.Proveedor)
                      .WithMany(p => p.Herramientas)
                      .HasForeignKey(e => e.IdProveedor)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Inventarios)
                      .WithOne(i => i.Herramienta)
                      .HasForeignKey(i => i.IdHerramienta)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            //====================================================
            // CONFIGURACIÓN INVENTARIO
            //====================================================

            modelBuilder.Entity<Inventario>(entity =>
            {
                entity.HasKey(e => e.IdInventario);

                entity.Property(e => e.TipoMovimiento)
                      .HasMaxLength(30);

                entity.Property(e => e.Observacion)
                      .HasMaxLength(255);

                entity.HasOne(e => e.Herramienta)
                      .WithMany(h => h.Inventarios)
                      .HasForeignKey(e => e.IdHerramienta)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            //====================================================
            // CONFIGURACIÓN CAPACITACIÓN
            //====================================================

            modelBuilder.Entity<Capacitacion>(entity =>
            {
                entity.HasKey(e => e.IdCapacitacion);

                entity.Property(e => e.Nombre)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Descripcion)
                      .HasMaxLength(500);

                entity.Property(e => e.Institucion)
                      .HasMaxLength(100);

                entity.Property(e => e.Estado)
                      .HasMaxLength(20);

                entity.HasMany(e => e.BomberosCapacitaciones)
                      .WithOne(b => b.Capacitacion)
                      .HasForeignKey(b => b.IdCapacitacion)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            //====================================================
            // CONFIGURACIÓN BOMBERO CAPACITACIÓN
            //====================================================

            modelBuilder.Entity<BomberoCapacitacion>(entity =>
            {
                entity.HasKey(e => e.IdBomberoCapacitacion);

                entity.HasIndex(e => new
                {
                    e.IdBombero,
                    e.IdCapacitacion
                })
                .IsUnique();

                entity.HasOne(e => e.Bombero)
                      .WithMany(b => b.BomberosCapacitaciones)
                      .HasForeignKey(e => e.IdBombero)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Capacitacion)
                      .WithMany(c => c.BomberosCapacitaciones)
                      .HasForeignKey(e => e.IdCapacitacion)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            //====================================================
            // CONFIGURACIÓN BOMBERO INCIDENTE
            //====================================================

            modelBuilder.Entity<BomberoIncidente>(entity =>
            {
                entity.HasKey(e => e.IdBomberoIncidente);

                entity.Property(e => e.Funcion)
                      .HasMaxLength(50);

                entity.HasIndex(e => new
                {
                    e.IdBombero,
                    e.IdIncidente
                })
                .IsUnique();

                entity.HasOne(e => e.Bombero)
                      .WithMany(b => b.BomberosIncidentes)
                      .HasForeignKey(e => e.IdBombero)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Incidente)
                      .WithMany(i => i.BomberosIncidentes)
                      .HasForeignKey(e => e.IdIncidente)
                      .OnDelete(DeleteBehavior.Restrict);
            });


       
            // CONFIGURACIÓN VEHÍCULO INCIDENTE
           

            modelBuilder.Entity<VehiculoIncidente>(entity =>
            {
                entity.HasKey(e => e.IdVehiculoIncidente);

                entity.Property(e => e.Observaciones)
                      .HasMaxLength(255);

                entity.HasIndex(e => new
                {
                    e.IdVehiculo,
                    e.IdIncidente
                })
                .IsUnique();

                entity.HasOne(e => e.Vehiculo)
                      .WithMany(v => v.VehiculosIncidentes)
                      .HasForeignKey(e => e.IdVehiculo)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Incidente)
                      .WithMany(i => i.VehiculosIncidentes)
                      .HasForeignKey(e => e.IdIncidente)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

    }
}
