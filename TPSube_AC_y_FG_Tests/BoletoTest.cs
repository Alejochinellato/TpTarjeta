using ManejoDeTiempos;
using NUnit.Framework;
using TransporteUrbano;

namespace Tests
{
    public class BoletoTests
    {
        private Tarjeta tarjeta;
        private TiempoFalso tiempoFalso;
        private Colectivo colectivo;

        [SetUp]
        public void Setup()
        {
            tiempoFalso = new TiempoFalso();
            tarjeta = new Tarjeta(0, tiempoFalso);
            colectivo = new Colectivo("123", false);
        }

        [Test]// tarjeta normal 
        public void CrearBoletoTarjetaNormal()
        {

            var tiempoFalso = new TiempoFalso();
            tiempoFalso.AgregarMinutos(60 * 15);

            Tarjeta tarjeta = new Tarjeta(2000, tiempoFalso);  // Tarjeta con saldo suficiente
            Urbano colectivo = new("122");

            Boleto boleto = colectivo.PagarCon(tarjeta, tiempoFalso);

            Assert.IsNotNull(boleto, "El boleto debería generarse correctamente.");
            Assert.AreEqual("Urbano", boleto.TipoColectivo, "El tipo de colectivo debería ser Urbano(false).");
            Assert.AreEqual(0, boleto.SaldoPendiente, "El saldo pendiento debería ser 0.");
            Assert.AreEqual("Tarjeta", boleto.TipoTarjeta, "El tipo de la tarjeta debería ser Regular.");
            Assert.AreEqual(1, boleto.NumeroViaje, "El número de viaje debería ser 1.");
            Assert.AreEqual(800, boleto.SaldoRestante, "El saldo restante debería ser 800 después del pago.");
            Assert.AreEqual(tiempoFalso.Now(), boleto.Fecha, "La fecha del boleto debería coincidir con la fecha actual de TiempoFalso.");
            Assert.AreEqual("122", boleto.Linea, "La línea del colectivo debería ser 'Línea A'.");
            Assert.AreEqual(1200, boleto.Monto, "El total abonado debería ser 1200.");
            Assert.AreEqual(tarjeta.Id, boleto.IdTarjeta, "El ID de la tarjeta debería coincidir con el ID del boleto.");
        }

        [Test]//MedioBoleto
        public void CrearBoletoMedioBoleto()
        {

            var tiempoFalso = new TiempoFalso();
            tiempoFalso.AgregarMinutos(60 * 15);

            MedioBoleto medioBoleto = new MedioBoleto(2000, tiempoFalso);  // Tarjeta con saldo suficiente
            Colectivo colectivo = new Colectivo("Línea A", false);

            Boleto boleto = colectivo.PagarCon(medioBoleto, tiempoFalso);

            Assert.IsNotNull(boleto, "El boleto debería generarse correctamente.");
            Assert.AreEqual("Urbano", boleto.TipoColectivo, "El tipo de colectivo debería ser Urbano(false).");
            Assert.AreEqual(0, boleto.SaldoPendiente, "El saldo pendiento debería ser 0.");
            Assert.AreEqual("MedioBoleto", boleto.TipoTarjeta, "El tipo de la tarjeta debería ser Regular.");
            Assert.AreEqual(1, boleto.NumeroViaje, "El número de viaje debería ser 1.");
            Assert.AreEqual(1400, boleto.SaldoRestante, "El saldo restante debería ser 800 después del pago.");
            Assert.AreEqual(tiempoFalso.Now(), boleto.Fecha, "La fecha del boleto debería coincidir con la fecha actual de TiempoFalso.");
            Assert.AreEqual("Línea A", boleto.Linea, "La línea del colectivo debería ser 'Línea A'.");
            Assert.AreEqual(600, boleto.Monto, "El total abonado debería ser 1200.");
            Assert.AreEqual(medioBoleto.Id, boleto.IdTarjeta, "El ID de la tarjeta debería coincidir con el ID del boleto.");
        }

        [Test]//FranquiciaEstudiantil
        public void CrearBoletoFranquiciaEstudiantil()
        {

            var tiempoFalso = new TiempoFalso();
            tiempoFalso.AgregarMinutos(60 * 15);

            FranquiciaEstudiantil franquiciaEstudiantil = new FranquiciaEstudiantil(2000, tiempoFalso);  // Tarjeta con saldo suficiente
            Colectivo colectivo = new Colectivo("Línea A", false);


            Boleto boleto = colectivo.PagarCon(franquiciaEstudiantil, tiempoFalso);



            Assert.IsNotNull(boleto, "El boleto debería generarse correctamente.");
            Assert.AreEqual("Urbano", boleto.TipoColectivo, "El tipo de colectivo debería ser Urbano(false).");
            Assert.AreEqual(0, boleto.SaldoPendiente, "El saldo pendiento debería ser 0.");
            Assert.AreEqual("FranquiciaEstudiantil", boleto.TipoTarjeta, "El tipo de la tarjeta debería ser Regular.");
            Assert.AreEqual(1, boleto.NumeroViaje, "El número de viaje debería ser 1.");
            Assert.AreEqual(2000, boleto.SaldoRestante, "El saldo restante debería ser 800 después del pago.");
            Assert.AreEqual(tiempoFalso.Now(), boleto.Fecha, "La fecha del boleto debería coincidir con la fecha actual de TiempoFalso.");
            Assert.AreEqual("Línea A", boleto.Linea, "La línea del colectivo debería ser 'Línea A'.");
            Assert.AreEqual(0, boleto.Monto, "El total abonado debería ser 1200.");
            Assert.AreEqual(franquiciaEstudiantil.Id, boleto.IdTarjeta, "El ID de la tarjeta debería coincidir con el ID del boleto.");
        }

        [Test]//FranquiciaJubilado
        public void CrearBoletoFranquiciaJubilados()
        {

            var tiempoFalso = new TiempoFalso();
            tiempoFalso.AgregarMinutos(60 * 15);
            FranquiciaJubilados franquiciaJubilados = new FranquiciaJubilados(2000, tiempoFalso);  // Tarjeta con saldo suficiente
            Colectivo colectivo = new Colectivo("Línea A", false);


            Boleto boleto = colectivo.PagarCon(franquiciaJubilados, tiempoFalso);



            Assert.IsNotNull(boleto, "El boleto debería generarse correctamente.");
            Assert.AreEqual("Urbano", boleto.TipoColectivo, "El tipo de colectivo debería ser Urbano(false).");
            Assert.AreEqual(0, boleto.SaldoPendiente, "El saldo pendiento debería ser 0.");
            Assert.AreEqual("FranquiciaJubilados", boleto.TipoTarjeta, "El tipo de la tarjeta debería ser Regular.");
            Assert.AreEqual(1, boleto.NumeroViaje, "El número de viaje debería ser 1.");
            Assert.AreEqual(2000, boleto.SaldoRestante, "El saldo restante debería ser 800 después del pago.");
            Assert.AreEqual(tiempoFalso.Now(), boleto.Fecha, "La fecha del boleto debería coincidir con la fecha actual de TiempoFalso.");
            Assert.AreEqual("Línea A", boleto.Linea, "La línea del colectivo debería ser 'Línea A'.");
            Assert.AreEqual(0, boleto.Monto, "El total abonado debería ser 1200.");
            Assert.AreEqual(franquiciaJubilados.Id, boleto.IdTarjeta, "El ID de la tarjeta debería coincidir con el ID del boleto.");
        }

    }

}