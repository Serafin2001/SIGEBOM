using Microsoft.EntityFrameworkCore;
using SIGEBOM.Datos.Context;
using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.DTOs;
using SIGEBOM.Negocio.Interfaces;

namespace SIGEBOM.Negocio.Services
{
    public class CargoService : ICargoService
    {
        private readonly ApplicationDbContext _context;

        public CargoService(ApplicationDbContext context)
        {
            _context = context;
        }

        //=========================================
        // OBTENER TODOS
        //=========================================

        public async Task<List<Cargo>> ObtenerTodos(string? buscar)
        {
            var cargos = _context.Cargos
                .Where(c => c.Estado != "Inactivo")
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                buscar = buscar.Trim();

                cargos = cargos.Where(c =>
                    EF.Functions.Like(c.NombreCargo, $"%{buscar}%"));
            }

            return await cargos
                .OrderBy(c => c.NombreCargo)
                .ToListAsync();
        }

        //=========================================
        // OBTENER POR ID
        //=========================================

        public async Task<Cargo?> ObtenerPorId(int id)
        {
            return await _context.Cargos
                .FirstOrDefaultAsync(c => c.IdCargo == id);
        }
        //=========================================
        // CREAR
        //=========================================

        public async Task<ResultadoOperacion> Crear(Cargo cargo)
        {
            cargo.NombreCargo = cargo.NombreCargo.Trim();

            if (await ExisteNombre(cargo.NombreCargo))
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "Ya existe un cargo con ese nombre."
                };
            }

            cargo.Estado = "Activo";

            _context.Cargos.Add(cargo);

            await _context.SaveChangesAsync();

            return new ResultadoOperacion
            {
                Exitoso = true,
                Mensaje = "Cargo registrado correctamente."
            };
        }

        //=========================================
        // ACTUALIZAR
        //=========================================

        public async Task<ResultadoOperacion> Actualizar(Cargo cargo)
        {
            var cargoBD = await _context.Cargos
                .FirstOrDefaultAsync(c => c.IdCargo == cargo.IdCargo);

            if (cargoBD == null)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "El cargo no existe."
                };
            }

            bool existe = await _context.Cargos.AnyAsync(c =>
                c.NombreCargo == cargo.NombreCargo &&
                c.IdCargo != cargo.IdCargo);

            if (existe)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "Ya existe un cargo con ese nombre."
                };
            }

            cargoBD.NombreCargo = cargo.NombreCargo.Trim();
            cargoBD.Descripcion = cargo.Descripcion;

            _context.Update(cargoBD);

            await _context.SaveChangesAsync();

            return new ResultadoOperacion
            {
                Exitoso = true,
                Mensaje = "Cargo actualizado correctamente."
            };
        }

        //=========================================
        // DESACTIVAR
        //=========================================

        public async Task<ResultadoOperacion> Desactivar(int id)
        {
            var cargo = await _context.Cargos
                .FirstOrDefaultAsync(c => c.IdCargo == id);

            if (cargo == null)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "El cargo no existe."
                };
            }

            cargo.Estado = "Inactivo";

            _context.Update(cargo);

            await _context.SaveChangesAsync();

            return new ResultadoOperacion
            {
                Exitoso = true,
                Mensaje = "Cargo desactivado correctamente."
            };
        }

        //=========================================
        // VALIDAR NOMBRE
        //=========================================

        public async Task<bool> ExisteNombre(string nombre)
        {
            nombre = nombre.Trim();

            return await _context.Cargos
                .AnyAsync(c => c.NombreCargo == nombre);
        }
    }
}