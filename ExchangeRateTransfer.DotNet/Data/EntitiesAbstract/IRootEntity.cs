namespace ExchangeRateTransfer.DotNet.Data.EntitiesAbstract
{
    public interface IRootEntity<PK> where PK : struct
    {
        public PK Id { get; set; }

        public DateTimeOffset CreateDate { get; set; }
    }
}
