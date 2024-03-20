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
    public partial class ServiciosFinancieraIndependiente : IChecklist
    {
        public (Codigo, String) RecuperarChecklist2 (int folioCredito)
        {
            String nombre;
            Codigo codigo = new Codigo();
            try
            {
                using (FinancieraBD context = new FinancieraBD())
                {
                    Checklist checklist = context.Database.SqlQuery<Checklist>("SELECT Checklist.idChecklist, Checklist.nombre, Checklist.descripcion " +
                        "  FROM Checklist INNER JOIN Credito on Credito.Checklist_idChecklist=Checklist.idChecklist where Credito.folioCredito=@folio;", new SqlParameter("@folio", folioCredito)).FirstOrDefault();
                    Console.WriteLine(checklist.nombre);
                    codigo = Codigo.EXITO;
                    nombre = checklist.nombre;
                }
            }
            catch (EntityException ex)
            {
                Console.WriteLine(ex.ToString());
                codigo = Codigo.ERROR_BD;
                nombre = null;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                nombre = null;
                codigo = Codigo.ERROR_SERVIDOR;
            }
            return (codigo, nombre);
        }
    }
}
