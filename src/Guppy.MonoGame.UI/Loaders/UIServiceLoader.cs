using Guppy.MonoGame.Services;
using Guppy.MonoGame.UI.Constants;
using Guppy.Loaders;
using ImGuiNET;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.MonoGame.UI.Services;
using Guppy.Resources.Providers;
using Guppy.MonoGame.UI.Providers.ResourcePackTypeProviders;
using System.Reflection;
using Guppy.Common.Helpers;
using System.Runtime.InteropServices;
using Guppy.MonoGame.UI.Messages.Inputs;
using Guppy.Common;
using Microsoft.Xna.Framework;

namespace Guppy.MonoGame.UI.Loaders
{
    internal sealed class UIServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Load the cimgui natives
            var directory = Path.Combine(Directory.GetCurrentDirectory(), PathConstants.Natives);
            NativeHelper.Load(directory, FileConstants.cImGui, FileConstants.cImPlot);


            services.AddTransient<IResourcePackTypeProvider, ResourcePackTrueTypeFontProvider>();

            services.AddResource<TrueTypeFont>(ResourceConstants.DiagnosticsTTF, FileConstants.DiagnosticsTTF);

            services.AddImGuiFont(ImGuiFontConstants.DiagnosticsFont, ResourceConstants.DiagnosticsTTF, 18);
            services.AddImGuiFont(ImGuiFontConstants.DiagnosticsFontHeader, ResourceConstants.DiagnosticsTTF, 24);

            services.AddScoped<ImGuiDebuggerService>()
                .AddAliases(Alias.ManyFrom<ImGuiDebuggerService>(typeof(IGameComponent), typeof(IDebuggerService)));

            services.AddScoped<ImGuiTerminalService>()
                .AddAliases(Alias.ManyFrom<ImGuiTerminalService>(typeof(IGameComponent), typeof(ITerminalService)));

            services.AddTransient<ImGuiBatch>();

            services.AddCommand<Definitions.CommandDefinitions.UI.Key>();

            AddImGuiInputMap(services, InputConstants.UI_Tab, Keys.Tab, ImGuiKey.Tab);
            AddImGuiInputMap(services, InputConstants.UI_LeftArrow, Keys.Left, ImGuiKey.LeftArrow);
            AddImGuiInputMap(services, InputConstants.UI_RightArrow, Keys.Right, ImGuiKey.RightArrow);
            AddImGuiInputMap(services, InputConstants.UI_UpArrow, Keys.Up, ImGuiKey.UpArrow);
            AddImGuiInputMap(services, InputConstants.UI_DownArrow, Keys.Down, ImGuiKey.DownArrow);
            AddImGuiInputMap(services, InputConstants.UI_PageUp, Keys.PageUp, ImGuiKey.PageUp);
            AddImGuiInputMap(services, InputConstants.UI_PageDown, Keys.PageDown, ImGuiKey.PageDown);
            AddImGuiInputMap(services, InputConstants.UI_Home, Keys.Home, ImGuiKey.Home);
            AddImGuiInputMap(services, InputConstants.UI_End, Keys.End, ImGuiKey.End);
            AddImGuiInputMap(services, InputConstants.UI_Delete, Keys.Delete, ImGuiKey.Delete);
            AddImGuiInputMap(services, InputConstants.UI_Backspace, Keys.Back, ImGuiKey.Backspace);
            AddImGuiInputMap(services, InputConstants.UI_Enter, Keys.Enter, ImGuiKey.Enter);
            AddImGuiInputMap(services, InputConstants.UI_Escape, Keys.Escape, ImGuiKey.Escape);
            AddImGuiInputMap(services, InputConstants.UI_Space, Keys.Space, ImGuiKey.Space);
            AddImGuiInputMap(services, InputConstants.UI_A, Keys.A, ImGuiKey.A);
            AddImGuiInputMap(services, InputConstants.UI_C, Keys.C, ImGuiKey.C);
            AddImGuiInputMap(services, InputConstants.UI_V, Keys.V, ImGuiKey.V);
            AddImGuiInputMap(services, InputConstants.UI_X, Keys.X, ImGuiKey.X);
            AddImGuiInputMap(services, InputConstants.UI_Y, Keys.Y, ImGuiKey.Y);
            AddImGuiInputMap(services, InputConstants.UI_Z, Keys.Z, ImGuiKey.Z);
        }

        private static void AddImGuiInputMap(IServiceCollection services, string key, Keys defaultKey, ImGuiKey mapping)
        {
            services.AddInput(
                key, 
                defaultKey,
                new[]
                {
                    (ButtonState.Pressed, new ImGuiKeyStateInput(mapping, ButtonState.Pressed)),
                    (ButtonState.Released, new ImGuiKeyStateInput(mapping, ButtonState.Released))
                });
        }
    }
}
