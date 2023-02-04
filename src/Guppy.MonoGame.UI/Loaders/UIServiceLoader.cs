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
using System.Reflection;
using Guppy.Common.Helpers;
using System.Runtime.InteropServices;
using Guppy.MonoGame.UI.Messages;
using Guppy.Common;
using Microsoft.Xna.Framework;
using Guppy.MonoGame.UI.Debuggers;
using Guppy.MonoGame.Enums;
using Guppy.MonoGame.Constants;
using InputConstants = Guppy.MonoGame.UI.Constants.InputConstants;
using Guppy.Common.DependencyInjection;
using MonoGame.Extended.Entities.Systems;
using Guppy.MonoGame.UI.Serialization.Json.Converters;
using System.Text.Json.Serialization;
using Guppy.Resources.Definitions;
using Guppy.Resources.Constants;
using Guppy.Resources;
using Guppy.Resources.Loaders;
using Guppy.MonoGame.UI.Providers;

namespace Guppy.MonoGame.UI.Loaders
{
    internal sealed class UIServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Load the cimgui natives
            var directory = Path.Combine(Directory.GetCurrentDirectory(), PathConstants.Natives);
            NativeHelper.Load(directory, FileConstants.cImGui, FileConstants.cImPlot);

            services.AddSingleton<JsonConverter, TrueTypeFontResourceConverter>();

            services.AddSingleton<IPackLoader, PackLoader>();

            services.AddImGuiFont(ImGuiFontConstants.DiagnosticsFont, ResourceConstants.DiagnosticsTTF, 18);
            services.AddImGuiFont(ImGuiFontConstants.DiagnosticsFontHeader, ResourceConstants.DiagnosticsTTF, 20);

            services.AddService<IImguiObjectViewer>()
                .SetLifetime(ServiceLifetime.Singleton)
                .SetImplementationType<ImguiObjectViewer>();

            services.AddService<ImGuiDebuggerService>()
                .SetLifetime(ServiceLifetime.Scoped)
                .AddInterfaceAliases();

            services.AddService<ImGuiTerminalService>()
                .SetLifetime(ServiceLifetime.Scoped)
                .AddInterfaceAliases();

            services.AddService<FpsDebugger>()
                .SetLifetime(ServiceLifetime.Scoped)
                .AddAlias<IDebugger>();

            services.AddTransient<ImGuiBatch>();

            services.AddCommand<Definitions.CommandDefinitions.UI.Key>();

            AddImGuiKeyEvent(services, InputConstants.UI_Tab, Keys.Tab, ImGuiKey.Tab);
            AddImGuiKeyEvent(services, InputConstants.UI_LeftArrow, Keys.Left, ImGuiKey.LeftArrow);
            AddImGuiKeyEvent(services, InputConstants.UI_RightArrow, Keys.Right, ImGuiKey.RightArrow);
            AddImGuiKeyEvent(services, InputConstants.UI_UpArrow, Keys.Up, ImGuiKey.UpArrow);
            AddImGuiKeyEvent(services, InputConstants.UI_DownArrow, Keys.Down, ImGuiKey.DownArrow);
            AddImGuiKeyEvent(services, InputConstants.UI_PageUp, Keys.PageUp, ImGuiKey.PageUp);
            AddImGuiKeyEvent(services, InputConstants.UI_PageDown, Keys.PageDown, ImGuiKey.PageDown);
            AddImGuiKeyEvent(services, InputConstants.UI_Home, Keys.Home, ImGuiKey.Home);
            AddImGuiKeyEvent(services, InputConstants.UI_End, Keys.End, ImGuiKey.End);
            AddImGuiKeyEvent(services, InputConstants.UI_Delete, Keys.Delete, ImGuiKey.Delete);
            AddImGuiKeyEvent(services, InputConstants.UI_Backspace, Keys.Back, ImGuiKey.Backspace);
            AddImGuiKeyEvent(services, InputConstants.UI_Enter, Keys.Enter, ImGuiKey.Enter);
            AddImGuiKeyEvent(services, InputConstants.UI_Escape, Keys.Escape, ImGuiKey.Escape);
            AddImGuiKeyEvent(services, InputConstants.UI_Space, Keys.Space, ImGuiKey.Space);
            AddImGuiKeyEvent(services, InputConstants.UI_A, Keys.A, ImGuiKey.A);
            AddImGuiKeyEvent(services, InputConstants.UI_C, Keys.C, ImGuiKey.C);
            AddImGuiKeyEvent(services, InputConstants.UI_V, Keys.V, ImGuiKey.V);
            AddImGuiKeyEvent(services, InputConstants.UI_X, Keys.X, ImGuiKey.X);
            AddImGuiKeyEvent(services, InputConstants.UI_Y, Keys.Y, ImGuiKey.Y);
            AddImGuiKeyEvent(services, InputConstants.UI_Z, Keys.Z, ImGuiKey.Z);
            AddImGuiKeyEvent(services, InputConstants.UI_LeftShift, Keys.LeftShift, ImGuiKey.LeftShift);
            AddImGuiKeyEvent(services, InputConstants.UI_RightShift, Keys.RightShift, ImGuiKey.RightShift);
            AddImGuiKeyEvent(services, InputConstants.UI_LeftCtrl, Keys.LeftControl, ImGuiKey.LeftCtrl);
            AddImGuiKeyEvent(services, InputConstants.UI_RightCtrl, Keys.RightControl, ImGuiKey.RightCtrl);
            AddImGuiKeyEvent(services, InputConstants.UI_LeftAlt, Keys.LeftAlt, ImGuiKey.LeftAlt);
            AddImGuiKeyEvent(services, InputConstants.UI_RightAlt, Keys.RightAlt, ImGuiKey.RightAlt);
            AddImGuiKeyEvent(services, InputConstants.UI_LeftSuper, Keys.LeftWindows, ImGuiKey.LeftSuper);
            AddImGuiKeyEvent(services, InputConstants.UI_RightSuper, Keys.RightWindows, ImGuiKey.RightSuper);

            AddImGuiMouseButtonEvent(services, InputConstants.UI_MouseButton01, MouseButtons.Left, 0);
            AddImGuiMouseButtonEvent(services, InputConstants.UI_MouseButton02, MouseButtons.Middle, 1);
            AddImGuiMouseButtonEvent(services, InputConstants.UI_MouseButton03, MouseButtons.Right, 2);
        }

        private static void AddImGuiKeyEvent(IServiceCollection services, string key, Keys defaultKey, ImGuiKey mapping)
        {
            services.AddInput(
                key, 
                defaultKey,
                new[]
                {
                    (ButtonState.Pressed, new ImGuiKeyEvent(mapping, true)),
                    (ButtonState.Released, new ImGuiKeyEvent(mapping, false))
                });
        }

        private static void AddImGuiMouseButtonEvent(IServiceCollection services, string key, MouseButtons defaultButton, int mapping)
        {
            services.AddInput(
                key,
                defaultButton,
                new[]
                {
                    (ButtonState.Pressed, new ImGuiMouseButtonEvent(mapping, true)),
                    (ButtonState.Released, new ImGuiMouseButtonEvent(mapping, false))
                });
        }
    }
}
