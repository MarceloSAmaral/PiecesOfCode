namespace TradeCancelling
{
    public static class ExcessiveTradeCancellingConfiguration
    {
        public static string DatafileFullName { get; set; } = "Trades.data";

        public static int TimeWindowInSeconds { get; set; } = 60;

        public static char ColumnSeparator { get; set; } = ',';

        public static double MaxPercentageOfCancellations = 1d / 3d;

    }
}
