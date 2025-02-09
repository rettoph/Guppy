namespace Guppy.Core.Commands.Common.Services
{
    public interface ICommandService
    {
        void Invoke(string input);

        void Invoke<TCommand>(TCommand command)
            where TCommand : ICommand;
    }
}