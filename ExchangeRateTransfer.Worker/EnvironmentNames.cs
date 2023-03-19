namespace ExchangeRateTransfer.Worker
{
    internal static class EnvironmentNames
    {
        /// <summary>
        /// Çalışma saati kotrolünü yapıp yapmayacağını belirler.
        /// Eğer Environmentler arasında bu environment varsa ve değeri true ise,
        /// çalışma saati kontrolü yapılmaz.
        /// False ise çalışma saati kontrol edilir. Eğer saat uygunsa işlemler gerçekleştirilir, değilse gerçekleştirilmez.
        /// </summary>
        internal const string ExchangeRateTransfer_Skip_WorkingHour = "ExchangeRateTransfer_Skip_WorkingHour";
    }
}