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
    public interface IChecklist
    {
        [OperationContract]
        (Codigo, String) RecuperarChecklist(int folioCredito);
        [OperationContract]
        Codigo GuardarChecklist(Checklist checklist, int[] listaIdPoliticas); 
    }
}
