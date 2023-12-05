using Guppy.Common;
using Guppy.Resources;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Game.ImGui
{
    public interface IImguiBatch
    {
        bool Running { get; }

        /// <summary>
        /// Sets up ImGui for a new frame, should be called at frame start
        /// </summary>
        void Begin(GameTime gameTime);

        /// <summary>
        /// Asks ImGui for the generated geometry data and sends it to the graphics pipeline, should be called after the UI is drawn using ImGui.** calls
        /// </summary>
        void End();

        Ref<ImFontPtr> GetFont(Resource<TrueTypeFont> ttf, int size);
    }
}
