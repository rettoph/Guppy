using Guppy.MonoGame.UI.Utilities;
using Guppy.MonoGame.UI.Utilities.ImGuiPushManagers;
using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Elements
{
    public abstract partial class Element
    {
        private int _pushManagerCount;
        private ImGuiPushManager[] _pushManagers;

        public Element()
        {
            _pushManagers = Array.Empty<ImGuiPushManager>();
        }

        public virtual void Draw(GameTime gameTime)
        {
            this.BeginDraw(gameTime);
            this.InnerDraw(gameTime);
            this.EndDraw(gameTime);
        }

        protected virtual void BeginDraw(GameTime gameTime)
        {
            foreach(ImGuiPushManager pushManager in _pushManagers)
            {
                pushManager.Push();
            }
        }

        protected abstract void InnerDraw(GameTime gameTime);

        protected virtual void EndDraw(GameTime gameTime)
        {
            foreach (ImGuiPushManager pushManager in _pushManagers)
            {
                pushManager.Pop();
            }
        }

        protected TPushManager AddPushManager<TPushManager>(TPushManager pushManager)
            where TPushManager : ImGuiPushManager
        {
            var index = Array.IndexOf(_pushManagers, pushManager);

            if(index != -1)
            {
                return (_pushManagers[index] as TPushManager)!;
            }

            if(_pushManagers.Length < ++_pushManagerCount)
            {
                Array.Resize(ref _pushManagers, _pushManagerCount);
            }

            _pushManagers[_pushManagerCount - 1] = pushManager;

            return pushManager;
        }

        protected ImGuiPushManager<TWhat> AddPushManager<TWhat>(Action<Dictionary<TWhat, ImGuiPushValue<TWhat>>> pop)
            where TWhat : struct, Enum
        {
            return this.AddPushManager(new ImGuiPushManager<TWhat>(pop));
        }

        protected void RemovePushManager<TPushManager>(TPushManager? pushManager)
            where TPushManager : ImGuiPushManager
        {
            if(pushManager is null)
            {
                return;
            }

            var index = Array.IndexOf(_pushManagers, pushManager);

            if(index == -1)
            {
                return;
            }

            for (var i= index; i< _pushManagerCount - 1; i++)
            {
                _pushManagers[i] = _pushManagers[i + 1];
            }

            _pushManagers[_pushManagerCount - 1] = null!;
            _pushManagerCount--;
        }
    }
}
