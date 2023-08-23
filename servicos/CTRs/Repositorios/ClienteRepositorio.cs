using CTRs.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Threading.Tasks;

namespace CTRs.Repositorios;

public class ClienteRepositorio : IClienteRepositorio
{
    private readonly MinhasConfiguracoes _minhasConfiguracoes;

    public ClienteRepositorio(IOptions<MinhasConfiguracoes> minhasConfiguracoes)
    {
        _minhasConfiguracoes = minhasConfiguracoes.Value;
    }

    public async Task<bool> SalvarNumeroCTRComErro(int id, string mensagem)
    {
        try
        {
            var str = _minhasConfiguracoes.SqlConnString;

            using (SqlConnection conn = new(str))
            {
                conn.Open();
                var query = @$"UPDATE Eventos
                               SET [Status] = 2, Mensagem = @mensagem, Quando = GETDATE()
                               WHERE Id = @Id";

                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int);
                    cmd.Parameters["@Id"].Value = id;
                    cmd.Parameters.AddWithValue("@mensagem", mensagem);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> SalvarNumeroCTRComSucesso(int id, string mensagem, int pedidoId, string numeroCtr)
    {
        try
        {
            var str = _minhasConfiguracoes.SqlConnString;

            using (SqlConnection conn = new(str))
            {
                conn.Open();
                var query = @$"UPDATE Eventos
                               SET [Status] = 0, Mensagem = @mensagem, Quando = GETDATE()
                               WHERE Id = @Id

                               UPDATE Pedidos
                               SET NumeroCTR = @numeroCtr
                               WHERE Id = @pedidoId";

                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int);
                    cmd.Parameters["@Id"].Value = id;
                    cmd.Parameters.AddWithValue("@mensagem", mensagem);
                    cmd.Parameters.Add("@pedidoId", SqlDbType.Int);
                    cmd.Parameters["@pedidoId"].Value = pedidoId;
                    cmd.Parameters.AddWithValue("@numeroCtr", numeroCtr);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> SetStatusAguardandoParaRecolherCacamba(int id, string mensagem)
    {
        try
        {
            var str = _minhasConfiguracoes.SqlConnString;

            using (SqlConnection conn = new(str))
            {
                conn.Open();
                var query = @$"UPDATE Eventos
                               SET [Status] = 1, Mensagem = @mensagem, Quando = GETDATE()
                               WHERE Id = @Id";

                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int);
                    cmd.Parameters["@Id"].Value = id;
                    cmd.Parameters.AddWithValue("@mensagem", mensagem);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> SetStatusConcluidoParaRecolherCacambaEPedidoConcluido(int recolherItemid, int pedidoConcluidoId, int cacambaId, string mensagem)
    {
        try
        {
            var str = _minhasConfiguracoes.SqlConnString;

            using (SqlConnection conn = new(str))
            {
                conn.Open();
                var query = @$"UPDATE Eventos
                               SET [Status] = 0, Mensagem = @mensagem, Quando = GETDATE()
                               WHERE Id = @recolherItemid
                                
                               UPDATE Eventos
                               SET [Status] = 0, Mensagem = @mensagem, Quando = GETDATE()
                               WHERE Id = @pedidoConcluidoId

                               UPDATE Cacambas
                               SET [Status] = 1
                               WHERE Id = @cacambaId";

                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.Add("@recolherItemid", SqlDbType.Int);
                    cmd.Parameters["@recolherItemid"].Value = recolherItemid;
                    cmd.Parameters.Add("@pedidoConcluidoId", SqlDbType.Int);
                    cmd.Parameters["@pedidoConcluidoId"].Value = pedidoConcluidoId;
                    cmd.Parameters.Add("@cacambaId", SqlDbType.Int);
                    cmd.Parameters["@cacambaId"].Value = cacambaId;
                    cmd.Parameters.AddWithValue("@mensagem", mensagem);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
    private static string GetEnvironmentVariable(string nome)
    {
        return nome + ": " +
            System.Environment.GetEnvironmentVariable(nome, EnvironmentVariableTarget.Process);
    }
}
