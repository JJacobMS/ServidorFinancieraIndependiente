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
    public partial class ServiciosFinancieraIndependiente : IChecklistSol
    {
        public (Codigo, List<Checklist>) ObtenerChecklists()
        {
            List<Checklist> checklists = null;
            Codigo codigo = Codigo.EXITO;

            try
            {
                using (FinancieraBD contexto = new FinancieraBD())
                {
                    List<Checklist> checklistsRecuperadas = contexto.Checklist.ToList();

                    if(checklistsRecuperadas.Count != 0)
                    {
                        checklists = new List<Checklist>();
                        foreach (Checklist checklist in checklistsRecuperadas)
                        {
                            Checklist checklistNueva = new Checklist
                            {
                                idChecklist = checklist.idChecklist,
                                nombre = checklist.nombre
                            };

                            checklists.Add(checklistNueva);
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
            return (codigo, checklists);
        }
    }
}
