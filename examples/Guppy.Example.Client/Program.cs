using Guppy.Example.Client;
using System.Reflection;
using System.Reflection.Emit;

MethodInfo methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.TestFunction), BindingFlags.Public | BindingFlags.Static);

DynamicMethod dynamicMethod = new DynamicMethod("Test", typeof(void), [typeof(TestClass), typeof(int)], typeof(Program).Module);

// Get an ILGenerator to emit the IL code for the method body
ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
ilGenerator.Emit(OpCodes.Ldarg_0);
ilGenerator.Emit(OpCodes.Box, typeof(int));
ilGenerator.Emit(OpCodes.Call, methodInfo);
ilGenerator.Emit(OpCodes.Ret);
var action = dynamicMethod.CreateDelegate<Action<int>>();

action(32);



Console.ReadLine();

using (var game = new Game1())
    game.Run();

class TestClass
{
    public static void TestFunction(object param)
    {
        Console.WriteLine($"TestFunction invoked with parameter1: {param}, Type: {param.GetType()}");
    }
}

interface ITestInterface
{
    string Value { get; }
}

class TestParam : ITestInterface
{
    public string Value { get; init; }
}