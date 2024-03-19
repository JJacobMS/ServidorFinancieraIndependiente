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
    public partial class ServiciosFinancieraIndependiente : ITipoUsuario
    {
        public (List<TipoUsuario>, Codigo) RecuperarTiposUsuario()
        {
            List<TipoUsuario> tiposUsuarios = null;
            Codigo codigo = Codigo.EXITO;
            try
            {
                using (FinancieraBD contexto = new FinancieraBD())
                {
                    List<TipoUsuario> listaTipos = contexto.Database.SqlQuery<TipoUsuario>
                        ("SELECT * FROM TipoUsuario").ToList();

                    if (listaTipos != null)
                    {
                        tiposUsuarios = new List<TipoUsuario>();
                        foreach (TipoUsuario tipoRecuperados in listaTipos)
                        {
                            TipoUsuario tipoUsuario = new TipoUsuario
                            {
                                idTipoUsuario = tipoRecuperados.idTipoUsuario,
                                nombre = tipoRecuperados.nombre
                            };
                            tiposUsuarios.Add(tipoUsuario);
                        }
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
            return (tiposUsuarios, codigo);
        }
    }
}
