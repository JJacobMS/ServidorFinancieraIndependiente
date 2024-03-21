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
                using (FinancieraBD contexto = new FinancieraBD())
                {
                    contexto.CondicionCredito.Add(condicionCredito);
                    contexto.SaveChanges();
                }
            }
            catch (SqlException ex)
            {
                //TODO Log
                codigo = Codigo.ERROR_BD;
                Console.WriteLine(ex.StackTrace + ex.Message);
            }
            catch (EntityException ex)
            {
                //TODO Log
                codigo = Codigo.ERROR_BD;
                Console.WriteLine(ex.StackTrace + ex.Message);
            }

            return codigo;
        }

        public (Codigo, List<CondicionCredito>) ObtenerCondicionesCreditoActivas()
        {
            List<CondicionCredito> condicionesCredito = null;
            Codigo codigo = Codigo.EXITO;

            try
            {
                using (FinancieraBD contexto = new FinancieraBD())
                {
                    List<CondicionCredito> condicionesCreditoRecuperadas = contexto.CondicionCredito
                        .Where(condicion => condicion.estaActiva)
                        .ToList();

                    if (condicionesCreditoRecuperadas.Count != 0)
                    {
                        condicionesCredito = new List<CondicionCredito>();
                        foreach (CondicionCredito condicionCredito in condicionesCreditoRecuperadas)
                        {
                            CondicionCredito condicionCreditoNueva = new CondicionCredito
                            {
                                idCondicionCredito = condicionCredito.idCondicionCredito,
                                descripcion = condicionCredito.descripcion,
                                plazoMeses = condicionCredito.plazoMeses,
                                tasaInteres = condicionCredito.tasaInteres,
                                tieneIVA = condicionCredito.tieneIVA
                            };

                            condicionesCredito.Add(condicionCreditoNueva);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                //TODO Log
                codigo = Codigo.ERROR_BD;
                Console.WriteLine(ex);
            }
            catch (EntityException ex)
            {
                //TODO Log
                codigo = Codigo.ERROR_BD;
                Console.WriteLine(ex);
            }
            return (codigo, condicionesCredito);
        }
    }
}
