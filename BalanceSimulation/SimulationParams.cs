namespace BalanceSimulation
{
    public static class SimulationParams
    {
        public static readonly double[] STGY_HIGH_BREAKPOINTS = { 1.26, 1.3, 1.34, 1.41 };
        public static readonly double[] STGY_LOW_BREAKPOINTS = { -0.29, -0.37, -0.44, -0.56 };
        public static readonly double[] STGY_RATIOS = { 0.03, 0.04, 0.05, 0.06 };
        public const double STOCKS_INIT_RATIO = 0.8;
        public const double BALANCING_HIGH = STOCKS_INIT_RATIO + 0.1;
        public const double BALANCING_LOW = STOCKS_INIT_RATIO - 0.1;
        public const int RUN_YEARS = 30;
    }
}
