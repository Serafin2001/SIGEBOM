using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SIGEBOM.Datos.Context;
using SIGEBOM.Datos.Models;
using SIGEBOM.Negocio.DTOs;
using SIGEBOM.Negocio.Interfaces;

namespace SIGEBOM.Negocio.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;

        private readonly PasswordHasher<Usuario> _passwordHasher;

        public UsuarioService(ApplicationDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        public async Task<List<Usuario>> ObtenerTodos(string? buscar)
        {
            var usuarios = _context.Usuarios
                .Include(u => u.Bombero)
                    .ThenInclude(b => b.Rango)
                .Include(u => u.Rol)
                .Where(u => u.Estado != "Inactivo")
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                buscar = buscar.Trim();

                usuarios = usuarios.Where(u =>

                    EF.Functions.Like(u.NombreUsuario, $"%{buscar}%") ||

                    EF.Functions.Like(u.Bombero.Nombre, $"%{buscar}%") ||

                    EF.Functions.Like(u.Bombero.Apellido, $"%{buscar}%") ||

                    EF.Functions.Like(u.Bombero.Cedula, $"%{buscar}%"));
            }

            return await usuarios
                .OrderBy(u => u.NombreUsuario)
                .ToListAsync();
        }

        public async Task<Usuario?> ObtenerPorId(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Bombero)
                    .ThenInclude(b => b.Rango)
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.IdUsuario == id);
        }

        public async Task<ResultadoOperacion> Crear(Usuario usuario)
        {
            usuario.NombreUsuario = usuario.NombreUsuario.Trim();

            if (await ExisteUsuario(usuario.NombreUsuario))
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "Ya existe un usuario con ese nombre."
                };
            }

            if (await ExisteBombero(usuario.IdBombero))
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "Ese bombero ya posee un usuario."
                };
            }

            usuario.Contraseña = _passwordHasher.HashPassword(
                usuario,
                usuario.Contraseña);

            usuario.Estado = "Activo";

            _context.Usuarios.Add(usuario);

            await _context.SaveChangesAsync();

            return new ResultadoOperacion
            {
                Exitoso = true,
                Mensaje = "Usuario registrado correctamente."
            };
        }

        public async Task<ResultadoOperacion> Actualizar(Usuario usuario)
        {
            var usuarioBD = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.IdUsuario == usuario.IdUsuario);

            if (usuarioBD == null)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "El usuario no existe."
                };
            }

            bool existe = await _context.Usuarios.AnyAsync(u =>
                u.NombreUsuario == usuario.NombreUsuario &&
                u.IdUsuario != usuario.IdUsuario);

            if (existe)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "Ya existe un usuario con ese nombre."
                };
            }

            // Actualizar únicamente los datos permitidos
            usuarioBD.NombreUsuario = usuario.NombreUsuario.Trim();
            usuarioBD.IdRol = usuario.IdRol;
            usuarioBD.IdBombero = usuario.IdBombero;

            // NO modificar la contraseña
            // usuarioBD.Contraseña permanece igual

            _context.Update(usuarioBD);

            await _context.SaveChangesAsync();

            return new ResultadoOperacion
            {
                Exitoso = true,
                Mensaje = "Usuario actualizado correctamente."
            };
        }

        public async Task<ResultadoOperacion> Desactivar(int id)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.IdUsuario == id);

            if (usuario == null)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "El usuario no existe."
                };
            }

            usuario.Estado = "Inactivo";

            usuario.MotivoEstado = "Usuario desactivado por el administrador.";

            _context.Update(usuario);

            await _context.SaveChangesAsync();

            return new ResultadoOperacion
            {
                Exitoso = true,
                Mensaje = "Usuario desactivado correctamente."
            };
        }

        public async Task<bool> ExisteUsuario(string nombreUsuario)
        {
            nombreUsuario = nombreUsuario.Trim();

            return await _context.Usuarios.AnyAsync(u =>
                u.NombreUsuario.ToLower() == nombreUsuario.ToLower());
        }

        public async Task<bool> ExisteBombero(int idBombero)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.IdBombero == idBombero);
        }

        public async Task<ResultadoOperacion> CambiarPassword(int idUsuario, string nuevaPassword)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);

            if (usuario == null)
            {
                return new ResultadoOperacion
                {
                    Exitoso = false,
                    Mensaje = "El usuario no existe."
                };
            }

            usuario.Contraseña = BCrypt.Net.BCrypt.HashPassword(nuevaPassword);

            _context.Update(usuario);

            await _context.SaveChangesAsync();

            return new ResultadoOperacion
            {
                Exitoso = true,
                Mensaje = "Contraseña actualizada correctamente."
            };
        }

      

        public async Task<Usuario?> Login(string nombreUsuario, string contraseña)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .Include(u => u.Bombero)
                .FirstOrDefaultAsync(u =>
                    u.NombreUsuario == nombreUsuario &&
                    u.Estado == "Activo");

            if (usuario == null)
            {
                return null;
            }

            bool passwordCorrecta = BCrypt.Net.BCrypt.Verify(
                contraseña,
                usuario.Contraseña);

            if (!passwordCorrecta)
            {
                return null;
            }

            return usuario;
        }
    }
}