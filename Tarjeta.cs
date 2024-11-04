using System.Linq;

namespace TransporteUrbano
{
    public class Tarjeta
    {
        public decimal saldo;

        public decimal saldoPendiente; // Saldo pendiente de acreditación

        public const decimal CostoPasaje = 940m;  
        public const decimal LimiteSaldo = 6600m; // Nuevo límite de saldo
        public const decimal LimiteNegativo = -480m;

        private static readonly decimal[] MontosAceptados = { 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000 };

        public Tarjeta(decimal saldoInicial)
        {
            saldo = saldoInicial > LimiteSaldo ? LimiteSaldo : saldoInicial;
            saldoPendiente = saldoInicial > LimiteSaldo ? saldoInicial - LimiteSaldo : 0;
        }

        public bool CargarSaldo(decimal monto)
        {
            if (!MontosAceptados.Contains(monto))
            {
                return false;
            }

            decimal saldoTotal = saldo + saldoPendiente + monto;

            if (saldoTotal > LimiteSaldo)
            {
                saldo = LimiteSaldo;
                saldoPendiente = saldoTotal - LimiteSaldo;
            }
            else
            {
                saldo += monto;
            }

            return true;
        }

        public virtual bool DescontarPasaje()
        {
            if (saldo >= CostoPasaje || saldo - CostoPasaje >= LimiteNegativo)
            {
                saldo -= CostoPasaje;

                // Si hay saldo pendiente, acreditarlo en la tarjeta a medida que se gasta el saldo.
                if (saldoPendiente > 0)
                {
                    decimal montoAcreditar = LimiteSaldo - saldo;
                    if (montoAcreditar > saldoPendiente)
                    {
                        saldo += saldoPendiente;
                        saldoPendiente = 0;
                    }
                    else
                    {
                        saldo += montoAcreditar;
                        saldoPendiente -= montoAcreditar;
                    }
                }

                return true;
            }

            return false;
        }

        public decimal ObtenerSaldo()
        {
            return saldo;
        }

        public decimal ObtenerSaldoPendiente()
        {
            return saldoPendiente;
        }
    }

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
