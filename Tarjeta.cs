namespace TransporteUrbano
{
    public class Tarjeta
    {


        private decimal saldo;
        private const decimal LimiteSaldo = 9900m;
        private const decimal CostoPasaje = 940m;
        private const decimal LimiteNegativo = -480m; 


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

        public bool DescontarPasaje()
        {
            if (saldo >= CostoPasaje || saldo - CostoPasaje >= LimiteNegativo)
            {
                saldo -= CostoPasaje;
              
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
            if (saldo >= CostoMedioPasaje)
            {
                saldo -= CostoMedioPasaje;


                return true;
            }

            return false;
        }
    }

    public class FranquiciaCompleta : Tarjeta
    {
        public FranquiciaCompleta(decimal saldoInicial) : base(saldoInicial)
        {
        }

        public override bool DescontarPasaje()
        {
            // La franquicia completa no necesita saldo
            return true;
        }
    }
}


