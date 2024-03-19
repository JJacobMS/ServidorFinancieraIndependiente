using DatosFinancieraIndependiente;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorFinancieraIndependiente
{
    public partial class ServiciosFinancieraIndependiente : IReferenciaTrabajo
    {
        public (List<ReferenciaTrabajo>, Codigo) RecuperarReferenciasTrabajo()
        {
            List<ReferenciaTrabajo> referenciasTrabajo = null;
            Codigo codigo = Codigo.EXITO;
            try
            {
                using (FinancieraBD contexto = new FinancieraBD())
                {
                    List<ReferenciaTrabajo> referenciasRecuperadas = contexto.Database.SqlQuery<ReferenciaTrabajo>
                        ("SELECT * FROM ReferenciaTrabajo").ToList();

                    if (referenciasRecuperadas != null)
                    {
                        referenciasTrabajo = new List<ReferenciaTrabajo>();
                        foreach (ReferenciaTrabajo referenciaRecuperada in referenciasRecuperadas)
                        {
                            ReferenciaTrabajo referenciaNueva = new ReferenciaTrabajo
                            {
                                idReferenciaTrabajo = referenciaRecuperada.idReferenciaTrabajo,
                                nombre = referenciaRecuperada.nombre,
                                direccion = referenciaRecuperada.direccion,
                                telefono = referenciaRecuperada.telefono
                            };
                            referenciasTrabajo.Add(referenciaNueva);

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
            return (referenciasTrabajo, codigo);
        }
    }
}
