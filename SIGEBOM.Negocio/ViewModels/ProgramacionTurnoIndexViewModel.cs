using System;

namespace SIGEBOM.Negocio.ViewModels
{
    public class ProgramacionTurnoIndexViewModel
    {
        public int IdProgramacionTurno { get; set; }

        public DateOnly Fecha { get; set; }

        public string Dia { get; set; } = string.Empty;

        public string Turno { get; set; } = string.Empty;

        public string? Encargado { get; set; }

        public string Estado { get; set; } = string.Empty;
    }
}