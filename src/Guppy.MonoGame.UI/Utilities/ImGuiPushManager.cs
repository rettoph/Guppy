using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Utilities
{
    public abstract  class ImGuiPushManager
    {
        public abstract void Push();

        public abstract void Pop();
    }

    public class ImGuiPushManager<TWhat> : ImGuiPushManager
        where TWhat : struct, Enum
    {
        private Dictionary<TWhat, ImGuiPushValue<TWhat>>? _pushValues;
        private Action<Dictionary<TWhat, ImGuiPushValue<TWhat>>> _pop;

        public ImGuiPushManager(Action<Dictionary<TWhat, ImGuiPushValue<TWhat>>> pop)
        {
            _pop = pop;
        }

        public void Add(ImGuiPushValue<TWhat> whatValue)
        {
            if (_pushValues is null)
            {
                _pushValues ??= new Dictionary<TWhat, ImGuiPushValue<TWhat>>();
            }

            _pushValues.Add(whatValue.What, whatValue);
        }

        public virtual bool Remove(TWhat what)
        {
            if (_pushValues is null)
            {
                return false;
            }

            return _pushValues.Remove(what);
        }

        public TValue? Get<TValue>(TWhat what)
            where TValue : struct
        {
            if (_pushValues is null)
            {
                return default;
            }

            if(_pushValues.TryGetValue(what, out var pushValue) && pushValue is ImGuiPushValue<TWhat, TValue> casted)
            {
                return casted.Value;
            }

            return default;
        }

        public bool TrySet<TValue>(TWhat what, ref TValue value)
            where TValue : struct
        {
            if (_pushValues is null)
            {
                _pushValues ??= new Dictionary<TWhat, ImGuiPushValue<TWhat>>();
            }
        
            if (_pushValues.TryGetValue(what, out var pushValue))
            {
                if(pushValue is ImGuiPushValue<TWhat, TValue> casted)
                {
                    casted.Value = value;
                    return true;
                }
                
                this.Remove(what);
            }
        
            return false;
        }

        public override void Push()
        {
            if (_pushValues is null)
            {
                return;
            }

            if (_pushValues.Count == 0)
            {
                return;
            }

            this.Push(_pushValues);
        }

        protected virtual void Push(Dictionary<TWhat, ImGuiPushValue<TWhat>> pushValues)
        {
            foreach ((_, ImGuiPushValue<TWhat> pushValue) in pushValues)
            {
                pushValue.Push();
            }
        }

        public override void Pop()
        {
            if (_pushValues is null)
            {
                return;
            }

            if(_pushValues.Count == 0)
            {
                return;
            }

            _pop(_pushValues);
        }
    }
}
