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
        public decimal TotalAbonado { get; }
        public int TarjetaId { get; }
        public bool SaldoNegativoCancelado { get; }

        public Boleto(decimal monto, string tipo, string linea, decimal saldoRestante, int viajeId, DateTime fecha, string tipoTarjeta, decimal totalAbonado, int tarjetaId, bool saldoNegativoCancelado)
        {
            Monto = monto;
            Tipo = tipo;
            Linea = linea;
            SaldoRestante = saldoRestante;
            ViajeId = viajeId;
            Fecha = fecha;
            TipoTarjeta = tipoTarjeta;
            TotalAbonado = totalAbonado;
            TarjetaId = tarjetaId;
            SaldoNegativoCancelado = saldoNegativoCancelado;
        }

        public void MostrarDetalles()
        {
            Console.WriteLine($"Boleto {ViajeId} - Fecha: {Fecha.ToString("dd/MM/yyyy HH:mm")} - Tipo de Tarjeta: {TipoTarjeta} - Línea: {Linea}");
            Console.WriteLine($"Monto: ${Monto} - Total Abonado: ${TotalAbonado} - Saldo Restante: ${SaldoRestante}");
            Console.WriteLine(SaldoNegativoCancelado ? "Se canceló el saldo negativo con este boleto." : "No había saldo negativo pendiente.");
        }
    }
}
