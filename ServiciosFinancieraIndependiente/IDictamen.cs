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
    public interface IDictamen
    {
        [OperationContract]
        Codigo GuardarDictamen(Dictamen dictamen);

        [OperationContract]
        (Codigo, List<Politica>) RecuperarPoliticasChecklist(int folioCredito);
    }
}
