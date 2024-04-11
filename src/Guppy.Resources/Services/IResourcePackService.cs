﻿namespace Guppy.Resources.Services
{
    public interface IResourcePackService
    {
        void Initialize();
        IEnumerable<ResourcePack> GetAll();
        ResourcePack GetById(Guid id);
    }
}
