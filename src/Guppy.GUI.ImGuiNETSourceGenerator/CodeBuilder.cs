using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.GUI.ImGuiNETSourceGenerator
{
    public class CodeBuilder : IDisposable
    {
        private StringBuilder _string = new StringBuilder();
        private int _depth = 0;

        public CodeBuilder Section(string section = null)
        {
            if(section != null)
            {
                this.AppendLine(section);
            }
            
            this.AppendLine("{");
            _depth++;

            return this;
        }

        public void AppendLine(string line = null)
        {
            if(line == null || line == string.Empty)
            {
                _string.AppendLine();
                return;
            }

            _string.Append(new string('\t', _depth));
            _string.AppendLine(line);
        }

        public void Dispose()
        {
            _depth--;
            this.AppendLine("}");
        }

        public override string ToString()
        {
            return _string.ToString();
        }
    }
}
