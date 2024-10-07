using System;

namespace TransporteUrbano
{
    public class Boleto
    {
        public decimal Monto { get; }
        public string Tipo { get; }
        public string Linea { get; }
        public decimal SaldoRestante { get; }
        public int ViajeId { get; }
        public DateTime Fecha { get; }
        public int TarjetaId { get; }
        public string DescripcionSaldoNegativo { get; }

        public Boleto(decimal monto, string tipo, string linea, decimal saldoRestante, int viajeId, DateTime fecha, int tarjetaId, string descripcionSaldoNegativo = "")
        {
            Monto = monto;
            Tipo = tipo;
            Linea = linea;
            SaldoRestante = saldoRestante;
            ViajeId = viajeId;
            Fecha = fecha;
            TarjetaId = tarjetaId;
            DescripcionSaldoNegativo = descripcionSaldoNegativo;
        }

        public void MostrarDetalles()
        {
            Console.WriteLine($"Boleto {ViajeId} - Tipo: {Tipo} - Línea: {Linea} - Monto: ${Monto} - Saldo restante: ${SaldoRestante} - Fecha: {Fecha.ToShortDateString()} - ID Tarjeta: {TarjetaId}");
            if (!string.IsNullOrEmpty(DescripcionSaldoNegativo))
            {
                Console.WriteLine(DescripcionSaldoNegativo);
            }
        }
    }
}
