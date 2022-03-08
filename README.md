# Portfolio Rebalancing Simulator
Command line application to simulate the performance of different strategies of rebalancing a portfolio of stocks and bonds. Built using .NET Core 3.1 and the Command design pattern.

The simulation uses historical data of the S&P 500 index and US bonds from 1871 to December 2020. The program calculates the annualized gains for 30 year rolling periods and gives you the average of the results. Optionally the results of each rolling period can be saved in a csv file.

To build the project you must have .NET Core SDK installed and run `dotnet build` in the folder where the solution file is.
To run the application, you pass the name of the strategy and the option to save the annualized gain for each running period, e. g. `.\BalanceSimulation SimPerfStgy --save-results`  
If no argument is provided usage explanation is shown.

## Strategies available
### Rebalancing based on performance.  
Rebalancing is made based on the past performance of the S&P 500 index. Thresholds are defined for: from peak decreases or gain in last year. Each threshold has a rebalancing percentage associated with it. Stocks are sold or bought based on the rebalancing percentage.

For example, if the index has gain 35% in the las year the annual rebalancing rate would be of 4%. The monthly value of that percentage would be sold from the stocks index and bought from the bonds index.  

### Rebalancing based on percentage.
The rebalance is made when the stocks ratio in the portfolio gets over or below some thresholds. The rebalance is made to leave the portfolio with the original composition.

### No rebalancing.
No rebalancing is made during the period. Gains are just that ones of the indexes.

## Parameters.
There are some parameters that can be set for the simulation.
- The initial stocks ratio in the portfolio (defaults to 80%).  
- The number of years each simulation last (default is 30 years).
- For performance rebalancing the gain and loss thresholds as well as the rebalancing percentage for each threshold.  
- For percentage rebalancing the high and low percentage threshold for stocks.  

