using Guppy.Gaming.Content;
using Guppy.Gaming.Services;
using Guppy.Gaming.UI.Constants;
using Guppy.Gaming.UI.Providers;
using Guppy.Gaming.UI.Services;
using Guppy.Gaming.UI.Structs;
using Guppy.Loaders;
using ImGuiNET;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.UI.Loaders
{
    internal sealed class UIServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddContent<TrueTypeFont>(ContentConstants.DiagnosticsTTF, "Fonts/DiagnosticsFont");

            services.AddFont(FontConstants.DiagnosticsFont, ContentConstants.DiagnosticsTTF, 18);

            services.AddSingleton<ITerminalService, TerminalService>();
            services.AddSingleton<ImGuiBatch>();

            services.AddCommand<Definitions.CommandDefinitions.UI.Key>();

            this.AddImGuiInputMap(services, InputConstants.UI_Tab, Keys.Tab, ImGuiKey.Tab);
            this.AddImGuiInputMap(services, InputConstants.UI_LeftArrow, Keys.Left, ImGuiKey.LeftArrow);
            this.AddImGuiInputMap(services, InputConstants.UI_RightArrow, Keys.Right, ImGuiKey.RightArrow);
            this.AddImGuiInputMap(services, InputConstants.UI_UpArrow, Keys.Up, ImGuiKey.UpArrow);
            this.AddImGuiInputMap(services, InputConstants.UI_DownArrow, Keys.Down, ImGuiKey.DownArrow);
            this.AddImGuiInputMap(services, InputConstants.UI_PageUp, Keys.PageUp, ImGuiKey.PageUp);
            this.AddImGuiInputMap(services, InputConstants.UI_PageDown, Keys.PageDown, ImGuiKey.PageDown);
            this.AddImGuiInputMap(services, InputConstants.UI_Home, Keys.Home, ImGuiKey.Home);
            this.AddImGuiInputMap(services, InputConstants.UI_End, Keys.End, ImGuiKey.End);
            this.AddImGuiInputMap(services, InputConstants.UI_Delete, Keys.Delete, ImGuiKey.Delete);
            this.AddImGuiInputMap(services, InputConstants.UI_Backspace, Keys.Back, ImGuiKey.Backspace);
            this.AddImGuiInputMap(services, InputConstants.UI_Enter, Keys.Enter, ImGuiKey.Enter);
            this.AddImGuiInputMap(services, InputConstants.UI_Escape, Keys.Escape, ImGuiKey.Escape);
            this.AddImGuiInputMap(services, InputConstants.UI_Space, Keys.Space, ImGuiKey.Space);
            this.AddImGuiInputMap(services, InputConstants.UI_A, Keys.A, ImGuiKey.A);
            this.AddImGuiInputMap(services, InputConstants.UI_C, Keys.C, ImGuiKey.C);
            this.AddImGuiInputMap(services, InputConstants.UI_V, Keys.V, ImGuiKey.V);
            this.AddImGuiInputMap(services, InputConstants.UI_X, Keys.X, ImGuiKey.X);
            this.AddImGuiInputMap(services, InputConstants.UI_Y, Keys.Y, ImGuiKey.Y);
            this.AddImGuiInputMap(services, InputConstants.UI_Z, Keys.Z, ImGuiKey.Z);
        }

        private void AddImGuiInputMap(IServiceCollection services, string key, Keys defaultKey, ImGuiKey mapping)
        {
            services.AddInput(
                key, 
                defaultKey, 
                p => new ImGuiKeyState(mapping, ButtonState.Pressed), 
                p => new ImGuiKeyState(mapping, ButtonState.Released));
        }
    }
}
