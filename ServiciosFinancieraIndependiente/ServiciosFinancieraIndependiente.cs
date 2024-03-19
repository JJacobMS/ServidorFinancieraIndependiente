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

        public int GuardarPoliticaOtorgamiento(Politica politica)
        {
            try
            {
                using (FinancieraBD context = new FinancieraBD())
                {
                    Console.WriteLine("Politica "+politica.nombre+" "+politica.descripcion+" "+politica.vigencia);
                    context.Politica.Add(politica);
                    Console.WriteLine(politica.nombre);
                    context.SaveChanges();
                    Console.WriteLine(1);
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

