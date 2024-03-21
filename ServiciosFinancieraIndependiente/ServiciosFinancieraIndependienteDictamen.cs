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
    public partial class ServiciosFinancieraIndependiente : IDictamen
    {
        public Codigo GuardarDictamen(Dictamen dictamen)
        {
            Codigo codigo = new Codigo();
            try
            {
                using (FinancieraBD context = new FinancieraBD())
                {
                    context.Dictamen.Add(dictamen);
                    context.SaveChanges();
                    codigo = Codigo.EXITO;
                }
            }
            catch (EntityException ex)
            {
                Console.WriteLine(ex.ToString());
                codigo = Codigo.ERROR_BD;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                codigo = Codigo.ERROR_SERVIDOR;
            }
            return codigo;
        }
    }
}
