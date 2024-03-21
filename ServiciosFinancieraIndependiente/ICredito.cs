using DatosFinancieraIndependiente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServidorFinancieraIndependiente
{
    [ServiceContract]
    public interface ICredito
    {
        [OperationContract]
        Codigo GuardarInformacionSolicitud(Credito credito);

        [OperationContract]
        (Codigo, SolicitudCredito[]) ObtenerSolicitudesCredito();
    }

    [DataContract]
    public class SolicitudCredito
    {
        [DataMember]
        public int FolioCredito { get; set; }
        [DataMember]
        public string RfcCliente { get; set; }
        [DataMember]
        public string Nombres { get; set; }
        [DataMember]
        public string Apellidos { get; set; }
        [DataMember]
        public double Monto { get;set; }
        [DataMember]
        public DateTime TiempoSolicitud { get; set; }
    }
}
