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
    public interface IPoliticaOtorgamiento
    {
        [OperationContract]
        Codigo GuardarPoliticaOtorgamiento(Politica politica);

        [OperationContract]
        (Codigo, List<Politica>) RecuperarPoliticas();
    }
}
