using DatosFinancieraIndependiente;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace ServidorFinancieraIndependiente
{
    public partial class ServiciosFinancieraIndependiente : IUsuario
    {
        public (Usuario, Codigo) ValidarUsuario(string correo, string contrasena)
        {
            Usuario usuario = null;
            Codigo codigo = Codigo.EXITO;
            try
            {
                using (FinancieraBD contexto = new FinancieraBD())
                {
                    Usuario usuarioConCorreoExistente = contexto.Database.SqlQuery<Usuario>
                        ("SELECT * FROM USUARIO WHERE correoElectronico = @correo", new SqlParameter("@correo", correo))
                        .FirstOrDefault();

                    if (usuarioConCorreoExistente != null)
                    {
                        Usuario usuarioVerificado = contexto.Database.SqlQuery<Usuario>
                            ("SELECT * From Usuario WHERE HASHBYTES('SHA2_512', @contrasena) = contrasenha AND correoElectronico = @correo",
                            new SqlParameter("@contrasena", contrasena), new SqlParameter("@correo", correo))
                            .FirstOrDefault();

                        TipoUsuario tipoUsuario = contexto.Database.SqlQuery<TipoUsuario>
                                ("SELECT * FROM TipoUsuario WHERE @tipo = idTipoUsuario", new SqlParameter("@tipo", usuarioConCorreoExistente.TipoUsuario_idTipoUsuario)).FirstOrDefault();
                        usuario = new Usuario
                        {
                            correoElectronico = usuarioConCorreoExistente.correoElectronico,
                            idUsuario = 0
                        };

                        if (usuarioVerificado != null && tipoUsuario != null)
                        {
                            TipoUsuario tipoVerificado = new TipoUsuario { idTipoUsuario = tipoUsuario.idTipoUsuario, nombre = tipoUsuario.nombre};
                            usuario.idUsuario = usuarioVerificado.idUsuario;
                            usuario.TipoUsuario = tipoVerificado;
                            usuario.nombres = usuarioVerificado.nombres;
                            usuario.apellidos = usuarioVerificado.apellidos;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                codigo = Codigo.ERROR_BD;
                Console.WriteLine(ex);
            }
            catch (EntityException ex)
            {
                codigo = Codigo.ERROR_BD;
                Console.WriteLine(ex);
            }
            return (usuario, codigo);                
        }
    }
}
