using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

public class Banco
{
    private List<Usuario> usuarios;

    public Banco()
    {
        usuarios = new List<Usuario>();
    }

    public void CargarUsuarios()
    {
        if (File.Exists("usuarios.json"))
        {
            Console.WriteLine("Cargando datos de usuarios desde el archivo...");

            string jsonString = File.ReadAllText("usuarios.json");
            usuarios = JsonConvert.DeserializeObject<List<Usuario>>(jsonString);

            Console.WriteLine("Datos de usuarios cargados exitosamente.\n");
        }
        else
        {
            usuarios = new List<Usuario>();

            Console.WriteLine("No se encontraron datos previos. Comenzando con una lista de usuarios vacía.\n");
        }
    }

    public void GuardarUsuarios()
    {
        Console.WriteLine("Guardando datos de usuarios...");

        string jsonString = JsonConvert.SerializeObject(usuarios, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText("usuarios.json", jsonString);

        Console.WriteLine("Datos guardados exitosamente en 'usuarios.json'.\n");
    }

    public Usuario IniciarSesion(string numeroCuenta, string pin)
    {
        foreach (Usuario usuario in usuarios)
        {
            if (usuario.NumeroCuenta == numeroCuenta && usuario.PIN == pin)
            {
                Console.WriteLine($"\nInicio de sesión exitoso. Bienvenido, {usuario.Nombre}.");
                return usuario;
            }
        }

        Console.WriteLine("\nNúmero de cuenta o PIN incorrectos. Por favor, intente de nuevo.");
        return null;
    }

    public void CrearUsuario(string nombre, string numeroCuenta, string pin, double saldoInicial)
    {
        Usuario nuevoUsuario = new Usuario(nombre, numeroCuenta, pin, saldoInicial);
        usuarios.Add(nuevoUsuario);
        GuardarUsuarios();

        Console.WriteLine($"Cuenta creada exitosamente para {nombre} con un saldo inicial de Q{saldoInicial}.\n");
    }

    public void ConsultarSaldo(Usuario usuario)
    {
        Console.WriteLine($"\nSaldo actual de la cuenta de {usuario.Nombre}: Q{usuario.Saldo:F2}\n");
    }

    public void RetirarDinero(Usuario usuario, double cantidad)
    {
        if (usuario.Saldo >= cantidad)
        {
            usuario.Saldo -= cantidad;
            usuario.AgregarTransaccion("Retiro", cantidad);

            Console.WriteLine($"Retiro exitoso de Q{cantidad:F2}. Saldo actual: Q{usuario.Saldo:F2}\n");
        }
        else
        {
            Console.WriteLine("Error: Saldo insuficiente para realizar el retiro.\n");
        }
    }

    public void DepositarDinero(Usuario usuario, double cantidad)
    {
        usuario.Saldo += cantidad;
        usuario.AgregarTransaccion("Depósito", cantidad);

        Console.WriteLine($"Depósito exitoso de Q{cantidad:F2}. Saldo actual: Q{usuario.Saldo:F2}\n");
    }

    public void MostrarHistorial(Usuario usuario)
    {
        Console.WriteLine("\nHistorial de Transacciones:");

        if (usuario.Historial.Count == 0)
        {
            Console.WriteLine("No hay transacciones registradas.");
        }
        else
        {
            foreach (var transaccion in usuario.Historial)
            {
                Console.WriteLine($"- {transaccion.Fecha:dd/MM/yyyy HH:mm}: {transaccion.Tipo} de Q{transaccion.Monto:F2}");
            }
        }

        Console.WriteLine($"\nSaldo total: Q{usuario.Saldo:F2}\n");
    }
}