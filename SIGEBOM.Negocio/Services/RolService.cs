using Microsoft.EntityFrameworkCore;
using SIGEBOM.Datos.Context;
using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.DTOs;
using SIGEBOM.Negocio.Interfaces;

namespace SIGEBOM.Negocio.Services
{
    public class RolService : IRolService
    {
        private readonly ApplicationDbContext _context;

        public RolService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Rol>> ObtenerTodos(string? buscar)
        {
            var roles = _context.Roles
                .Where(r => r.Estado != "Inactivo")
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                buscar = buscar.Trim();

                roles = roles.Where(r =>
                    EF.Functions.Like(r.NombreRol, $"%{buscar}%"));
            }

            return await roles
                .OrderBy(r => r.NombreRol)
                .ToListAsync();
        }

        public async Task<Rol?> ObtenerPorId(int id)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.IdRol == id);
        }

        public async Task<ResultadoOperacion> Crear(Rol rol)
        {
            rol.NombreRol = rol.NombreRol.Trim();

            if (await ExisteNombre(rol.NombreRol))
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "Ya existe un rol con ese nombre."
                };
            }

            rol.Estado = "Activo";

            _context.Roles.Add(rol);

            await _context.SaveChangesAsync();

            return new ResultadoOperacion
            {
                Exitoso = true,
                Mensaje = "Rol registrado correctamente."
            };
        }

        public async Task<ResultadoOperacion> Actualizar(Rol rol)
        {
            var rolBD = await _context.Roles
                .FirstOrDefaultAsync(r => r.IdRol == rol.IdRol);

            if (rolBD == null)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "El rol no existe."
                };
            }

            bool existe = await _context.Roles.AnyAsync(r =>
                r.NombreRol.ToLower() == rol.NombreRol.ToLower()
                && r.IdRol != rol.IdRol);

            if (existe)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "Ya existe un rol con ese nombre."
                };
            }

            rolBD.NombreRol = rol.NombreRol.Trim();

            _context.Update(rolBD);

            await _context.SaveChangesAsync();

            return new ResultadoOperacion
            {
                Exitoso = true,
                Mensaje = "Rol actualizado correctamente."
            };
        }

        public async Task<ResultadoOperacion> Desactivar(int id)
        {
            var rol = await _context.Roles
                .FirstOrDefaultAsync(r => r.IdRol == id);

            if (rol == null)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "El rol no existe."
                };
            }

            rol.Estado = "Inactivo";

            _context.Update(rol);

            await _context.SaveChangesAsync();

            return new ResultadoOperacion
            {
                Exitoso = true,
                Mensaje = "Rol desactivado correctamente."
            };
        }

        public async Task<bool> ExisteNombre(string nombreRol)
        {
            nombreRol = nombreRol.Trim();

            return await _context.Roles.AnyAsync(r =>
                r.NombreRol.ToLower() == nombreRol.ToLower());
        }
    }
}