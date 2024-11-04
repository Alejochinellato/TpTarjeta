using ManejoDeTiempos;
using NUnit.Framework;
using TransporteUrbano;

namespace MiProyecto_Tests
{
    public class TestsIteracion1
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
        public void CargaYDescuentoDeSaldo_Iteracion1()
        {
            tarjeta.CargarSaldo(2000);
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(2000));

            tarjeta.CargarSaldo(3000);
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(5000));

            tarjeta.CargarSaldo(4000);
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(9000));

            tarjeta.CargarSaldo(5000);
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(14000));

            tarjeta.CargarSaldo(6000);
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(20000));

            tarjeta.CargarSaldo(7000);
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(27000));

            tarjeta.CargarSaldo(8000);
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(35000));

            colectivo.PagarCon(tarjeta, tiempoFalso);//33800
            colectivo.PagarCon(tarjeta, tiempoFalso);//32600
            colectivo.PagarCon(tarjeta, tiempoFalso);//31400
            colectivo.PagarCon(tarjeta, tiempoFalso);//30200
            colectivo.PagarCon(tarjeta, tiempoFalso);//29000
            colectivo.PagarCon(tarjeta, tiempoFalso);//27800
            colectivo.PagarCon(tarjeta, tiempoFalso);//26600

            tarjeta.CargarSaldo(9000);
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(35600));


        }
    }
}
