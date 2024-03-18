using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatosFinancieraIndependiente;

namespace ServidorFinancieraIndependiente
{
    public partial class ServiciosFinancieraIndependiente : IPoliticaOtorgamiento
    {

        public int GuardarPoliticaOtorgamiento(int numero)
        {
            try
            {
                using (FinancieraBDEntities context = new FinancieraBDEntities())
                {
                    Politica politica = new Politica
                    {
                        idPolitica = 1,
                        nombre = "Politica1",
                        descripcion = "Descripcion Politica 1",
                        estaActiva = true,
                        vigencia = DateTime.Now
                    };
                    context.Politica.Add(politica);
                    Console.WriteLine(politica.nombre);
                    context.SaveChanges();
                    return 1;

                }
            }
            catch (EntityException ex)
            {
                Console.WriteLine(ex.ToString());
                return 0;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }
    }
}
