using Guppy;
using Guppy.EntityComponent;
using Guppy.Example.Library;
using Guppy.Example.Server;
using Guppy.Gaming;
using Microsoft.Extensions.DependencyInjection;

var game = new GuppyEngine(
    libraries: new[]
    {
        typeof(ExampleScene).Assembly
    })
    .ConfigureThreading()
    .ConfigureNetwork(channelsCount: 1)
    .ConfigureGame<ServerExampleGame>()
    .BuildServiceProvider()
    .GetRequiredService<ServerExampleGame>();

var token = new CancellationTokenSource();
game.StartAsync(token.Token);

Console.ReadLine();