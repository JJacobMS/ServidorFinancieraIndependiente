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

        public (Codigo, List<Politica>) RecuperarPoliticasChecklist(int folioCredito)
        {
            ActualizarVigenciaPoliticas();
            Codigo codigo = new Codigo();
            List<Politica> politicas = null;
            try
            {
                using (FinancieraBD context = new FinancieraBD())
                {
                    List<Politica> politicasOtorgamiento = context.Database.SqlQuery<Politica>("SELECT Politica.idPolitica, Politica.nombre, Politica.descripcion, Politica.vigencia, Politica.estaActiva " +
                        "  FROM Politica  INNER JOIN ChecklistPolitica on Politica.idPolitica=ChecklistPolitica.Politica_idPolitica " +
                        " INNER JOIN Checklist on Checklist.idChecklist=ChecklistPolitica.Checklist_idChecklist " +
                        " INNER JOIN Credito on Credito.Checklist_idChecklist=Checklist.idChecklist where Credito.folioCredito=@folio AND Politica.estaActiva=1;", new SqlParameter("@folio", folioCredito)).ToList();
                    codigo = Codigo.EXITO;
                    if (politicasOtorgamiento != null && politicasOtorgamiento.Count() > 0)
                    {
                        politicas = new List<Politica>();
                        foreach (Politica politica in politicasOtorgamiento)
                        {
                            Politica politicaNueva = new Politica()
                            {
                                idPolitica = politica.idPolitica,
                                nombre = politica.nombre,
                                descripcion = politica.descripcion,
                                estaActiva = politica.estaActiva,
                                vigencia = politica.vigencia
                            };
                            politicas.Add(politicaNueva);
                        }
                    }
                }
            }
            catch (EntityException ex)
            {
                Console.WriteLine(ex.ToString());
                codigo = Codigo.ERROR_BD;
                politicas = null;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                codigo = Codigo.ERROR_SERVIDOR;
                politicas = null;
            }
            return (codigo, politicas);
        }

    }
}
