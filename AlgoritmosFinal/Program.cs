using System;

namespace CajeroAutomaticoAlgoritmos
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Banco banco = new Banco();
            banco.CargarUsuarios();
            MostrarBienvenida(); // Muestra el mensaje de bienvenida

            while (true)
            {
                MostrarMenuPrincipal();
                string opcion = Console.ReadLine();

                if (opcion == "1")
                {
                    Console.Clear(); // Limpiar la consola
                    MostrarCuadro("Iniciar Sesión");

                    Console.Write("Ingrese su número de cuenta: ");
                    string numeroCuenta = Console.ReadLine();

                    Console.Write("Ingrese su PIN: ");
                    string pin = Console.ReadLine();

                    Usuario usuario = banco.IniciarSesion(numeroCuenta, pin);

                    if (usuario != null)
                    {
                        MostrarBienvenidaUsuario(usuario.Nombre); // Mostrar bienvenida al usuario
                        MostrarMenu(banco, usuario);
                    }
                    else
                    {
                        MostrarError("Número de cuenta o PIN incorrectos.");
                    }
                }
                else if (opcion == "2")
                {
                    Console.Clear(); // Limpiar la consola
                    MostrarCuadro("Crear Nueva Cuenta (Administrador)");

                    Console.Write("Ingrese el nombre del usuario: ");
                    string nombre = Console.ReadLine();

                    Console.Write("Ingrese el número de cuenta: ");
                    string numeroCuenta = Console.ReadLine();

                    Console.Write("Ingrese el PIN: ");
                    string pin = Console.ReadLine();

                    Console.Write("Ingrese el saldo inicial: ");
                    double saldoInicial;
                    while (!double.TryParse(Console.ReadLine(), out saldoInicial) || saldoInicial < 0) // Validar el saldo inicial
                    {
                        Console.WriteLine("Por favor, ingrese un valor numérico válido para el saldo inicial.");
                    }

                    banco.CrearUsuario(nombre, numeroCuenta, pin, saldoInicial);
                    MostrarExito("Cuenta creada exitosamente.");
                }
                else if (opcion == "3")
                {
                    MostrarCuadro("Gracias por utilizar nuestro cajero automático.");
                    break;
                }
                else
                {
                    MostrarError("Opción no válida.");
                }
            }

            banco.GuardarUsuarios(); // Guardar usuarios
        }

        static void MostrarBienvenida()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(new string('=', 40));
            Console.WriteLine("* Bienvenido al Cajero Automático *");
            Console.WriteLine("* Cajero 5B *"); // Mensaje agregado
            Console.WriteLine(new string('=', 40));
            Console.ResetColor();
            Console.WriteLine();
        }

        static void MostrarBienvenidaUsuario(string nombreUsuario)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('=', 40));
            Console.WriteLine($"¡Bienvenido {nombreUsuario}!"); // Mensaje de bienvenida al usuario
            Console.WriteLine(new string('=', 40));
            Console.ResetColor();
            Console.WriteLine();
        }

        static void MostrarMenuPrincipal()
        {
            Console.Clear(); // Limpiar la consola
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('=', 40));
            Console.WriteLine("Seleccione una opción:");
            Console.WriteLine("1. Iniciar sesión");
            Console.WriteLine("2. Crear nueva cuenta (Administrador)");
            Console.WriteLine("3. Salir");
            Console.WriteLine(new string('=', 40));
            Console.ResetColor();
            Console.Write("Opción: ");
        }

        static void MostrarMenu(Banco banco, Usuario usuario)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(new string('=', 40));
                Console.WriteLine($"Bienvenido {usuario.Nombre}, seleccione una opción:");
                Console.WriteLine(new string('=', 40));
                Console.ResetColor();
                Console.WriteLine("1. Consultar saldo");
                Console.WriteLine("2. Retirar dinero");
                Console.WriteLine("3. Depositar dinero");
                Console.WriteLine("4. Ver historial de transacciones");
                Console.WriteLine("5. Cerrar sesión");
                Console.WriteLine(new string('=', 40));
                Console.Write("Opción: ");
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        Console.Clear();
                        banco.ConsultarSaldo(usuario);
                        PausaContinuar();
                        break;
                    case "2":
                        Console.Clear();
                        Console.Write("Ingrese la cantidad a retirar: ");
                        double retiro;
                        while (!double.TryParse(Console.ReadLine(), out retiro) || retiro <= 0) // Validar el retiro
                        {
                            Console.WriteLine("Por favor, ingrese un valor numérico válido para el retiro.");
                        }
                        banco.RetirarDinero(usuario, retiro);
                        PausaContinuar();
                        break;
                    case "3":
                        Console.Clear();
                        Console.Write("Ingrese la cantidad a depositar: ");
                        double deposito;
                        while (!double.TryParse(Console.ReadLine(), out deposito) || deposito <= 0) // Validar el depósito
                        {
                            Console.WriteLine("Por favor, ingrese un valor numérico válido para el depósito.");
                        }
                        banco.DepositarDinero(usuario, deposito);
                        PausaContinuar();
                        break;
                    case "4":
                        Console.Clear();
                        banco.MostrarHistorial(usuario);
                        PausaContinuar();
                        break;
                    case "5":
                        Console.WriteLine("Cerrando sesión...");
                        return;
                    default:
                        MostrarError("Opción no válida.");
                        break;
                }
            }
        }

        static void MostrarCuadro(string texto)
        {
            int longitud = texto.Length + 4;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('=', longitud)); // Línea superior
            Console.WriteLine($"| {texto} |"); // Texto en el cuadro
            Console.WriteLine(new string('=', longitud)); // Línea inferior
            Console.ResetColor();
            Console.WriteLine(); // Espacio en blanco
        }

        static void MostrarError(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"✖ {mensaje} ✖");
            Console.ResetColor();
        }

        static void MostrarExito(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✔ {mensaje} ✔");
            Console.ResetColor();
        }

        static void PausaContinuar()
        {
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}