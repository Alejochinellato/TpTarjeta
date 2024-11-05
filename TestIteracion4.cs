using ManejoDeTiempos;
using NUnit.Framework;
using TransporteUrbano;

namespace MiProyecto_Tests
{
    public class TestsIteracion4
    {
        private Tarjeta tarjeta;
        private TiempoFalso tiempoFalso;
        private Colectivo colectivo;

        [SetUp]
        public void Setup()
        {
            tiempoFalso = new TiempoFalso();
            tarjeta = new Tarjeta(9000, tiempoFalso);
            colectivo = new Colectivo("123", false);
        }

        [Test]
        public void PruebaUsoFrecuente()
        {
            for (int i = 1; i <= 30; i++)
            {
                colectivo.PagarCon(tarjeta, tiempoFalso);
                tarjeta.CargarSaldo(2000);
            }

            Assert.That(tarjeta.CalcularCostoViaje(false), Is.EqualTo(1200 * 0.8));

            for (int i = 0; i <= 49; i++)
            {
                colectivo.PagarCon(tarjeta, tiempoFalso);
                tarjeta.CargarSaldo(2000);
            }

            Assert.That(tarjeta.CalcularCostoViaje(false), Is.EqualTo(1200 * 0.75));

            for (int i = 0; i <= 5; i++)
            {
                colectivo.PagarCon(tarjeta, tiempoFalso);
                tarjeta.CargarSaldo(2000);
            }

            Assert.That(tarjeta.CalcularCostoViaje(false), Is.EqualTo(1200));
        }

        [Test]
        public void FranjaHorariaFranquicias()
        {
            //lunes 4/11/2024 00:00:00
            FranquiciaCompleta franquiciaCompleta = new FranquiciaCompleta(2000, tiempoFalso);
            MedioBoleto medioBoleto = new MedioBoleto(2000, tiempoFalso);

            colectivo.PagarCon(franquiciaCompleta, tiempoFalso);
            colectivo.PagarCon(medioBoleto, tiempoFalso);

            Assert.That(franquiciaCompleta.ObtenerSaldo(), Is.EqualTo(2000)); // por estar fuera de horario valido no puede viajar
            Assert.That(medioBoleto.ObtenerSaldo(), Is.EqualTo(2000)); // por estar fuera de horario valido no puede viajar

            tiempoFalso.AgregarDias(5); //00 hs del dia sabado 9/11/2024
            tiempoFalso.AgregarMinutos(15 * 60);//15 hs del dia sabado 9/11/2024

            colectivo.PagarCon(franquiciaCompleta, tiempoFalso);
            colectivo.PagarCon(medioBoleto, tiempoFalso);

            Assert.That(franquiciaCompleta.ObtenerSaldo(), Is.EqualTo(2000)); //No puede viajar por estar fuera de dia valido, el saldo no cambió
            Assert.That(medioBoleto.ObtenerSaldo(), Is.EqualTo(2000)); //No puede viajar por estar fuera de dia valido,el saldo no cambió

            tiempoFalso.AgregarDias(2);//15 hs del dia lunes 11/11/2024

            colectivo.PagarCon(franquiciaCompleta, tiempoFalso);
            colectivo.PagarCon(medioBoleto, tiempoFalso);

            Assert.That(franquiciaCompleta.ObtenerSaldo(), Is.EqualTo(2000)); //viaja gratis
            colectivo.PagarCon(franquiciaCompleta, tiempoFalso);
            Assert.That(franquiciaCompleta.ObtenerSaldo(), Is.EqualTo(2000)); //viaja gratis
            colectivo.PagarCon(franquiciaCompleta, tiempoFalso);
            Assert.That(franquiciaCompleta.ObtenerSaldo(), Is.EqualTo(800)); //viaja y paga por haber utilizado los 2 viajes

            Assert.That(medioBoleto.ObtenerSaldo(), Is.EqualTo(1400));// viaja y paga medio

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
        public void PagarConTarjetaFranquiciaCompletaConSaldoSuficienteViajeInterurbano()
        {

            tiempoFalso.AgregarMinutos(60 * 15);//15:00
            FranquiciaCompleta tarjeta = new FranquiciaCompleta(3000, tiempoFalso);
            Colectivo colectivo = new Colectivo("Línea Interurbana", true);


            Boleto boleto = colectivo.PagarCon(tarjeta, tiempoFalso);


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
