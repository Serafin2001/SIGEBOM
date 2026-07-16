using Microsoft.EntityFrameworkCore;
using SIGEBOM.Datos.Context;
using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.DTOs;
using SIGEBOM.Negocio.Interfaces;

namespace SIGEBOM.Negocio.Services
{
    public class RangoService : IRangoService
    {
        private readonly ApplicationDbContext _context;

        public RangoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Rango>> ObtenerTodos(string? buscar)
        {
            var rangos = _context.Rangos
                .Where(r => r.Estado != "Inactivo")
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                buscar = buscar.Trim();

                rangos = rangos.Where(r =>
                    EF.Functions.Like(r.NombreRango, $"%{buscar}%"));
            }

            return await rangos
                .OrderBy(r => r.OrdenJerarquico)
                .ToListAsync();
        }

        public async Task<Rango?> ObtenerPorId(int id)
        {
            return await _context.Rangos
                .FirstOrDefaultAsync(r => r.IdRango == id);
        }

        public async Task<ResultadoOperacion> Crear(Rango rango)
        {
            rango.NombreRango = rango.NombreRango.Trim();

            if (await ExisteNombre(rango.NombreRango))
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "Ya existe un rango con ese nombre."
                };
            }

            rango.Estado = "Activo";

            _context.Rangos.Add(rango);

            await _context.SaveChangesAsync();

            return new ResultadoOperacion
            {
                Exitoso = true,
                Mensaje = "Rango registrado correctamente."
            };
        }

        public async Task<ResultadoOperacion> Actualizar(Rango rango)
        {
            var rangoBD = await _context.Rangos
                .FirstOrDefaultAsync(r => r.IdRango == rango.IdRango);

            if (rangoBD == null)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "El rango no existe."
                };
            }

            bool existe = await _context.Rangos.AnyAsync(r =>
                r.NombreRango.ToLower() == rango.NombreRango.ToLower()
                && r.IdRango != rango.IdRango);

            if (existe)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "Ya existe un rango con ese nombre."
                };
            }

            rangoBD.NombreRango = rango.NombreRango.Trim();
            rangoBD.Descripcion = rango.Descripcion;
            rangoBD.OrdenJerarquico = rango.OrdenJerarquico;

            _context.Update(rangoBD);

            await _context.SaveChangesAsync();

            return new ResultadoOperacion
            {
                Exitoso = true,
                Mensaje = "Rango actualizado correctamente."
            };
        }

        public async Task<ResultadoOperacion> Desactivar(int id)
        {
            var rango = await _context.Rangos
                .FirstOrDefaultAsync(r => r.IdRango == id);

            if (rango == null)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "El rango no existe."
                };
            }

            rango.Estado = "Inactivo";

            _context.Update(rango);

            await _context.SaveChangesAsync();

            return new ResultadoOperacion
            {
                Exitoso = true,
                Mensaje = "Rango desactivado correctamente."
            };
        }

        public async Task<bool> ExisteNombre(string nombreRango)
        {
            nombreRango = nombreRango.Trim();

            return await _context.Rangos.AnyAsync(r =>
                r.NombreRango.ToLower() == nombreRango.ToLower());
        }
    }
}