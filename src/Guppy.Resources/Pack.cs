using Guppy.Resources.Constants;
using Guppy.Resources.Definitions;
using Guppy.Resources.Serialization.Json;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources
{
    public sealed class Pack
    {
        public const string EntryFile = "pack.json";

        private IServiceProvider _provider;
        private ISetting<string> _localization;

        internal readonly IDictionary<string, IResourceCollection> localized;

        public Guid Id { get; }
        public string Name { get; private set; }
        public string? Directory { get; set; }
        public IEnumerable<IResource> Resources => this.localized.Values.SelectMany(l => l.Items);
        public bool Initialized { get; private set; }
        public DateTime Loaded { get; private set; }

        internal Pack(Guid id, string name, Dictionary<string, IResourceCollection> localized)
        {
            this.localized = localized;
            _provider = default!;
            _localization = default!;

            this.Id = id;
            this.Name = name;
            this.Directory = default!;
            this.Loaded = DateTime.MinValue;
            this.Initialized = false;
        }
        public Pack(Guid id, string name) : this(id, name, new Dictionary<string, IResourceCollection>())
        {

        }

        public void Initialize(IServiceProvider provider)
        {
            _provider = provider;
            _localization = provider.GetSetting<string>(SettingConstants.CurrentLanguage);

            this.Reload();

            this.Initialized = true;
        }

        public void Import(string? directory = null)
        {
            this.Directory = directory ?? this.Directory;

            if (this.Directory is null)
            {
                throw new ArgumentException(nameof(directory));
            }

            string path = Path.Combine(this.Directory, EntryFile);
            System.IO.Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            using (var file = File.OpenRead(path))
            {
                var pack = _provider.GetRequiredService<IJsonSerializer>().Deserialize<Pack>(file)!;
                this.Import(pack);
            }
        }

        public void Import(Pack pack)
        {
            if(pack.Id != this.Id)
            {
                throw new InvalidOperationException();
            }


            this.Name = pack.Name;
            this.localized.Clear();

            foreach((string localization, IResourceCollection resources) in pack.localized)
            {
                this.localized.Add(localization, resources);
            }

            this.Reload();
        }

        public void Reload(string? directory = null)
        {
            this.Directory = directory ?? this.Directory;

            if(this.Directory is null)
            {
                return;
            }

            foreach(var resource in this.Resources)
            {
                resource.Initialize(this.Directory, _provider);
            }

            this.Loaded = DateTime.Now;
        }

        public Pack Add(string localization, params IResource[] resources)
        {
            if(!this.localized.TryGetValue(localization, out IResourceCollection? localized))
            {
                localized = new ResourceCollection();
                this.localized.Add(localization, localized);
            }

            foreach(var resource in resources)
            {
                localized.Add(resource);

                if(this.Directory is not null)
                {
                    resource.Initialize(this.Directory, _provider);
                }
            }

            return this;
        }

        public Pack Add(params IResource[] resources)
        {
            return this.Add(LanguageConstants.Default, resources);
        }

        public bool TryGet<T>(string name, [MaybeNullWhen(false)] out IResource<T> resource)
        {
            if (
                this.localized.TryGetValue(_localization.Value, out var localized) && 
                localized.TryGet<T>(name, out resource)
            )
            {
                return true;
            }

            if (
                _localization.Value != LanguageConstants.Default &&
                this.localized.TryGetValue(LanguageConstants.Default, out localized) && 
                localized.TryGet<T>(name, out resource)
            )
            {
                return true;
            }

            resource = null;
            return false;
        }

        public void Export(string? directory = null)
        {
            this.Directory = directory ?? this.Directory;

            if(this.Directory is null)
            {
                throw new ArgumentException(nameof(directory));
            }

            string path = Path.Combine(this.Directory, EntryFile);
            System.IO.Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            using (var file = File.OpenWrite(path))
            {
                _provider.GetRequiredService<IJsonSerializer>().Serialize(file, this);
            }

            foreach(IResourceCollection resources in this.localized.Values)
            {
                foreach(IResource resource in resources.Items)
                {
                    resource.Export(this.Directory, _provider);
                }
            }
        }
    }
}
