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
    public interface IClienteRFC
    {
        [OperationContract]
        (Codigo, Cliente) BuscarClientePorRFC(string rfc);
    }

    [DataContract]
    public class Cliente
    {
        [DataMember]
        public int IdCliente { get; set; }
        [DataMember]
        public string Nombres { get; set; }
        [DataMember]
        public string Apellidos { get; set; }
        [DataMember]
        public string Rfc { get; set; }
        [DataMember]
        public bool EsDeudor { get; set; }
        [DataMember]
        public string CorreoElectronico { get; set; }
        [DataMember]
        public string CuentaDeposito { get; set; }
        [DataMember]
        public string CuentaCobro { get; set; }
        [DataMember]
        public string Direccion { get; set; }
        [DataMember]
        public List<string> Telefonos { get; set; }
    }
}
