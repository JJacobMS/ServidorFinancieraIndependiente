using DatosFinancieraIndependiente;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorFinancieraIndependiente
{
    public partial class ServiciosFinancieraIndependiente : IClienteRFC
    {
        public (Codigo, Cliente) BuscarClientePorRFC(string rfc)
        {
            Cliente cliente = null;
            Codigo codigo = Codigo.EXITO;

            try
            {
                using (FinancieraBD contexto = new FinancieraBD())
                {
                    var clienteRecuperado = contexto.Cliente.Where(c => c.rfc == rfc).Take(1).SingleOrDefault();

                    if (clienteRecuperado != null)
                    {
                        cliente = new Cliente
                        {
                            IdCliente = clienteRecuperado.idCliente,
                            Nombres = clienteRecuperado.nombres,
                            Apellidos = clienteRecuperado.apellidos,
                            Rfc = clienteRecuperado.rfc,
                            EsDeudor = clienteRecuperado.esDeudor,
                            CorreoElectronico = clienteRecuperado.correoElectronico,
                            CuentaCobro = clienteRecuperado.cuentaCobro.Substring(clienteRecuperado.cuentaCobro.Length - 3),
                            CuentaDeposito = clienteRecuperado.cuentaDeposito.Substring(clienteRecuperado.cuentaDeposito.Length - 3),
                            Direccion = clienteRecuperado.direccion,
                        };

                        var telefonos = contexto.Telefono
                        .Where(t => t.Cliente_idCliente == clienteRecuperado.idCliente)
                        .Take(2)
                        .ToList();

                        cliente.Telefonos = telefonos
                            .SelectMany((t, index) => new[] { t.idTelefono.ToString(), t.numeroTelefonico.Substring(t.numeroTelefonico.Length - 4)})
                            .ToList();
                    }
                }
            }
            catch (SqlException ex)
            {
                codigo = Codigo.ERROR_BD;
                Console.WriteLine(ex.StackTrace + ex.Message);
            }
            catch (EntityException ex)
            {
                codigo = Codigo.ERROR_BD;
                Console.WriteLine(ex.StackTrace + ex.Message);
            }

            return (codigo, cliente);
        }

    }
}
