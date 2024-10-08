using System;

namespace TransporteUrbano
{
    public class Colectivo
    {
        private string linea;
        private static int viajeContador = 1;

        public Colectivo(string linea)
        {
            this.linea = linea;
        }

        public Boleto PagarCon(Tarjeta tarjeta)
        {
            if (tarjeta.DescontarPasaje())
            {
                decimal montoCobrado = tarjeta is MedioBoleto ? Tarjeta.CostoPasaje / 2 : Tarjeta.CostoPasaje;
                return new Boleto(linea, Tarifa);
            }

            return null;
        }
    }
}

