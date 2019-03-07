using Microsoft.Xna.Framework;
using System;
using System.Threading;

namespace Pong.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerPongGame _pongGame = new ServerPongGame();
            DateTime _lastFrame = DateTime.Now;
            DateTime _frameStart = DateTime.Now;
            DateTime _start = DateTime.Now;

            _pongGame.Start();
            Thread.Sleep(16);

            while(true)
            {
                _frameStart = DateTime.Now;
                _pongGame.Update(new GameTime(_frameStart - _start, _frameStart - _lastFrame));
                _lastFrame = _frameStart;

                Thread.Sleep(16);
            }
        }
    }
}
