using ManejoDeTiempos;
using NUnit.Framework;
using TransporteUrbano;

namespace MiProyecto_Tests
{
    public class TestsIteracion2
    {
        private Tarjeta tarjeta;
        private Tarjeta tarjeta1;
        private TiempoFalso tiempoFalso;
        private Colectivo colectivo;

        [SetUp]
        public void Setup()
        {
            tiempoFalso = new TiempoFalso();
            tarjeta = new Tarjeta(0, tiempoFalso);
            colectivo = new Colectivo("123", false);
        }

        [Test]
        public void PagaConPositivo()
        {
            tarjeta.CargarSaldo(2000);
            colectivo.PagarCon(tarjeta, tiempoFalso);

            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(800));// tiene 2000 y paga 1200, entonce sse queda con 800

        }

        [Test]
        public void MasDatosSobreTarjetaTarjetaNormal()
        {

            var tiempoFalso = new TiempoFalso();
            Tarjeta tarjeta = new Tarjeta(2000, tiempoFalso);  // Tarjeta con saldo suficiente
            Colectivo colectivo = new Colectivo("Línea A", false);


            Boleto boleto = colectivo.PagarCon(tarjeta, tiempoFalso);


             
            Assert.IsNotNull(boleto, "El boleto debería generarse correctamente.");
            Assert.AreEqual("Urbano", boleto.TipoColectivo, "El tipo de colectivo debería ser Urbano(false).");
            Assert.AreEqual(0, boleto.SaldoPendiente, "El saldo pendiento debería ser 0.");
            Assert.AreEqual("Tarjeta", boleto.TipoTarjeta, "El tipo de la tarjeta debería ser Regular.");
            Assert.AreEqual(1, boleto.NumeroViaje, "El número de viaje debería ser 1.");
            Assert.AreEqual(800, boleto.SaldoRestante, "El saldo restante debería ser 800 después del pago.");
            Assert.AreEqual(tiempoFalso.Now(), boleto.Fecha, "La fecha del boleto debería coincidir con la fecha actual de TiempoFalso.");
            Assert.AreEqual("Línea A", boleto.Linea, "La línea del colectivo debería ser 'Línea A'.");
            Assert.AreEqual(1200, boleto.Monto, "El total abonado debería ser 1200.");
            Assert.AreEqual(tarjeta.Id, boleto.IdTarjeta, "El ID de la tarjeta debería coincidir con el ID del boleto.");
        }

        [Test]
        public void PagaConNegativo()
        {
            tarjeta =  new Tarjeta(1000, tiempoFalso);
            colectivo.PagarCon(tarjeta, tiempoFalso);

            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(-200));

        }

        [Test]
        public void SinSaldoNoViaja()
        {
            tarjeta1 = new Tarjeta(100, tiempoFalso);
            colectivo.PagarCon(tarjeta1, tiempoFalso);

            Assert.That(tarjeta1.ObtenerSaldo(), Is.EqualTo(100));// no se modifica porque no viaja, por ende no puede quedar con menos de -480
        }

        [Test]
        public void VerificarMedioBoleto()
        {
            tiempoFalso.AgregarMinutos(700);
            MedioBoleto medioBoleto = new MedioBoleto(1200, tiempoFalso);

            colectivo.PagarCon(medioBoleto, tiempoFalso);
            Assert.That(medioBoleto.ObtenerSaldo(), Is.EqualTo(600));// paga solo 600
        }

        [Test]
        public void VerificarFranquiciaCompleta()
        {
            tiempoFalso.AgregarMinutos(700);
            FranquiciaCompleta franquiciaCompleta = new FranquiciaCompleta(2000, tiempoFalso);
            colectivo.PagarCon(franquiciaCompleta, tiempoFalso);
            Assert.That(franquiciaCompleta.ObtenerSaldo(), Is.EqualTo(2000));// No paga nada
        }
    }
}
