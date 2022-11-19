using Guppy.MonoGame;
using Guppy.MonoGame.UI;
using Guppy.MonoGame.UI.Constants;
using Guppy.MonoGame.UI.Elements;
using Guppy.MonoGame.UI.Extensions.Elements;
using Guppy.Network.Identity;
using Guppy.Network.Identity.Claims;
using Guppy.Network.Identity.Providers;
using ImGuiNET;
using ImPlotNET;
using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;
using Num = System.Numerics;

namespace Guppy.Network.UI
{
    public class UsersDebugger : IImGuiDebugger
    {
        private IUserProvider _users;
        private Window _window;
        private Dictionary<User, Element> _renderedUsers;
        private ImGuiFont _fontHeader;

        public UsersDebugger(IUserProvider users)
        {
            _users = users;
            _window = new Window("Users");
            _renderedUsers = new Dictionary<User, Element>();
            _fontHeader = default!;

            _users.OnUserConnected += this.HandleUserConnected;
        }

        public void Initialize(ImGuiBatch imGuiBatch)
        {
            _fontHeader = imGuiBatch.Fonts[ImGuiFontConstants.DiagnosticsFontHeader];

            _window.AddStyleColor(ImGuiCol.Text, Color.White)
                .AddStyleVar(ImGuiStyleVar.WindowMinSize, new Num.Vector2(400, 0))
                .SetFont(imGuiBatch.Fonts[ImGuiFontConstants.DiagnosticsFont]);
        }

        public void Draw(GameTime gameTime)
        {
            _window.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            // throw new NotImplementedException();
        }

        private void HandleUserConnected(IUserProvider sender, User user)
        {
            _window.AddChild(() =>
            {
                var container = new CollapsingHeader($"Id: {user.Id} Claims: {user.Count()}");

                container.AddChild(() =>
                {
                    var claims = new Table("claims", 4)
                        .AddText("Key", t => t.SetFont(_fontHeader))
                        .AddText("Value", t => t.SetFont(_fontHeader))
                        .AddText("Type", t => t.SetFont(_fontHeader))
                        .AddText("Accessability", t => t.SetFont(_fontHeader));

                    foreach (Claim claim in user)
                    {
                        claims.AddTextWrapped(claim.Key)
                            .AddTextWrapped(claim.GetValue()?.ToString() ?? string.Empty)
                            .AddTextWrapped(claim.GetValue()?.GetType().Name ?? "null")
                            .AddTextWrapped(claim.Accessiblity.ToString());
                    }

                    return claims;
                });

                _renderedUsers.Add(user, container);

                return container;
            });
        }
    }
}