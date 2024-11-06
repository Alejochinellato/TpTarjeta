using ManejoDeTiempos;
using NUnit.Framework;
using TransporteUrbano;

namespace Tests
{
    public class TarjetaTests
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

            tarjeta.CargarSaldo(3000);//5000
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(5000));

            tarjeta.CargarSaldo(4000);//9000
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(9000));

            tarjeta.CargarSaldo(5000);//14000
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(14000));

            tarjeta.CargarSaldo(6000);//20000
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(20000));

            tarjeta.CargarSaldo(7000);//27000
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(27000));

            tarjeta.CargarSaldo(8000);//35000
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

        [Test]
        public void PagaConPositivo()
        {
            tarjeta.CargarSaldo(2000);
            colectivo.PagarCon(tarjeta, tiempoFalso);

            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(800));// tiene 2000 y paga 1200, entonce sse queda con 800
        }


        [Test]
        public void PagaConNegativo()
        {
            tarjeta = new Tarjeta(1000, tiempoFalso);
            colectivo.PagarCon(tarjeta, tiempoFalso);
            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(-200));

        }

        [Test]
        public void SinSaldoNoViaja()
        {
            tarjeta = new Tarjeta(100, tiempoFalso);
            colectivo.PagarCon(tarjeta, tiempoFalso);

            Assert.That(tarjeta.ObtenerSaldo(), Is.EqualTo(100));// no se modifica porque no viaja, por ende no puede quedar con menos de -480
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
        public void VerificarFranquiciaEstudiantil()
        {
            tiempoFalso.AgregarMinutos(700);
            FranquiciaEstudiantil FranquiciaEstudiantil = new FranquiciaEstudiantil(2000, tiempoFalso);
            colectivo.PagarCon(FranquiciaEstudiantil, tiempoFalso);
            Assert.That(FranquiciaEstudiantil.ObtenerSaldo(), Is.EqualTo(2000));// No paga nada
        }

        [Test]
        public void VerificarFranquiciaJubenil()
        {
            tiempoFalso.AgregarMinutos(700);
            FranquiciaEstudiantil FranquiciaEstudiantil = new FranquiciaEstudiantil(2000, tiempoFalso);
            colectivo.PagarCon(FranquiciaEstudiantil, tiempoFalso);
            Assert.That(FranquiciaEstudiantil.ObtenerSaldo(), Is.EqualTo(2000));// No paga nada
        }

        [Test]
        public void DosViajesGratis()
        {
            FranquiciaEstudiantil FranquiciaEstudiantil = new FranquiciaEstudiantil(2000, tiempoFalso);
            tiempoFalso.AgregarMinutos(700);

            colectivo.PagarCon(FranquiciaEstudiantil, tiempoFalso);
            colectivo.PagarCon(FranquiciaEstudiantil, tiempoFalso);

            Assert.That(FranquiciaEstudiantil.ObtenerSaldo(), Is.EqualTo(2000));// no paga nada en los dos viajes

        }

        [Test]
        public void TerceroPagado()
        {
            FranquiciaEstudiantil FranquiciaEstudiantil = new FranquiciaEstudiantil(2000, tiempoFalso);
            tiempoFalso.AgregarMinutos(700);

            colectivo.PagarCon(FranquiciaEstudiantil, tiempoFalso);
            colectivo.PagarCon(FranquiciaEstudiantil, tiempoFalso);
            colectivo.PagarCon(FranquiciaEstudiantil, tiempoFalso);

            Assert.That(FranquiciaEstudiantil.ObtenerSaldo(), Is.EqualTo(800));// solo tiene 2 viajes gratis por dia, el tercero lo paga y descuetna el saldo

        }

        [Test]
        public void PagosIlimitadosJubilados()
        {
            FranquiciaJubilados FranquiciaJubilados = new FranquiciaJubilados(2000, tiempoFalso);
            tiempoFalso.AgregarMinutos(700);

            colectivo.PagarCon(FranquiciaJubilados, tiempoFalso);
            for (int i = 1; i <= 70; i++)
            {
                colectivo.PagarCon(FranquiciaJubilados, tiempoFalso);
            }//varios pagos seguidos dentro de la franja

            Assert.That(FranquiciaJubilados.ObtenerSaldo(), Is.EqualTo(2000));//paga varios viajes , no tiene limite y tampoco cobra.Sueldo igual.

        }

        [Test]
        public void SaldoPendiente()
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
            FranquiciaEstudiantil franquiciaEstudiantil = new FranquiciaEstudiantil(2000, tiempoFalso);
            MedioBoleto medioBoleto = new MedioBoleto(2000, tiempoFalso);
            FranquiciaJubilados franquiciaJubilados = new FranquiciaJubilados(2000, tiempoFalso);

            colectivo.PagarCon(franquiciaEstudiantil, tiempoFalso);
            colectivo.PagarCon(medioBoleto, tiempoFalso);
            colectivo.PagarCon(franquiciaJubilados, tiempoFalso);

            Assert.That(franquiciaEstudiantil.ObtenerSaldo(), Is.EqualTo(2000)); // por estar fuera de horario valido no puede viajar
            Assert.That(medioBoleto.ObtenerSaldo(), Is.EqualTo(2000)); // por estar fuera de horario valido no puede viajar
            Assert.That(franquiciaJubilados.ObtenerSaldo(), Is.EqualTo(2000));// por estar fuera de horario valido no puede viajar

            tiempoFalso.AgregarDias(5); //00 hs del dia sabado 9/11/2024
            tiempoFalso.AgregarMinutos(15 * 60);//15 hs del dia sabado 9/11/2024

            colectivo.PagarCon(franquiciaEstudiantil, tiempoFalso);
            colectivo.PagarCon(medioBoleto, tiempoFalso);
            colectivo.PagarCon(franquiciaJubilados, tiempoFalso);

            Assert.That(franquiciaEstudiantil.ObtenerSaldo(), Is.EqualTo(2000)); //No puede viajar por estar fuera de dia valido, el saldo no cambió
            Assert.That(medioBoleto.ObtenerSaldo(), Is.EqualTo(2000)); //No puede viajar por estar fuera de dia valido,el saldo no cambió
            Assert.That(franquiciaJubilados.ObtenerSaldo(), Is.EqualTo(2000));// por estar fuera de horario valido no puede viajar

            tiempoFalso.AgregarDias(2);//15 hs del dia lunes 11/11/2024

            colectivo.PagarCon(franquiciaEstudiantil, tiempoFalso);
            colectivo.PagarCon(medioBoleto, tiempoFalso);
            colectivo.PagarCon(franquiciaJubilados, tiempoFalso);

            Assert.That(franquiciaEstudiantil.ObtenerSaldo(), Is.EqualTo(2000)); //viaja gratis
            colectivo.PagarCon(franquiciaEstudiantil, tiempoFalso);
            Assert.That(franquiciaEstudiantil.ObtenerSaldo(), Is.EqualTo(2000)); //viaja gratis
            colectivo.PagarCon(franquiciaEstudiantil, tiempoFalso);
            Assert.That(franquiciaEstudiantil.ObtenerSaldo(), Is.EqualTo(800)); //viaja y paga por haber utilizado los 2 viajes

            Assert.That(medioBoleto.ObtenerSaldo(), Is.EqualTo(1400));// viaja y paga medio

            for (int i = 0; i < 10; i++)
            {
                colectivo.PagarCon(franquiciaJubilados, tiempoFalso);
            }

            Assert.That(franquiciaJubilados.ObtenerSaldo(), Is.EqualTo(2000));

        }


    }
}