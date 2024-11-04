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

    public class MedioBoleto : Tarjeta
{
    private readonly decimal CostoMedioPasaje = CostoPasaje / 2;
    private int viajesHoy = 0;
    private DateTime? ultimaHoraDeViaje = null;
    private const int MaxViajesPorDia = 4;
    private static readonly TimeSpan IntervaloMinimo = TimeSpan.FromMinutes(5);

    public MedioBoleto(decimal saldoInicial) : base(saldoInicial)
    {
    }

    public override bool DescontarPasaje()
    {
    if (!EstaEnHorarioValido())
{
    Console.WriteLine("La Franquicia Completa no puede usarse fuera del horario permitido.");
    return false;
}
        if (EsNuevoDia())
        {
            ReiniciarViajesDiarios();
        }

        DateTime ahora = DateTime.Now;

        // Verificar si ha pasado el intervalo de 5 minutos
        if (ultimaHoraDeViaje.HasValue && (ahora - ultimaHoraDeViaje.Value) < IntervaloMinimo)
        {
            Console.WriteLine("No se puede realizar otro viaje aún. Debes esperar 5 minutos.");
            return false;
        }
        
    
        if (viajesHoy < MaxViajesPorDia)
        {
            if (saldo >= CostoMedioPasaje || saldo - CostoMedioPasaje >= LimiteNegativo)
            {
                saldo -= CostoMedioPasaje;
                return true;
            }
        }
        else
        {
          
            if (saldo >= CostoPasaje || saldo - CostoPasaje >= LimiteNegativo)
            {
                saldo -= CostoPasaje;
                viajesHoy++;
                ultimaHoraDeViaje = ahora;
                ultimaFechaViaje = ahora; // Actualizar fecha del último viaje
                return true;
            }
        }

        return false; // Saldo insuficiente
    }

    protected override void ReiniciarViajesDiarios()
    {
        viajesHoy = 0;
    }

    public int ViajesHoy
    {
        get { return viajesHoy; }
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
}
