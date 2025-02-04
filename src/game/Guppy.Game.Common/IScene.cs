using Guppy.Core.Common;
using Guppy.Core.Common.Services;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Common
{
    public interface IScene
    {
        ulong Id { get; }
        string Name { get; }

        /// <summary>
        /// When false the current scene will not automatically be updated each frame.
        /// Updates must be manually called.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// When false the current scene will not automatically be drawn each frame.
        /// Draws must be manually called.
        /// </summary>
        bool Visible { get; set; }

        event OnEventDelegate<IScene, bool>? OnEnabledChanged;
        event OnEventDelegate<IScene, bool>? OnVisibleChanged;

        IScopedSystemService Systems { get; }

        void Initialize(IGuppyScope scope);

        T Resolve<T>() where T : notnull;

        void Draw(GameTime gameTime);

        void Update(GameTime gameTime);
    }
}