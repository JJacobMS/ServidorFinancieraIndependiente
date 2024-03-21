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
        public Codigo GuardarChecklist(Checklist checklist, int[] listaIdPoliticas)
        {
            Codigo codigo = new Codigo();
            int idChecklist = 0;
            try
            {
                using (FinancieraBD context = new FinancieraBD())
                {
                    context.Checklist.Add(checklist);
                    context.SaveChanges();
                    idChecklist = checklist.idChecklist;
                    foreach (int idPolitica in listaIdPoliticas)
                    {
                        ChecklistPolitica checklistPolitica = new ChecklistPolitica
                        {
                            Politica_idPolitica = idPolitica,
                            Checklist_idChecklist = idChecklist,
                        };
                        context.ChecklistPolitica.Add(checklistPolitica);
                        context.SaveChanges();
                    }
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

        public (Codigo, String) RecuperarChecklist (int folioCredito)
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

        public (Codigo, List<Checklist>) ObtenerChecklists()
        {
            List<Checklist> checklists = null;
            Codigo codigo = Codigo.EXITO;

            try
            {
                using (FinancieraBD contexto = new FinancieraBD())
                {
                    List<Checklist> checklistsRecuperadas = contexto.Checklist.ToList();

                    if (checklistsRecuperadas.Count != 0)
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
