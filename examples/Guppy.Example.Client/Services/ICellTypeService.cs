using Guppy.Example.Client.Entities;

namespace Guppy.Example.Client.Services
{
    public interface ICellTypeService
    {
        void Update(ref Cell cell, Grid old, Grid output);
    }
}