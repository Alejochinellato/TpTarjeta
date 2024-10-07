using System;

namespace TransporteUrbano
{
    public class Boleto
    {
        public decimal Monto { get; }
        public string Tipo { get; }
        public string Linea { get; }
        public decimal SaldoRestante { get; }
        public int NumeroViaje { get; }

        public Boleto(decimal monto, string tipo, string linea, decimal saldoRestante, int numeroViaje)
        {
            Monto = monto;
            Tipo = tipo;
            Linea = linea;
            SaldoRestante = saldoRestante;
            NumeroViaje = numeroViaje;
        }

        public void Imprimir()
        {
            Console.WriteLine($"Boleto #{NumeroViaje}:");
            Console.WriteLine($"Tipo: {Tipo}");
            Console.WriteLine($"Línea: {Linea}");
            Console.WriteLine($"Monto: ${Monto}");
            Console.WriteLine($"Saldo restante: ${SaldoRestante}");
            Console.WriteLine("-----------------------------");
        }
    }
}
