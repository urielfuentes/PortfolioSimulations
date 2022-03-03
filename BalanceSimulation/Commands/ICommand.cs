namespace BalanceSimulation.Commands
{
    public interface ICommand
    {
        string CommandName { get; }
        void Execute();
    }
}
