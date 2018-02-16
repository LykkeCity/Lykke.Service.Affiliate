using System.Threading.Tasks;

namespace Lykke.Service.Affiliate.Core.Services.Managers
{
    public interface IShutdownManager
    {
        Task StopAsync();
    }
}
