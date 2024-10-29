using System.Linq;

namespace TransporteUrbano
{
    public class Tarjeta
    {
        public decimal saldo;
        public const decimal CostoPasaje = 940m;  
        public const decimal LimiteSaldo = 9900m;
        public const decimal LimiteNegativo = -480m;

        private static readonly decimal[] MontosAceptados = { 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000 };

        public Tarjeta(decimal saldoInicial)
        {
            saldo = saldoInicial;
        }

        public bool CargarSaldo(decimal monto)
        {
            if (!MontosAceptados.Contains(monto))
            {
                return false;
            }

            if (saldo + monto > LimiteSaldo)
            {
                saldo = LimiteSaldo;
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
                return true;
            }

            return false;
        }

        public decimal ObtenerSaldo()
        {
            return saldo;
        }
    }

    public class MedioBoleto : Tarjeta
    {
        private const decimal CostoMedioPasaje = CostoPasaje / 2;

        public MedioBoleto(decimal saldoInicial) : base(saldoInicial)
        {
        }

        public override bool DescontarPasaje()
        {
            if (saldo >= CostoMedioPasaje || saldo - CostoMedioPasaje >= LimiteNegativo)
            {
                saldo -= CostoMedioPasaje;
                return true;
            }

            return false;
        }
    }

       public FranquiciaCompleta(decimal saldoInicial) : base(saldoInicial)
    {
    }

    public override bool DescontarPasaje()
    {
        if (EsNuevoDia())
        {
            ReiniciarViajesDiarios();
        }

        if (viajesGratuitosHoy < MaxViajesGratuitos)
        {
            viajesGratuitosHoy++;
            ultimaFechaViaje = DateTime.Now; // Actualizar fecha del último viaje
            return true; // Viaje gratiiiis
        }
        else
        {
            
            return base.DescontarPasaje();
        }
    }

    protected override void ReiniciarViajesDiarios()
    {
        viajesGratuitosHoy = 0;
    }

    public int ViajesGratuitosHoy
    {
        get { return viajesGratuitosHoy; }
    }
}

}
