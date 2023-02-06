using Guppy.Common;
using Guppy.MonoGame;
using Guppy.MonoGame.Constants;
using Guppy.MonoGame.Messages;
using Guppy.MonoGame.Providers;
using Guppy.MonoGame.UI;
using Guppy.MonoGame.UI.Constants;
using Guppy.MonoGame.UI.Providers;
using Guppy.Network.Identity;
using Guppy.Network.Identity.Claims;
using Guppy.Network.Identity.Providers;
using Guppy.Network.Peers;
using ImGuiNET;
using ImPlotNET;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Runtime.InteropServices;
using Num = System.Numerics;

namespace Guppy.Network.UI
{
    public class UsersDebugger : SimpleDrawableGameComponent,
        ISubscriber<Toggle<UsersDebugger>>
    {
        private readonly NetScope _netScope;

        private ImFontPtr _fontHeader;

        public UsersDebugger(
            NetScope netScope, 
            IMenuProvider menus,
            IImGuiBatchProvider batches)
        {
            _netScope = netScope;
            _fontHeader = batches.Get(ImGuiBatchConstants.Debug).Fonts[ResourceConstants.DiagnosticsImGuiFont].Ptr;

            this.IsEnabled = false;
            this.Visible = false;

            menus.Get(MenuConstants.Debug).Add(new MenuItem()
            {
                Label = "Users",
                OnClick = Toggle<UsersDebugger>.Instance
            });
        }

        public override void Draw(GameTime gameTime)
        {
            ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, new Num.Vector2(400, 0));
            if (ImGui.Begin($"Users ({_netScope.Users.Count()})"))
            {
                foreach (User user in _netScope.Users)
                {
                    if (ImGui.CollapsingHeader($"Id: {user.Id}, Claims: {user.Count()}"))
                    {
                        if (ImGui.BeginTable("claims", 4))
                        {
                            ImGui.PushFont(_fontHeader);
                            ImGui.TableNextColumn();
                            ImGui.Text("Key");
                            ImGui.TableNextColumn();
                            ImGui.Text("Value");
                            ImGui.TableNextColumn();
                            ImGui.Text("Type");
                            ImGui.TableNextColumn();
                            ImGui.Text("Accessibility");
                            ImGui.PopFont();

                            foreach (Claim claim in user)
                            {
                                ImGui.TableNextRow();
                                ImGui.TableNextColumn();
                                ImGui.TextWrapped($"{claim.Key}");
                                ImGui.TableNextColumn();
                                ImGui.TextWrapped($"{claim.GetValue()}");
                                ImGui.TableNextColumn();
                                ImGui.TextWrapped($"{claim.GetValue()?.GetType().Name ?? "null"}");
                                ImGui.TableNextColumn();
                                ImGui.TextWrapped($"{claim.Accessibility}");
                            }
                        }
                        ImGui.EndTable();
                    }
                }
            }
            ImGui.End();

            ImGui.PopStyleVar();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Process(in Toggle<UsersDebugger> message)
        {
            this.Visible = !this.Visible;
        }
    }
}