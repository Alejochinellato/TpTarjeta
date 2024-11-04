using System;
using ManejoDeTiempos;

namespace TransporteUrbano
{
    public class Colectivo
    {
        public string Linea { get; }
        public bool EsInterurbano { get; }

        public Colectivo(string linea, bool esInterurbano = false)
        {
            Linea = linea;
            EsInterurbano = esInterurbano;
        }


        public Boleto PagarCon(Tarjeta tarjeta, Tiempo tiempo)
        {
            if (tarjeta.DescontarPasaje(EsInterurbano))
            {
                return new Boleto(tarjeta.CalcularCostoViaje(EsInterurbano), EsInterurbano ? "Interurbano" : "Urbano", Linea, tarjeta.ObtenerSaldo(), tarjeta.SaldoPendiente, tarjeta.ViajesMesActual, tarjeta.GetType().Name, tarjeta.Id, tiempo);
            }
            return null;
        }


    }
}
