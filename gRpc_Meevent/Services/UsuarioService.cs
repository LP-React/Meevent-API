using Grpc.Core;
using gRpc_Meevent.Protos.Usuario;
using Microsoft.Data.SqlClient;
using System.Data;

namespace gRpc_Meevent.Services
{
    public class UsuarioService : ServiceUsuario.ServiceUsuarioBase
    {
        private readonly ILogger<CategoriaService> _logger;

        public UsuarioService(ILogger<CategoriaService> logger)
        {
            _logger = logger;
        }

        string cadena = "server=.;database=MeeventDB; trusted_connection=true; MultipleActiveResultSets=true; TrustServerCertificate=true";

        List<Usuario> Lista()
        {
            List<Usuario> temporal = new();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_Listar_Usuarios_grpc", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new Usuario
                    {
                        IdUsuario = dr.GetInt32(0),
                        NombreCompleto = dr.GetString(1),
                        CorreoElectronico = dr.GetString(2),
                        NumeroTelefono = dr.IsDBNull(3) ? "" : dr.GetString(3),
                        CuentaActiva = dr.GetBoolean(4),
                        TipoUsuario = dr.GetString(5)
                    });
                }
            }
            return temporal;
        }

        public override Task<Usuarios> GetAll(EmptyUsuario request, ServerCallContext context)
        {
            Usuarios response = new();
            response.Items.AddRange(Lista());
            return Task.FromResult(response);
        }

        public override Task<Usuarios> GetById(UsuarioRequest request, ServerCallContext context)
        {
            Usuarios response = new();

            if (request.IdUsuario > 0)
            {
                response.Items.AddRange(
                    Lista().Where(u => u.IdUsuario == request.IdUsuario).ToArray()
                );
            }

            return Task.FromResult(response);
        }

        public override Task<Usuarios> GetByFiltro(UsuarioFiltroRequest request,ServerCallContext context)
        {
            Usuarios response = new();

            using SqlConnection cn = new(cadena);
            cn.Open();

            SqlCommand cmd = new SqlCommand("usp_Listar_Usuarios_grpc", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (string.IsNullOrWhiteSpace(request.Nombre))
                cmd.Parameters.AddWithValue("@nombre", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@nombre", request.Nombre);

            if (request.UsarEstado)
                cmd.Parameters.AddWithValue("@estado", request.Estado);
            else
                cmd.Parameters.AddWithValue("@estado", DBNull.Value);

            if (string.IsNullOrWhiteSpace(request.Tipo))
                cmd.Parameters.AddWithValue("@tipo", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@tipo", request.Tipo);

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                response.Items.Add(new Usuario
                {
                    IdUsuario = dr.GetInt32(0),
                    NombreCompleto = dr.GetString(1),
                    CorreoElectronico = dr.GetString(2),
                    NumeroTelefono = dr.IsDBNull(3) ? "" : dr.GetString(3),
                    CuentaActiva = dr.GetBoolean(4),
                    TipoUsuario = dr.GetString(5)
                });
            }

            return Task.FromResult(response);
        }
  
    }
}
