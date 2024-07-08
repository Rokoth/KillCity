using System.Threading.Tasks;

namespace KillCity.KillCityDeployer
{
    public interface IDeployService
    {
        Task Deploy(int? num = null);
    }
}
