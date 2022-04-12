using Guppy;
using Guppy.EntityComponent;
using Guppy.Example.Library;
using Microsoft.Extensions.DependencyInjection;

var guppy = new GuppyEngine(
    libraries: new[]
    {
        typeof(ExampleScene).Assembly
    }).ConfigureEntityComponent().ConfigureNetwork(1);

var provider = guppy.BuildServiceProvider();
var scene = provider.GetRequiredService<ExampleScene>();

Console.ReadLine();