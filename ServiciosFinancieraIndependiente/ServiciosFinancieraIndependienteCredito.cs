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
    public partial class ServiciosFinancieraIndependiente : ICredito
    {
        private static readonly int ID_ESTATUS_PENDIENTE = 1;
        private static readonly int ID_ESTATUS_APROBADO = 2;
        private static readonly int ID_ESTATUS_RECHAZADO = 3;

        public Codigo GuardarInformacionSolicitud(Credito credito)
        {
            Codigo codigo = Codigo.EXITO;

            try
            {
                using (FinancieraBD contexto = new FinancieraBD())
                {
                    credito.EstatusCredito_idEstatusCredito = ID_ESTATUS_PENDIENTE;
                    contexto.Credito.Add(credito);
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
    }
}
