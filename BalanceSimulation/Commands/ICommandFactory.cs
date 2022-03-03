namespace BalanceSimulation.Commands
{
    public interface ICommandFactory
    {
        string CommandName { get; }
        ICommand MakeCommand(string[] arguments);
    }
}
