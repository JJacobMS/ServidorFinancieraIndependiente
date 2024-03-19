using DatosFinancieraIndependiente;
using ServidorFinancieraIndependiente;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PruebasFinancieraIndependiente
{
    public class ConfiguracionBDRecuperarRegistros : IDisposable
    {
        public TipoUsuario TipoNuevo { get; set; }
        public ConfiguracionBDRecuperarRegistros()
        {
            try
            {
                using (var contexto = new FinancieraBD())
                {
                    TipoNuevo = contexto.TipoUsuario.Add(new TipoUsuario { nombre = "Nuevo Tipo Usuario" });
                    contexto.SaveChanges();

                    contexto.Database.ExecuteSqlCommand("INSERT Usuario VALUES ('usuarioPrueba@gmail', 'Nombre del Usuario Prueba', 'Apellidos del Usuario Prueba' " +
                        ", HASHBYTES('SHA2_512', N'12345'), @tipo)", new SqlParameter("@tipo", TipoNuevo.idTipoUsuario));
                    
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
            }
            catch (EntityException ex)
            {
                Console.WriteLine(ex);
            }
        }
        public void Dispose()
        {
            try
            {
                using (var contexto = new FinancieraBD())
                {
                    contexto.Database.ExecuteSqlCommand("DELETE Usuario WHERE correoElectronico = 'usuarioPrueba@gmail'");
                    contexto.Database.ExecuteSqlCommand("DELETE TipoUsuario WHERE idTipoUsuario = @id", new SqlParameter("@id", TipoNuevo.idTipoUsuario));
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
            }
            catch (EntityException ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
    public class RecuperarConExitoRegistros : IClassFixture<ConfiguracionBDRecuperarRegistros>
    {
        private ConfiguracionBDRecuperarRegistros _configuracion;
        public RecuperarConExitoRegistros(ConfiguracionBDRecuperarRegistros configuracion)
        {
            _configuracion = configuracion;
        }
        [Fact]
        public void VerificarUsuarioConCorreoContrasenaValidos()
        {
            Codigo codigoEsperado = Codigo.EXITO;

            Usuario usuarioEsperado = new Usuario { correoElectronico = "usuarioPrueba@gmail", nombres = "Nombre del Usuario Prueba", 
                apellidos = "Apellidos del Usuario Prueba"
            };

            string correoExistente = "usuarioPrueba@gmail";
            string contrasenaCorrecta = "12345";

            ServiciosFinancieraIndependiente servicioComunicacion = new ServiciosFinancieraIndependiente();
            (Usuario usuarioResultante, Codigo codigoResultante) = servicioComunicacion.ValidarUsuario(correoExistente, contrasenaCorrecta);

            Assert.Equal(codigoEsperado, codigoResultante);
            Assert.Equal(usuarioEsperado, usuarioResultante);

        }
    }
}
