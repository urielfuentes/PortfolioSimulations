using System;
using System.Collections.Generic;
using System.Text;

namespace BalanceSimulation.Commands
{
    public interface ICommand
    {
        string CommandName { get; }
        void Execute();
    }
}
