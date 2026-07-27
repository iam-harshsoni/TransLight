using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransLight.DataAccess.Models;

namespace TransLight.Services.Interfaces.Masters
{
    public interface IUnitService : IBaseService<Unit>
    {
        void Update(Unit obj);
    }
}
