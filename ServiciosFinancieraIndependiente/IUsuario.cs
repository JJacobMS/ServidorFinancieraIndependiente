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
    public interface IUsuario
    {
        [OperationContract]
        (Usuario, Codigo) ValidarUsuario(string correo, string contrasena);
    }
}
