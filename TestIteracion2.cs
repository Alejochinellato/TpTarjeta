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
            Colectivo colectivo = new Colectivo("L�nea A", false);


            Boleto boleto = colectivo.PagarCon(tarjeta, tiempoFalso);


            Assert.IsNotNull(boleto, "El boleto deber�a generarse correctamente.");
            Assert.AreEqual(800, boleto.SaldoRestante, "El saldo restante deber�a ser 800 despu�s del pago.");
            Assert.AreEqual(tiempoFalso.Now(), boleto.Fecha, "La fecha del boleto deber�a coincidir con la fecha actual de TiempoFalso.");
            Assert.AreEqual("L�nea A", boleto.Linea, "La l�nea del colectivo deber�a ser 'L�nea A'.");
            Assert.AreEqual(1200, boleto.Monto, "El total abonado deber�a ser 1200.");
            Assert.AreEqual(tarjeta.Id, boleto.IdTarjeta, "El ID de la tarjeta deber�a coincidir con el ID del boleto.");
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
