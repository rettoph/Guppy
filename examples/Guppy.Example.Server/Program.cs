using Guppy;
using Guppy.Example.Library;
using Guppy.Example.Server;
using Guppy.MonoGame.Helpers;
using Microsoft.Extensions.DependencyInjection;

var guppy = new GuppyEngine(
    libraries: new[]
    {
        typeof(ExampleGuppy).Assembly
    })
    .ConfigureGame()
    .ConfigureNetwork(1)
    .Build()
    .Create<ServerExampleGuppy>();

var token = new CancellationTokenSource();
TaskHelper.CreateLoop(gt =>
{
    guppy.Update(gt);
}, 16, token.Token).GetAwaiter().GetResult();