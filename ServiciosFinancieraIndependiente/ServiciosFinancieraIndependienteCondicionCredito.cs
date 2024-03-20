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
    public partial class ServiciosFinancieraIndependiente : ICondicionCredito
    {
        public Codigo GuardarCondicionCredito(CondicionCredito condicionCredito)
        {
            Codigo codigo = Codigo.EXITO;

            try
            {
                using (FinancieraBD context = new FinancieraBD())
                {
                    context.CondicionCredito.Add(condicionCredito);
                    context.SaveChanges();
                }
            }
            catch (SqlException ex)
            {
                //TODO Log
                codigo = Codigo.ERROR_BD;
                Console.WriteLine(ex.StackTrace);
            }
            catch (EntityException ex)
            {
                //TODO Log
                codigo = Codigo.ERROR_BD;
                Console.WriteLine(ex.StackTrace);
            }

            return codigo;
        }
    }
}
