﻿using Guppy.MonoGame;
using Guppy.MonoGame.UI;
using Guppy.MonoGame.UI.Constants;
using Guppy.MonoGame.UI.Debuggers;
using Guppy.Network.Identity;
using Guppy.Network.Identity.Claims;
using Guppy.Network.Identity.Providers;
using Guppy.Network.Peers;
using ImGuiNET;
using ImPlotNET;
using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;
using Num = System.Numerics;

namespace Guppy.Network.UI
{
    public class UsersDebugger : SimpleDebugger, IImGuiDebugger
    {
        private readonly NetScope _netScope;

        private ImFontPtr _fontHeader;

        public string ButtonLabel { get; }

        public UsersDebugger(NetScope netScope)
        {
            _netScope = netScope;

            this.IsEnabled = false;
            this.Visible = false;

            this.ButtonLabel = "Users";
        }

        public void Initialize(ImGuiBatch imGuiBatch)
        {
            _fontHeader = imGuiBatch.Fonts[ImGuiFontConstants.DiagnosticsFontHeader].Ptr;
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

        public void Toggle()
        {
            this.Visible = !this.Visible;
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}