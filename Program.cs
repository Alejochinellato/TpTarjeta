using System;

namespace TransporteUrbano
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ingrese el saldo inicial de la tarjeta:");
            decimal saldoInicial = Convert.ToDecimal(Console.ReadLine());

            Console.WriteLine("Elija el tipo de tarjeta: 1. Regular 2. Medio Boleto 3. Franquicia Completa");
            string tipoTarjeta = Console.ReadLine();

            Tarjeta tarjeta;

            switch (tipoTarjeta)
            {
                case "2":
                    tarjeta = new MedioBoleto(saldoInicial);
                    break;
                case "3":
                    tarjeta = new FranquiciaCompleta(saldoInicial);
                    break;
                default:
                    tarjeta = new Tarjeta(saldoInicial);
                    break;
            }

            Colectivo colectivo = new Colectivo("123");

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
                        PagarBoleto(tarjeta, colectivo);
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

        static void MostrarSaldo(Tarjeta tarjeta)
        {
            Console.WriteLine($"Saldo actual de la tarjeta: ${tarjeta.ObtenerSaldo()}");
        }

        static void CargarSaldo(Tarjeta tarjeta)
        {
            Console.Write("Ingresa el monto a cargar (Recuerde que las opciones de carga son: 2000-3000-4000-5000-6000-7000-8000-9000): ");
            decimal montoCarga;
            if (!decimal.TryParse(Console.ReadLine(), out montoCarga))
            {
                Console.WriteLine("Error: Ingrese un monto válido.");
                return;
            }

            bool cargaExitosa = tarjeta.CargarSaldo(montoCarga);

            if (cargaExitosa)
            {
                Console.WriteLine($"Saldo actual ${tarjeta.ObtenerSaldo()}");
            }
            else
            {
                Console.WriteLine($"Error: El monto ingresado (${montoCarga}) no está permitido.");
            }
        }

        static void PagarBoleto(Tarjeta tarjeta, Colectivo colectivo)
        {
            try
            {
                Boleto boleto = colectivo.PagarCon(tarjeta);

                if (boleto != null)
                {
                    Console.WriteLine("Pago realizado:");
                    boleto.MostrarDetalles();
                }
                else
                {
                    Console.WriteLine("No se pudo realizar el pago. Saldo insuficiente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al pagar el boleto: {ex.Message}");
            }
        }
    }
}
