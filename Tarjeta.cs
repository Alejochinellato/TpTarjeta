using System;
using ManejoDeTiempos;

namespace TransporteUrbano
{
    public class Tarjeta
    {
        public const decimal LimiteSaldo = 36000;
        public const decimal LimiteNegativo = -480;
        public static decimal CostoPasaje = 1200;
        protected decimal saldo;
        protected decimal saldoPendiente;
        protected DateTime? ultimaFechaViaje;
        protected int viajesMesActual = 0;
        protected int viajesDiarios = 0;
        public string Id { get; }
        protected Tiempo tiempo;  // Dependencia de tiempo

        public Tarjeta(decimal saldoInicial, Tiempo tiempo)
        {
            saldo = saldoInicial <= LimiteSaldo ? saldoInicial : LimiteSaldo;
            saldoPendiente = saldoInicial > LimiteSaldo ? saldoInicial - LimiteSaldo : 0;
            Id = Guid.NewGuid().ToString();
            this.tiempo = tiempo;
        }

        public virtual bool DescontarPasaje(bool esInterurbano)
        {
            if (EsNuevoMes()) ReiniciarViajesMensuales();
            decimal costoViaje = CalcularCostoViaje(esInterurbano);

            if (saldo >= costoViaje || saldo - costoViaje >= LimiteNegativo)
            {
                saldo -= costoViaje;
                ultimaFechaViaje = tiempo.Now();
                viajesMesActual++;
                AcreditarSaldoPendiente();
                return true;
            }
            return false;
        }

        protected bool EsNuevoMes() => ultimaFechaViaje?.Month != tiempo.Now().Month;
        protected bool EsNuevoDia() => ultimaFechaViaje?.Date != tiempo.Now().Date;

        protected virtual void ReiniciarViajesMensuales() => viajesMesActual = 0;

        public decimal ObtenerSaldo() => saldo;

        public bool CargarSaldo(decimal monto)
        {
            decimal[] montosValidos = { 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000 };

            if (Array.IndexOf(montosValidos, monto) == -1) return false;

            decimal nuevoSaldo = saldo + monto;

            if (nuevoSaldo > LimiteSaldo)
            {
                saldoPendiente += nuevoSaldo - LimiteSaldo;
                saldo = LimiteSaldo;
            }
            else
            {
                saldo = nuevoSaldo;
            }

            return true;
        }

        private void AcreditarSaldoPendiente()
        {
            if (saldoPendiente > 0)
            {
                decimal espacioDisponible = LimiteSaldo - saldo;
                if (saldoPendiente <= espacioDisponible)
                {
                    saldo += saldoPendiente;
                    saldoPendiente = 0;
                }
                else
                {
                    saldo += espacioDisponible;
                    saldoPendiente -= espacioDisponible;
                }
            }
        }


        protected virtual void ReiniciarViajesDiarios() { }

        public decimal CalcularCostoViaje(bool esInterurbano)
        {
            decimal costoBase = esInterurbano ? 2500 : 1200;

            if (this is MedioBoleto)
            {
                decimal costo = viajesDiarios > 4 ? costoBase : costoBase / 2;

                return costo;
            }

            if (this is FranquiciaCompleta)
            {
                decimal costo = viajesDiarios > 2 ? costoBase : 0;

                return costo;
            }

            // Para otras tarjetas (tarjeta regular)
            if (viajesMesActual < 29) return costoBase;
            if (viajesMesActual < 79) return costoBase * 0.8m;
            if (viajesMesActual < 81) return costoBase * 0.75m;
            return costoBase;
        }




        public decimal SaldoPendiente => saldoPendiente;

        public int ViajesMesActual => viajesMesActual;
    }

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
                Console.WriteLine("La Franquicia Completa no puede usarse fuera del horario permitido.");
                return false;
            }

            if (ultimaFechaViaje.HasValue && (tiempo.Now() - ultimaFechaViaje.Value).TotalMinutes < 5)
            {
                Console.WriteLine("Debes esperar 5 minutos antes de realizar otro viaje con el MedioBoleto.");
                return false; // Deniega el pago si no pasaron 5 minutos
            }


            viajesDiarios++;

            decimal costoViaje = CalcularCostoViaje(esInterurbano);

            if (saldo >= costoViaje || saldo - costoViaje >= LimiteNegativo)
            {
                saldo -= costoViaje;
                ultimaFechaViaje = tiempo.Now();
                viajesMesActual++;
                return true; // Viaje pagado realizado
            }

            // Decrementar el contador de viajes si no se puede descontar el pasaje
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


    public class FranquiciaCompleta : Tarjeta
    {
        private int viajesGratuitosHoy = 0;
        private const int MaxViajesGratuitos = 2;

        public FranquiciaCompleta(decimal saldoInicial, Tiempo tiempo) : base(saldoInicial, tiempo) { }

        public override bool DescontarPasaje(bool esInterurbano)
        {
            if (EsNuevoMes()) ReiniciarViajesMensuales();
            if (EsNuevoDia()) ReiniciarViajesDiarios();

            if (!EstaEnHorarioValido())
            {
                Console.WriteLine("La Franquicia Completa no puede usarse fuera del horario permitido.");
                return false;
            }

            viajesDiarios++;

            decimal costoViaje = CalcularCostoViaje(esInterurbano);

            if (saldo >= costoViaje || saldo - costoViaje >= LimiteNegativo)
            {
                saldo -= costoViaje;
                ultimaFechaViaje = tiempo.Now(); // Uso de tiempo en lugar de DateTime.Now
                viajesMesActual++;
                return true;
            }

            // Decrementar el contador de viajes si no se puede descontar el pasaje
            viajesDiarios--;
            return false;
        }

        protected override void ReiniciarViajesMensuales()
        {
            base.ReiniciarViajesMensuales();
            viajesGratuitosHoy = 0;
        }

        protected override void ReiniciarViajesDiarios() => viajesGratuitosHoy = 0;

        private bool EstaEnHorarioValido()
        {
            var ahora = tiempo.Now(); // Uso de tiempo en lugar de DateTime.Now
            return ahora.DayOfWeek >= DayOfWeek.Monday && ahora.DayOfWeek <= DayOfWeek.Friday && ahora.Hour >= 6 && ahora.Hour <= 22;
        }

    }
}
