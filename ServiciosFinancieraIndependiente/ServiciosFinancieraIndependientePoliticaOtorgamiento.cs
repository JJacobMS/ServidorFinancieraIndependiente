﻿using DatosFinancieraIndependiente;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServidorFinancieraIndependiente
{
    public partial class ServiciosFinancieraIndependiente : IPoliticaOtorgamiento
    {

        public Codigo GuardarPoliticaOtorgamiento(Politica politica)
        {
            Codigo codigo = new Codigo();
            try
            {
                using (FinancieraBD context = new FinancieraBD())
                {
                    context.Politica.Add(politica);
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

        public void ActualizarVigenciaPoliticas()
        {   
            try
            {
                using (FinancieraBD context = new FinancieraBD())
                {
                    context.Database.ExecuteSqlCommand("UPDATE Politica SET estaActiva = CASE WHEN vigencia <= GETDATE() THEN 0 ELSE estaActiva END;");
                }
            }
            catch (EntityException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
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
                    List<Politica> politicasRecuperadas = context.Database.SqlQuery<Politica>("SELECT Politica.nombre, Politica.descripcion, Politica.vigencia, Politica.estaActiva " +
                        "  FROM Politica\r\n  INNER JOIN ChecklistPolitica on Politica.idPolitica=ChecklistPolitica.Politica_idPolitica " +
                        " INNER JOIN Checklist on Checklist.idChecklist=ChecklistPolitica.Checklist_idChecklist " +
                        " INNER JOIN Credito on Credito.Checklist_idChecklist=Checklist.idChecklist where Credito.folioCredito=@folio AND Politica.estaActiva=1;", new SqlParameter("@folio", folioCredito)).ToList();
                    codigo = Codigo.EXITO;
                    if(politicasRecuperadas!=null && politicasRecuperadas.Count() > 0)
                    {
                        politicas = new List<Politica>();
                        foreach (Politica politicaRecuperada in politicasRecuperadas)
                        {
                            Politica politicaNueva = new Politica() 
                            {
                                idPolitica = politicaRecuperada.idPolitica,
                                descripcion = politicaRecuperada.descripcion,
                                estaActiva = politicaRecuperada.estaActiva,
                                vigencia = politicaRecuperada.vigencia
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

