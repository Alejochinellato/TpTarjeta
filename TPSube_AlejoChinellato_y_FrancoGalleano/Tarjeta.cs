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

            if (this is FranquiciaEstudiantil)
            {
                decimal costo = viajesDiarios > 2 ? costoBase : 0;
                return costo;
            }

            if (this is FranquiciaJubilados)
            {
                return 0;
            }

            if (viajesMesActual < 29) return costoBase;
            if (viajesMesActual < 79) return costoBase * 0.8m;
            if (viajesMesActual < 81) return costoBase * 0.75m;
            return costoBase;
        }

        public decimal SaldoPendiente => saldoPendiente;
        public int ViajesMesActual => viajesMesActual;
    }
}
