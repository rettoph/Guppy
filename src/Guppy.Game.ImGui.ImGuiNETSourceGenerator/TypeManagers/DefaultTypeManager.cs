using System;

namespace Guppy.GUI.ImGuiNETSourceGenerator.TypeManagers
{
    internal class DefaultTypeManager : TypeManager
    {
        public override string ReturnTypeName => this.ImGuiType.IsVoid() ? "void" : base.ReturnTypeName;

        public override string GuppyParameterType => base.GuppyParameterType;

        public DefaultTypeManager(Type imGuiType) : base(imGuiType, imGuiType.FullName)
        {
        }

        public override void GenerateSourceFiles(CodeBuilder source)
        {
            //throw new NotImplementedException();
        }

        public override string GetGuppyToImGuiConverter(string parameter)
        {
            return $"{parameter}";
        }

        public override string GetImGuiToGuppyConverter(string parameter)
        {
            return $"{parameter}";
        }
    }
}
