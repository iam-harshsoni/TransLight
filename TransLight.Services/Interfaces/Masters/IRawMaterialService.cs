using TransLight.DataAccess.Models;

namespace TransLight.Services.Interfaces.Masters
{
    public interface IRawMaterialService : IBaseService<RawMaterial>
    {
        void Update(RawMaterial obj);
    }
}
