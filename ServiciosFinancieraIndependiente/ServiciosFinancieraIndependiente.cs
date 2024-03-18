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
    public partial class ServiciosFinancieraIndependiente : IPoliticaOtorgamiento
    {

        public int GuardarPoliticaOtorgamiento(int numero)
        {
            Console.WriteLine("Cliente "+ numero);
            try
            {
                using (FinancieraBD context = new FinancieraBD())
                {
                    Politica politica = new Politica
                    {
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

