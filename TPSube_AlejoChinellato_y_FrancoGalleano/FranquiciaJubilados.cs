using System;
using ManejoDeTiempos;

namespace TransporteUrbano
{
    public class FranquiciaJubilados : Tarjeta
    {
        public FranquiciaJubilados(decimal saldoInicial, Tiempo tiempo) : base(saldoInicial, tiempo) { }

        public override bool DescontarPasaje(bool esInterurbano)
        {
            if (EsNuevoMes()) ReiniciarViajesMensuales();
            if (EsNuevoDia()) ReiniciarViajesDiarios();

            if (!EstaEnHorarioValido())
            {
                Console.WriteLine("La Franquicia jubilados no puede usarse fuera del horario permitido.");
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

        private bool EstaEnHorarioValido()
        {
            var ahora = tiempo.Now();
            return ahora.DayOfWeek >= DayOfWeek.Monday && ahora.DayOfWeek <= DayOfWeek.Friday && ahora.Hour >= 6 && ahora.Hour <= 22;
        }
    }
}
