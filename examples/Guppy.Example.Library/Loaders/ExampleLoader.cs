using Guppy.Attributes;
using Guppy.Example.Library.Components;
using Guppy.Example.Library.Constants;
using Guppy.Loaders;
using Guppy.Network;
using Guppy.Network.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library.Loaders
{
    [AutoLoad]
    internal sealed class ExampleLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGuppy<ExampleGuppy>();

            services.AddResourcePack("default", "Content/Default");

            services.AddColorResource(ColorConstants.ShipColor1);
            services.AddColorResource(ColorConstants.ShipColor2);
            services.AddColorResource(ColorConstants.ShipColor3);

            services.ConfigureEntity(EntityConstants.Ship, EntityConstants.Tags.Ship, EntityConstants.Tags.Movable);

            services.AddComponent<User>(p => User.DefaultUser, EntityConstants.Tags.Ship);
            services.AddComponent<Movable>(p => new Movable(), EntityConstants.Tags.Movable);
        }
    }
}
