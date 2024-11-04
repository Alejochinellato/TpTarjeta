using System;
using ManejoDeTiempos;

namespace TransporteUrbano
{
    class Program
    {
        static void Main(string[] args)
        {
            Tiempo tiempo = new Tiempo(); // o TiempoFalso para pruebas
            decimal saldoInicial = SolicitarSaldoInicial();
            Tarjeta tarjeta = SeleccionarTipoTarjeta(saldoInicial, tiempo);
            Colectivo colectivo = SeleccionarTipoColectivo();

            bool continuar = true;
            while (continuar)
            {
                MostrarMenu();

                Console.Write("Elige una opción: ");
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        MostrarSaldo(tarjeta);
                        break;
                    case "2":
                        CargarSaldo(tarjeta);
                        break;
                    case "3":
                        PagarBoleto(tarjeta, colectivo, tiempo);
                        break;
                    case "4":
                        continuar = false;
                        Console.WriteLine("Saliendo del programa...");
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Intenta de nuevo.");
                        break;
                }
            }
        }

        static void MostrarMenu()
        {
            Console.WriteLine("-------------------------------");
            Console.WriteLine("|          OPCIONES           |");
            Console.WriteLine("-------------------------------");
            Console.WriteLine("| 1. Ver saldo                |");
            Console.WriteLine("| 2. Cargar saldo             |");
            Console.WriteLine("| 3. Pagar boleto             |");
            Console.WriteLine("| 4. Salir                    |");
            Console.WriteLine("-------------------------------");
        }

        static decimal SolicitarSaldoInicial()
        {
            decimal saldoInicial;

            while (true)
            {
                Console.WriteLine("Ingrese el saldo inicial de la tarjeta :");
                if (decimal.TryParse(Console.ReadLine(), out saldoInicial) && saldoInicial <= Tarjeta.LimiteSaldo)
                {
                    return saldoInicial;
                }
                else
                {
                    Console.WriteLine("Saldo no válido. Debe ser un número menor o igual a 36000.");
                }
            }
        }

        static Tarjeta SeleccionarTipoTarjeta(decimal saldoInicial, Tiempo tiempo)
        {
            Console.WriteLine("Elija el tipo de tarjeta: 1. Regular, 2. Medio Boleto, 3. Franquicia Completa");
            string tipoTarjeta = Console.ReadLine();

            switch (tipoTarjeta)
            {
                case "2":
                    return new MedioBoleto(saldoInicial, tiempo);
                case "3":
                    return new FranquiciaCompleta(saldoInicial, tiempo);
                default:
                    return new Tarjeta(saldoInicial, tiempo);
            }
        }

        static Colectivo SeleccionarTipoColectivo()
        {
            Console.WriteLine("Ingrese la línea del colectivo:");
            string linea = Console.ReadLine();

            Console.WriteLine("¿Es un colectivo interurbano? (s/n):");
            string interurbanoRespuesta = Console.ReadLine();
            bool esInterurbano = interurbanoRespuesta.ToLower() == "s";
            return new Colectivo(linea, esInterurbano);
        }

        static void MostrarSaldo(Tarjeta tarjeta)
        {
            Console.WriteLine($"Saldo actual: ${tarjeta.ObtenerSaldo()}");
            Console.WriteLine($"Saldo pendiente: ${tarjeta.SaldoPendiente}");
        }

        static void CargarSaldo(Tarjeta tarjeta)
        {
            Console.WriteLine("Ingrese el monto a cargar (solo son validos 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000):");
            if (decimal.TryParse(Console.ReadLine(), out decimal monto) && tarjeta.CargarSaldo(monto))
            {
                Console.WriteLine("Saldo cargado exitosamente.");
            }
            else
            {
                Console.WriteLine("Monto no válido. Debe estar entre 2000 y 9000.");
            }
        }

        static void PagarBoleto(Tarjeta tarjeta, Colectivo colectivo, Tiempo tiempo)
        {
            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);

            if (boleto != null)
            {
                boleto.Imprimir();
            }
            else
            {
                Console.WriteLine("No se pudo realizar el pago del boleto.");
            }
        }
    }
}
