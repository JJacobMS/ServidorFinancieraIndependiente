using DatosFinancieraIndependiente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServidorFinancieraIndependiente
{
    [ServiceContract]
    public interface ICliente
    {
        [OperationContract]
        (bool, Codigo) ValidarRfcClienteUnico(string rfcCliente);
        [OperationContract]
        Codigo GuardarInformacionCliente(Cliente cliente, ReferenciaTrabajo referenciaTrabajo, ReferenciaCliente[] referenciaCliente, Documento[] documentos);
    }
}
