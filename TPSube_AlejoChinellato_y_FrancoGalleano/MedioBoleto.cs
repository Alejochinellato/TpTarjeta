using System;
using ManejoDeTiempos;

namespace TransporteUrbano
{
    public class MedioBoleto : Tarjeta
    {
        public const int MaxViajesDiarios = 4;

        public MedioBoleto(decimal saldoInicial, Tiempo tiempo) : base(saldoInicial, tiempo) { }

        public override bool DescontarPasaje(bool esInterurbano)
        {
            if (EsNuevoMes()) ReiniciarViajesMensuales();
            if (EsNuevoDia()) ReiniciarViajesDiarios();

            if (!EstaEnHorarioValido())
            {
                Console.WriteLine("La Franquicia Estudiantil no puede usarse fuera del horario permitido.");
                return false;
            }

            if (ultimaFechaViaje.HasValue && (tiempo.Now() - ultimaFechaViaje.Value).TotalMinutes < 5)
            {
                Console.WriteLine("Debes esperar 5 minutos antes de realizar otro viaje con el MedioBoleto.");
                return false;
            }

            viajesDiarios++;

            decimal costoViaje = CalcularCostoViaje(esInterurbano);

            if (saldo >= costoViaje || saldo - costoViaje >= LimiteNegativo)
            {
                saldo -= costoViaje;
                ultimaFechaViaje = tiempo.Now();
                viajesMesActual++;
                return true;
            }

            viajesDiarios--;
            return false;
        }

        protected override void ReiniciarViajesDiarios()
        {
            viajesDiarios = 0;
        }

        private bool EstaEnHorarioValido()
        {
            var ahora = tiempo.Now();
            return ahora.DayOfWeek >= DayOfWeek.Monday && ahora.DayOfWeek <= DayOfWeek.Friday && ahora.Hour >= 6 && ahora.Hour <= 22;
        }
    }
}
