using System;
using System.Collections.Generic;
using System.Text;

namespace BalanceSimulation.Commands
{
    public interface ICommandFactory
    {
        string CommandName { get; }
        ICommand MakeCommand(string[] arguments);
    }
}
