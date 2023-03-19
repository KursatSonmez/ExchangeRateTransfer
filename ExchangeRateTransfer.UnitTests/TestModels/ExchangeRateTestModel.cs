using ExchangeRateTransfer.DotNet.Data.Entities;

namespace ExchangeRateTransfer.UnitTests.TestModels
{
    public class ExchangeRateTestModel : ExchangeRate<Guid>
    {
        public static ExchangeRateTestModel MapFrom(ExchangeRate<Guid> exchangeRate)
            => new()
            {
                BanknoteBuying = exchangeRate.BanknoteBuying,
                BanknoteSelling = exchangeRate.BanknoteSelling,
                BulletinNo = exchangeRate.BulletinNo,
                CurrencyCode = exchangeRate.CurrencyCode,
                CurrencyName = exchangeRate.CurrencyName,
                ExchangeRateDate = exchangeRate.ExchangeRateDate,
                ForexBuying = exchangeRate.ForexBuying,
                ForexSelling = exchangeRate.ForexSelling,
                ReleaseDate = exchangeRate.ReleaseDate,
                Unit = exchangeRate.Unit,
                CreateDate = exchangeRate.CreateDate,
                CrossRateOther = exchangeRate.CrossRateOther,
                CrossRateUSD = exchangeRate.CrossRateUSD,
                Id = exchangeRate.Id,
                Isim = exchangeRate.Isim,
                Kod = exchangeRate.Kod,
            };

        public override int GetHashCode()
            => CurrencyCode!.GetHashCode();

        public override bool Equals(object? obj)
            => this.Equals((obj as ExchangeRateTestModel)!);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Code Smell", "S3875:\"operator==\" should not be overloaded on reference types", Justification = "<Pending>")]
        public static bool operator ==(ExchangeRateTestModel leftObj, ExchangeRateTestModel rightObj)
        {
            if (leftObj is null)
            {
                if (rightObj is null)
                    return true;

                return false;
            }

            return leftObj.Equals(rightObj);
        }
        public static bool operator !=(ExchangeRateTestModel leftObj, ExchangeRateTestModel rightObj)
            => !(leftObj == rightObj);

        private bool Equals(ExchangeRateTestModel obj)
        {
            if (obj is null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            return this.ExchangeRateDate.Date == obj.ExchangeRateDate.Date
                && this.ReleaseDate.Date == obj.ReleaseDate.Date
                && this.BulletinNo == obj.BulletinNo
                && this.Kod == obj.Kod
                && this.CurrencyCode == obj.CurrencyCode
                && this.Unit == obj.Unit
                && this.Isim == obj.Isim
                && this.CurrencyName == obj.CurrencyName
                && this.ForexBuying == obj.ForexBuying
                && this.ForexSelling == obj.ForexSelling
                && this.BanknoteBuying == obj.BanknoteBuying
                && this.BanknoteSelling == obj.BanknoteSelling
                && this.CrossRateUSD == obj.CrossRateUSD
                && this.CrossRateOther == obj.CrossRateOther;
        }
    }
}
