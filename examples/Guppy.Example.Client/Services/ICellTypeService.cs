using Guppy.Example.Client.Entities;

namespace Guppy.Example.Client.Services
{
    public interface ICellTypeService
    {
        void Update(ref CellPair cell, Grid input, Grid output);
    }
}