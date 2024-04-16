﻿using Guppy.Engine;
using Guppy.Example.Client;
using Guppy.Game;

using (var game = new GuppyMonoGame<MainGuppy>(new GuppyContext("rettoph", "example")))
    game.Run();