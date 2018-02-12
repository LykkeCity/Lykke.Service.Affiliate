using System.Threading.Tasks;

namespace Lykke.Service.Affiliate.Core.Services
{
    public interface IShutdownManager
    {
        Task StopAsync();
    }
}