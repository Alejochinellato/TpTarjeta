using ManejoDeTiempos;

using System;

namespace TransporteUrbano
{
    public class Boleto
    {
        public decimal Monto { get; }
        public string TipoColectivo { get; }
        public string Linea { get; }
        public decimal SaldoRestante { get; }
        public decimal SaldoPendiente { get; }
        public int NumeroViaje { get; }
        public string TipoTarjeta { get; }
        public DateTime Fecha { get; }
        public string IdTarjeta { get; }


        public Boleto(decimal monto, string tipoColectivo, string linea, decimal saldoRestante, decimal saldoPendiente, int numeroViaje, string tipoTarjeta, string id, Tiempo tiempo)
        {
            Monto = monto;
            TipoColectivo = tipoColectivo;
            Linea = linea;
            SaldoRestante = saldoRestante;
            SaldoPendiente = saldoPendiente;
            NumeroViaje = numeroViaje;
            TipoTarjeta = tipoTarjeta;
            Fecha = tiempo.Now();
            IdTarjeta = id;
        }

        public void Imprimir()
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine($"Boleto #{NumeroViaje}");
            Console.WriteLine($"Fecha: {Fecha}");
            Console.WriteLine($"Tipo Tarjeta: {TipoTarjeta}");
            Console.WriteLine($"Tipo Colectivo: {TipoColectivo}");
            Console.WriteLine($"Línea: {Linea}");
            Console.WriteLine($"Monto Abonado: ${Monto}");
            Console.WriteLine($"Saldo Restante: ${SaldoRestante}");
            Console.WriteLine($"Saldo Pendiente por Acreditar: ${SaldoPendiente}");
            Console.WriteLine($"ID Tarjeta: {IdTarjeta}");
            Console.WriteLine("-----------------------------");
        }
    }
}
