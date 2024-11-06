using ManejoDeTiempos;
using NUnit.Framework;
using TransporteUrbano;

namespace Tests
{
    public class ColectivoTests
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

        [Test]
        public void PagarConTarjetaNormalConSaldoSuficienteViajeInterurbano()
        {
            // Arrange
            Tarjeta tarjeta = new Tarjeta(3000, tiempoFalso);
            Colectivo colectivo = new Colectivo("Línea Interurbana", true);

            // Act
            Boleto boleto = colectivo.PagarCon(tarjeta, tiempoFalso);

            // Assert
            Assert.IsNotNull(boleto);
            Assert.AreEqual(2500, boleto.Monto);
            Assert.AreEqual("Interurbano", boleto.TipoColectivo);
            Assert.AreEqual("Línea Interurbana", boleto.Linea);
            Assert.AreEqual(500, boleto.SaldoRestante);
        }

        [Test]
        public void PagarConTarjetaNormalSaldoInsuficienteViajeInterurbano()
        {

            Tarjeta tarjeta = new Tarjeta(2000, tiempoFalso);
            Colectivo colectivo = new Colectivo("Línea Interurbana", true);


            Boleto boleto = colectivo.PagarCon(tarjeta, tiempoFalso);


            Assert.IsNull(boleto); // No debería permitir el viaje por saldo insuficiente
            Assert.AreEqual(2000, tarjeta.ObtenerSaldo()); // El saldo debe permanecer igual
        }

        [Test]
        public void PagarConTarjetaFranquiciaEstudiantilConSaldoSuficienteViajeInterurbano()
        {

            tiempoFalso.AgregarMinutos(60 * 15);//15:00
            FranquiciaEstudiantil franquiciaEstudiantil = new FranquiciaEstudiantil(3000, tiempoFalso);
            Colectivo colectivo = new Colectivo("Línea Interurbana", true);

            Boleto boleto = colectivo.PagarCon(franquiciaEstudiantil, tiempoFalso);

            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, boleto.Monto);
            Assert.AreEqual("Interurbano", boleto.TipoColectivo); //es interurbano
            Assert.AreEqual("Línea Interurbana", boleto.Linea);
            Assert.AreEqual(3000, boleto.SaldoRestante); //no cambia el saldo
        }

        [Test]
        public void PagarConTarjetaMedioBoletoConSaldoSuficienteViajeInterurbano()
        {
            tiempoFalso.AgregarMinutos(60 * 15);//15:00
            MedioBoleto medioBoleto = new MedioBoleto(5000, tiempoFalso);
            Colectivo colectivo = new Colectivo("Línea Interurbana", true);


            Boleto boleto = colectivo.PagarCon(medioBoleto, tiempoFalso);


            Assert.IsNotNull(boleto);
            Assert.AreEqual(1250, boleto.Monto); // Precio medio boleto interurbano
            Assert.AreEqual("Interurbano", boleto.TipoColectivo);//es interurbano
            Assert.AreEqual("Línea Interurbana", boleto.Linea);
            Assert.AreEqual(3750, boleto.SaldoRestante);
        }

        [Test]
        public void PagarConTarjetaMedioBoletoSaldoInsuficienteViajeInterurbano()
        {
            tiempoFalso.AgregarMinutos(60 * 15);//15:00
            MedioBoleto medioBoleto = new MedioBoleto(500, tiempoFalso); // Saldo insuficiente
            Colectivo colectivo = new Colectivo("Línea Interurbana", true);


            Boleto boleto = colectivo.PagarCon(medioBoleto, tiempoFalso);


            Assert.IsNull(boleto); // No debería permitir el viaje por saldo insuficiente
            Assert.AreEqual(500, medioBoleto.ObtenerSaldo()); // El saldo debe permanecer igual
        }


    }

}