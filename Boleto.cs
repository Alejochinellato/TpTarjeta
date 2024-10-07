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
        public string TipoTarjeta { get; }
        public int TarjetaId { get; }
        public bool SaldoNegativoCancelado { get; }

        public Boleto(decimal monto, string tipo, string linea, decimal saldoRestante, int viajeId, DateTime fecha, string tipoTarjeta, int tarjetaId, bool saldoNegativoCancelado)
        {
            Monto = monto;
            Tipo = tipo;
            Linea = linea;
            SaldoRestante = saldoRestante;
            ViajeId = viajeId;
            Fecha = fecha;
            TipoTarjeta = tipoTarjeta;
            TarjetaId = tarjetaId;
            SaldoNegativoCancelado = saldoNegativoCancelado;
        }

        public void MostrarDetalles()
        {
            Console.WriteLine($"Boleto {ViajeId} - Fecha: {Fecha} - Tipo: {Tipo} - Tipo de Tarjeta: {TipoTarjeta} - Línea: {Linea} - Monto: ${Monto} - Saldo restante: ${SaldoRestante} - ID Tarjeta: {TarjetaId}");

            if (SaldoNegativoCancelado)
            {
                Console.WriteLine($"El saldo negativo ha sido cancelado con este pago.");
            }
        }
    }
}
