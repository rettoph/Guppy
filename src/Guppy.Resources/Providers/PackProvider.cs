using Guppy.Resources.Definitions;
using Guppy.Resources.Loaders;
using Guppy.Resources.Serialization.Json;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Guppy.Resources.Providers
{
    internal sealed class PackProvider : IPackProvider
    {
        public const string ConfigurationFile = "pack.json";

        private IServiceProvider _provider;
        private IJsonSerializer _json;
        private IDictionary<Guid, Pack> _packs;

        public PackProvider(
            IServiceProvider provider,
            IJsonSerializer json,
            IEnumerable<Pack> packs,
            IEnumerable<IPackLoader> loaders)
        {
            _provider = provider;
            _json = json;
            _packs = packs.ToDictionary(x => x.Id, x => x);

            foreach(var loader in loaders)
            {
                loader.Load(this);
            }

            foreach(Pack pack in _packs.Values)
            {
                pack.Initialize(_provider);
            }
        }

        public IEnumerable<Pack> GetAll()
        {
            return _packs.Values;
        }

        public Pack GetById(Guid id)
        {
            return _packs[id];
        }

        // public IPack Import(string path)
        // {
        //     var configurationFile = Path.Combine(path, ConfigurationFile);
        // 
        //     if (!File.Exists(configurationFile))
        //     {
        //         throw new FileNotFoundException(configurationFile);
        //     }
        // 
        //     using (var configurationStream = File.OpenRead(configurationFile))
        //     {
        //         var manifest = _json.Deserialize<PackDefinition>(configurationStream);
        // 
        //         if (manifest is null)
        //         {
        //             throw new Exception();
        //         }
        // 
        //         return this.Create(manifest);
        //     }
        // }
    }
}
