﻿using Microsoft.CodeAnalysis;
using System;
using System.Text;

namespace Guppy.GUI.ImGuiNETSourceGenerator
{
    public class CodeBuilder : IDisposable
    {
        private StringBuilder _string = new StringBuilder();
        private int _depth = 0;
        private GeneratorExecutionContext _context;
        private string _nameSpace;
        private string _fileName;

        public CodeBuilder(ref GeneratorExecutionContext context, string nameSpace)
        {
            _context = context;
            _nameSpace = nameSpace;
        }

        public CodeBuilder File(string filename)
        {
            _fileName = filename;
            _string.Clear();

            this.AppendLine("// <auto-generated/>");
            this.AppendLine();

            return this.Section($"namespace {_nameSpace}");
        }


        public CodeBuilder Section(string section = null)
        {
            if (section != null)
            {
                this.AppendLine(section);
            }

            this.AppendLine("{");
            _depth++;

            return this;
        }

        public void AppendLine(string line = null)
        {
            if (line == null || line == string.Empty)
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

            if (_depth == 0 && _fileName != null)
            {
                _context.AddSource(_fileName, _string.ToString());
                _fileName = null;
            }
        }

        public override string ToString()
        {
            return _string.ToString();
        }
    }
}
