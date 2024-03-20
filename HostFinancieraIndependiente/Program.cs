using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ServidorFinancieraIndependiente;

namespace HostFinancieraIndependiente
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (ServiceHost host = new ServiceHost(typeof(ServidorFinancieraIndependiente.ServiciosFinancieraIndependiente)))
                {
                    host.Open();
                    Console.WriteLine("Servidor en línea");
                    Console.ReadLine();
                }
            }
            catch (AddressAccessDeniedException ex)
            {
                Console.WriteLine("No se cuentan con los permisos necesarios en el servidor", ex.ToString());
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error inesperado ", ex.ToString());
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }
    }
}
