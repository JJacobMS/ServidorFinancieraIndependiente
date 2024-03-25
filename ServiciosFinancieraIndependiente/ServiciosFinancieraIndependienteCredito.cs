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
                    bool existeEstatusPendiente = contexto.EstatusCredito.Any(e => e.idEstatusCredito == ID_ESTATUS_PENDIENTE);

                    if (!existeEstatusPendiente)
                    {
                        return Codigo.ERROR_BD;
                    }

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


        public (Codigo, SolicitudCredito[]) ObtenerSolicitudesCredito()
        {
            List<SolicitudCredito> solicitudes = null;
            Codigo codigo = Codigo.EXITO;

            try
            {
                using (FinancieraBD contexto = new FinancieraBD())
                {
                    bool existeEstatusPendiente = contexto.EstatusCredito.Any(e => e.idEstatusCredito == ID_ESTATUS_PENDIENTE);

                    if (!existeEstatusPendiente)
                    {
                        return (Codigo.ERROR_BD, null);
                    }

                    solicitudes = (from c in contexto.Credito
                                   join cl in contexto.Cliente on c.Cliente_idCliente equals cl.idCliente
                                   where c.EstatusCredito_idEstatusCredito == ID_ESTATUS_PENDIENTE
                                   select new SolicitudCredito
                                   {
                                       FolioCredito = c.folioCredito,
                                       RfcCliente = cl.rfc,
                                       Nombres = cl.nombres,
                                       Apellidos = cl.apellidos,
                                       Monto = c.monto,
                                       TiempoSolicitud = c.fechaSolicitud
                                   }).ToList();
                }
            }
            catch (SqlException ex)
            {
                codigo = Codigo.ERROR_BD;
                Console.WriteLine(ex.StackTrace + ex.Message);
            }
            catch (EntityException ex)
            {
                codigo = Codigo.ERROR_BD;
                Console.WriteLine(ex.StackTrace + ex.Message);
            }

            return (codigo, solicitudes?.ToArray());
        }

    }
}