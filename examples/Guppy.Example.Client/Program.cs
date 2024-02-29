using Guppy.Example.Client;
using Guppy.Game.MonoGame;

using (var game = new GuppyMonoGame<MainGuppy>("rettoph", "example"))
    game.Run();