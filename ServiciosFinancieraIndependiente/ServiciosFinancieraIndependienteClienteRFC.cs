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
                            idCliente = clienteRecuperado.idCliente,
                            nombres = clienteRecuperado.nombres,
                            apellidos = clienteRecuperado.apellidos,
                            rfc = clienteRecuperado.rfc,
                            esDeudor = clienteRecuperado.esDeudor,
                            correoElectronico = clienteRecuperado.correoElectronico,
                            cuentaCobro = clienteRecuperado.cuentaCobro,
                            cuentaDeposito = clienteRecuperado.cuentaDeposito,
                            direccion = clienteRecuperado.direccion
                        };
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
