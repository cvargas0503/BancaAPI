using BancaAPI.Domain.Entities;
using System.Threading.Tasks;

namespace BancaAPI.Application.Interfaces.Repositories
{
    public interface IClientRepository
    {
        Task CrearAsync(Cliente cliente);
    }
}
