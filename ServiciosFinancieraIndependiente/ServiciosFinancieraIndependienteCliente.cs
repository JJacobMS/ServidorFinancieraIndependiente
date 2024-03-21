using DatosFinancieraIndependiente;
using ServidorFinancieraIndependiente;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorFinancieraIndependiente
{
    public partial class ServiciosFinancieraIndependiente : ICliente
    {
        public Codigo GuardarInformacionCliente(Cliente cliente, ReferenciaTrabajo referenciaTrabajo, ReferenciaCliente[] referenciaCliente, Documento[] documentos)
        {
            Codigo codigo = Codigo.EXITO;
            try
            {
                using (FinancieraBD contexto = new FinancieraBD())
                {
                    List<TipoDocumento> tiposDocumento = contexto.Database.SqlQuery<TipoDocumento>
                        ("SELECT * FROM TipoDocumento").ToList();
                    if (tiposDocumento != null && tiposDocumento.Count > 0 && referenciaTrabajo != null)
                    {
                        if (referenciaTrabajo.idReferenciaTrabajo == 0)
                        {
                            referenciaTrabajo = contexto.ReferenciaTrabajo.Add(referenciaTrabajo);
                            contexto.SaveChanges();
                        }

                        cliente.ReferenciaTrabajo_idReferenciaTrabajo = referenciaTrabajo.idReferenciaTrabajo;
                        contexto.Database.ExecuteSqlCommand
                            ("INSERT INTO Cliente (correoElectronico, nombres, apellidos, esDeudor, direccion, cuentaDeposito, cuentaCobro, rfc, ReferenciaTrabajo_idReferenciaTrabajo) " +
                             "VALUES (@correo, @nombre, @apellidos, 0, @direccion, @cuentaDeposito, @cuentaCobro, @rfc, @refTra)",
                                new SqlParameter("@correo", cliente.correoElectronico), new SqlParameter("@nombre", cliente.nombres), new SqlParameter("@apellidos", cliente.apellidos),
                                 new SqlParameter("@direccion", cliente.direccion), new SqlParameter("@cuentaDeposito", cliente.cuentaDeposito), new SqlParameter("@cuentaCobro", cliente.cuentaCobro),
                                  new SqlParameter("@rfc", cliente.rfc), new SqlParameter("@refTra", referenciaTrabajo.idReferenciaTrabajo));

                        cliente.idCliente = contexto.Database.SqlQuery<int>
                            ("SELECT idCliente FROM Cliente WHERE rfc = @rfc", new SqlParameter("@rfc", cliente.rfc)).FirstOrDefault();

                        List<Telefono> telefonos = cliente.Telefono.ToList();
                        for (int i = 0; i < telefonos.Count; i++)
                        {
                            telefonos[i].Cliente_idCliente = cliente.idCliente;
                            contexto.Telefono.Add(telefonos[i]);
                            contexto.SaveChanges();
                        }

                        for (int i = 0; i < referenciaCliente.Length; i++)
                        {
                            referenciaCliente[i].Cliente_idCliente = cliente.idCliente;
                            contexto.ReferenciaCliente.Add(referenciaCliente[i]);
                            contexto.SaveChanges();
                        }

                        tiposDocumento.ForEach(tipo => {
                            foreach (Documento documento in documentos)
                            {
                                if (documento.TipoDocumento.descripcion.Equals(tipo.descripcion))
                                {
                                    documento.TipoDocumento_idTipoDocumento = tipo.idTipoDocumento;
                                    return;
                                }
                            }
                        });

                        for (int i = 0; i < documentos.Length; i++)
                        {
                            if (documentos[i].TipoDocumento_idTipoDocumento != 0)
                            {
                                contexto.Database.ExecuteSqlCommand
                                    ("INSERT Documento VALUES (@archivo, @nombre, @extension, @idCliente, @idTipo)", 
                                    new SqlParameter("@archivo", documentos[i].archivo), new SqlParameter("@nombre", documentos[i].nombre),
                                    new SqlParameter("@extension", documentos[i].extension), new SqlParameter("@idCliente", cliente.idCliente),
                                    new SqlParameter("@idTipo", documentos[i].TipoDocumento_idTipoDocumento));
                            }   
                        }

                        codigo = Codigo.EXITO;
                    }

                }
            }
            catch (SqlException ex)
            {
                codigo = Codigo.ERROR_BD;
                Console.WriteLine(ex);
            }
            catch (EntityException ex)
            {
                codigo = Codigo.ERROR_BD;
                Console.WriteLine(ex);
            }
            return codigo;
        }

        public (bool, Codigo) ValidarRfcClienteUnico(string rfcCliente)
        {
            bool esUnicoRfc = false;
            Codigo codigo = Codigo.EXITO;
            try
            {
                using (FinancieraBD contexto = new FinancieraBD())
                {
                    List<Cliente> usuariosConElMismoRFC = contexto.Database.SqlQuery<Cliente>
                        ("SELECT * FROM Cliente WHERE rfc = @rfc", new SqlParameter("@rfc", rfcCliente))
                        .ToList();

                    if (usuariosConElMismoRFC == null || usuariosConElMismoRFC.Count == 0)
                    {
                        esUnicoRfc = true;
                    }
                }
            }
            catch (SqlException ex)
            {
                codigo = Codigo.ERROR_BD;
                Console.WriteLine(ex);
            }
            catch (EntityException ex)
            {
                codigo = Codigo.ERROR_BD;
                Console.WriteLine(ex);
            }
            return (esUnicoRfc, codigo);
        }

        public (Codigo, ClienteRFC) BuscarClientePorRFC(string rfc)
        {
            ClienteRFC cliente = null;
            Codigo codigo = Codigo.EXITO;

            try
            {
                using (FinancieraBD contexto = new FinancieraBD())
                {
                    var clienteRecuperado = contexto.Cliente.Where(c => c.rfc == rfc).Take(1).SingleOrDefault();

                    if (clienteRecuperado != null)
                    {
                        cliente = new ClienteRFC
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
                            .SelectMany((t, index) => new[] { t.idTelefono.ToString(), t.numeroTelefonico.Substring(t.numeroTelefonico.Length - 4) })
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
