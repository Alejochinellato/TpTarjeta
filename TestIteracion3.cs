using ManejoDeTiempos;
using NUnit.Framework;
using TransporteUrbano;

namespace MiProyecto_Tests
{
    public class TestsIteracion3
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
        public void Rechaza5Minutos()
        {
            tiempoFalso.AgregarMinutos(700);
            MedioBoleto medioBoleto = new MedioBoleto(1200, tiempoFalso);

            colectivo.PagarCon(medioBoleto, tiempoFalso);
            Assert.That(medioBoleto.ObtenerSaldo(), Is.EqualTo(600));// paga solo 600

            tiempoFalso.AgregarMinutos(1);
            colectivo.PagarCon(medioBoleto, tiempoFalso);
            Assert.That(medioBoleto.ObtenerSaldo(), Is.EqualTo(600));

        }

            [Test]
        public void DosViajesGratis()
        {
            FranquiciaCompleta franquiciaCompleta = new FranquiciaCompleta(2000, tiempoFalso);
            tiempoFalso.AgregarMinutos(700);

            colectivo.PagarCon(franquiciaCompleta, tiempoFalso);
            colectivo.PagarCon(franquiciaCompleta, tiempoFalso);

            Assert.That(franquiciaCompleta.ObtenerSaldo(), Is.EqualTo(2000));// no paga nada en los dos viajes

        }

        [Test]
        public void TerceroPagado()
        {
            FranquiciaCompleta franquiciaCompleta = new FranquiciaCompleta(2000, tiempoFalso);
            tiempoFalso.AgregarMinutos(700);

            colectivo.PagarCon(franquiciaCompleta, tiempoFalso);
            colectivo.PagarCon(franquiciaCompleta, tiempoFalso);
            colectivo.PagarCon(franquiciaCompleta, tiempoFalso);

            Assert.That(franquiciaCompleta.ObtenerSaldo(), Is.EqualTo(800));// solo tiene 2 viajes gratis por dia, el tercero lo paga y descuetna el saldo

        }


        [Test]
        public void SaldoPendiente ()
        {
            tarjeta.CargarSaldo(9000);
            tarjeta.CargarSaldo(9000);
            tarjeta.CargarSaldo(9000);
            tarjeta.CargarSaldo(9000);// hasta aca hay 36000
            tarjeta.CargarSaldo(3000);// queda como saldo pendiente


            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(36000));
            Assert.That(tarjeta.SaldoPendiente, Is.EqualTo(3000));
        }

        [Test]
        public void AcreditaSaldoPendiente()
        {
            tarjeta.CargarSaldo(9000);
            tarjeta.CargarSaldo(9000);
            tarjeta.CargarSaldo(9000);
            tarjeta.CargarSaldo(9000);// hasta aca hay 36000
            tarjeta.CargarSaldo(3000);// queda como saldo pendiente


            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(36000));
            colectivo.PagarCon(tarjeta, tiempoFalso);

            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(36000));
            Assert.That(tarjeta.SaldoPendiente, Is.EqualTo(1800));
        }

    }
}
