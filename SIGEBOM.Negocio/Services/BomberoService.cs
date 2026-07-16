using Microsoft.EntityFrameworkCore;
using SIGEBOM.Datos.Context;
using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.DTOs;
using SIGEBOM.Negocio.Interfaces;

namespace SIGEBOM.Negocio.Services
{
    public class BomberoService : IBomberoService
    {
        private readonly ApplicationDbContext _context;

        public BomberoService(ApplicationDbContext context)
        {
            _context = context;
        }


        // OBTENER TODOS


        public async Task<List<Bombero>> ObtenerTodos(string? buscar)
        {
            var bomberos = _context.Bomberos
                .Include(b => b.Rango)
                .Include(b => b.Cargo)
                .Where(b => b.Estado != "Inactivo")
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                buscar = buscar.Trim();

                bomberos = bomberos.Where(b =>

                    EF.Functions.Like(b.Cedula, $"%{buscar}%") ||

                    EF.Functions.Like(b.Nombre, $"%{buscar}%") ||

                    EF.Functions.Like(b.Apellido, $"%{buscar}%"));
            }

            return await bomberos
                .OrderBy(b => b.Apellido)
                .ThenBy(b => b.Nombre)
                .ToListAsync();
        }


        // BOMBEROS DISPONIBLES


        public async Task<List<Bombero>> ObtenerBomberosDisponibles(string? buscar)
        {
            var bomberos = _context.Bomberos
                .Include(b => b.Rango)
                .Include(b => b.Cargo)
                .Include(b => b.Usuario)
                .Where(b =>
                    b.Estado == "Activo" &&
                    b.Usuario == null)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                buscar = buscar.Trim();

                bomberos = bomberos.Where(b =>

                    EF.Functions.Like(b.Cedula, $"%{buscar}%") ||

                    EF.Functions.Like(b.Nombre, $"%{buscar}%") ||

                    EF.Functions.Like(b.Apellido, $"%{buscar}%"));
            }

            return await bomberos
                .OrderBy(b => b.Rango.OrdenJerarquico)
                .ThenBy(b => b.Apellido)
                .ThenBy(b => b.Nombre)
                .ToListAsync();
        }

 
        // OBTENER POR ID
     

        public async Task<Bombero?> ObtenerPorId(int id)
        {
            return await _context.Bomberos
                .Include(b => b.Rango)
                .Include(b => b.Cargo)
                .FirstOrDefaultAsync(b => b.IdBombero == id);
        }

        // CREAR


        public async Task<ResultadoOperacion> Crear(Bombero bombero)
        {
            bombero.Cedula = bombero.Cedula.Trim();

            if (await ExisteCedula(bombero.Cedula))
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "Ya existe un bombero con esa cédula."
                };
            }

            bombero.Estado = "Activo";

            _context.Bomberos.Add(bombero);

            await _context.SaveChangesAsync();

            return new ResultadoOperacion
            {
                Exitoso = true,
                Mensaje = "Bombero registrado correctamente."
            };
        }

       
        // ACTUALIZAR


        public async Task<ResultadoOperacion> Actualizar(Bombero bombero)
        {
            var bomberoBD = await _context.Bomberos
                .FirstOrDefaultAsync(b => b.IdBombero == bombero.IdBombero);

            if (bomberoBD == null)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "El bombero no existe."
                };
            }

            bool existe = await _context.Bomberos.AnyAsync(b =>
                b.Cedula == bombero.Cedula &&
                b.IdBombero != bombero.IdBombero);

            if (existe)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "Ya existe un bombero con esa cédula."
                };
            }

            bomberoBD.Cedula = bombero.Cedula.Trim();
            bomberoBD.Nombre = bombero.Nombre;
            bomberoBD.Apellido = bombero.Apellido;
            bomberoBD.Sexo = bombero.Sexo;
            bomberoBD.FechaNacimiento = bombero.FechaNacimiento;
            bomberoBD.Telefono = bombero.Telefono;
            bomberoBD.Direccion = bombero.Direccion;
            bomberoBD.TipoSangre = bombero.TipoSangre;
            bomberoBD.FechaIngreso = bombero.FechaIngreso;
            bomberoBD.Correo = bombero.Correo;
            bomberoBD.IdCargo = bombero.IdCargo;
            bomberoBD.IdRango = bombero.IdRango;

            _context.Update(bomberoBD);

            await _context.SaveChangesAsync();

            return new ResultadoOperacion
            {
                Exitoso = true,
                Mensaje = "Bombero actualizado correctamente."
            };
        }

      
        // DESACTIVAR


        public async Task<ResultadoOperacion> Desactivar(int id)
        {
            var bombero = await _context.Bomberos
                .FirstOrDefaultAsync(b => b.IdBombero == id);

            if (bombero == null)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "El bombero no existe."
                };
            }

            bombero.Estado = "Inactivo";

            _context.Update(bombero);

            await _context.SaveChangesAsync();

            return new ResultadoOperacion
            {
                Exitoso = true,
                Mensaje = "Bombero desactivado correctamente."
            };
        }

        // VALIDAR CÉDULA
   

        public async Task<bool> ExisteCedula(string cedula)
        {
            cedula = cedula.Trim();

            return await _context.Bomberos
                .AnyAsync(b => b.Cedula == cedula);
        }
    }
}