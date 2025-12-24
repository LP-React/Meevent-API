using gRpc_Meevent.Protos;
using Meevent_API.src.Core.Entities;
using Meevent_API.src.Features.CategoriasEvento.DAO;
using Microsoft.Data.SqlClient;

namespace Meevent_API.src.Features.CategoriasEvento.Services
{
    public class CategoriaEventoService : ICategoriaEventoService
    {
        private ServiceCategoria.ServiceCategoriaClient _client;
        private readonly ICategoriaEventoDAO _dao;

        public CategoriaEventoService(ICategoriaEventoDAO dao, ServiceCategoria.ServiceCategoriaClient client)
        {
            _dao = dao;
            _client = client;
        }

        public async Task<CategoriaEventoListResponseDTO> ObtenerCategoriasAsync()
        {
            var lista = await Task.Run(() => _dao.GetCategorias());

            return new CategoriaEventoListResponseDTO
            {
                Exitoso = true,
                Mensaje = "Listado obtenido correctamente",
                TotalCategoriasEvento = lista.Count(),
                CategoriasEvento = lista
            };
        }

        public async Task<CategoriaEventoDTO> ObtenerCategoriaPorIdAsync(int id_categoria_evento)
        {
            var lista = await Task.Run(() => _dao.GetCategoriaPorId(id_categoria_evento));
            return lista.FirstOrDefault();
        }

        public async Task<string> RegistrarCategoriaAsync(CategoriaEventoCrearDTO registro)
        {
            var respuestaGrpc = await _client.GetAllAsync(new Empty());

            bool nombreExiste = respuestaGrpc.Items.Any(c =>
                c.NombreCategoria.Trim().Equals(registro.NombreCategoria.Trim(), StringComparison.OrdinalIgnoreCase)
            );

            if (nombreExiste)
            {
                return "Error: El nombre de la categoría ya está registrado.";
            }

            registro.SlugCategoria = GenerarSlug(registro.NombreCategoria);
            return await Task.Run(() => _dao.InsertCategoria(registro));
        }

        public async Task<CategoriaEventoOperacionResponseDTO> ActualizarCategoriaAsync(int idCategoriaEvento, CategoriaEventoEditarDTO dto)
        {
            var categoriaActual = (await Task.Run(() =>
                _dao.GetCategoriaPorId(idCategoriaEvento)
            )).FirstOrDefault();

            if (categoriaActual == null)
            {
                return new CategoriaEventoOperacionResponseDTO
                {
                    Exitoso = false,
                    Mensaje = "Error: La categoría no existe."
                };
            }

            var respuestaGrpc = await _client.GetAllAsync(new Empty());

            bool nombreExiste = respuestaGrpc.Items.Any(c =>
                c.IdCategoriaEvento != idCategoriaEvento &&
                c.NombreCategoria.Trim().Equals(dto.NombreCategoria.Trim(), StringComparison.OrdinalIgnoreCase)
            );

            if (nombreExiste)
            {
                return new CategoriaEventoOperacionResponseDTO
                {
                    Exitoso = false,
                    Mensaje = "Error: El nombre de la categoría ya está registrado."
                };
            }

            if (!categoriaActual.NombreCategoria.Trim()
                .Equals(dto.NombreCategoria.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                dto.SlugCategoria = GenerarSlug(dto.NombreCategoria);
            }
            else
            {
                dto.SlugCategoria = categoriaActual.SlugCategoria;
            }

            var resultado = await Task.Run(() =>
                _dao.UpdateCategoria(idCategoriaEvento, dto)
            );

            return new CategoriaEventoOperacionResponseDTO
            {
                Exitoso = !resultado.StartsWith("Error"),
                Mensaje = resultado
            };
        }

        public async Task<CategoriaCambiarEstadoResponseDTO> ActivarDesactivarCategoriaAsync(
            int id_categoria_evento,
            bool estado)
        {
            string resultado = await Task.Run(() =>
                _dao.CambiarEstado(id_categoria_evento, estado));

            return new CategoriaCambiarEstadoResponseDTO
            {
                Exitoso = !resultado.Contains("Error"),
                Mensaje = resultado,
                Estado = estado
            };
        }

        private string GenerarSlug(string nombre)   
        {
            if (string.IsNullOrWhiteSpace(nombre)) return string.Empty;
            string slug = nombre.ToLowerInvariant().Trim();
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"\s+", "-").Trim();

            return slug;
        }
    }
}
