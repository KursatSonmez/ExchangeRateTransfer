namespace ExchangeRateTransfer.DotNet.Data.EntitiesAbstract
{
    public interface IExchangeRate<PK> : IRootEntity<PK> where PK : struct
    {
        /// <summary>
        /// Sistem içerisinde geçerli olan kur tarihi
        /// 
        /// Örneğin;
        /// 
        /// ExchangeRateDate = 26.09.2021(Pazar) olduğunda,
        /// XmlDate = 24.09.2021(Cuma) olacaktır.
        /// </summary>
        public DateTime ExchangeRateDate { get; set; }

        /// <summary>
        /// Yayın Tarihi (XML bilgisinden gelen tarih)
        /// </summary>
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// Bülten Numarası
        /// </summary>
        public string? BulletinNo { get; set; }

        /// <summary>
        /// Döviz Kodu (TR)
        /// </summary>
        public string? Kod { get; set; }

        /// <summary>
        /// Döviz Kodu (EN)
        /// </summary>
        public string? CurrencyCode { get; set; }

        /// <summary>
        /// Birim
        /// </summary>
        public int Unit { get; set; }

        /// <summary>
        /// Döviz Cinsi (TR)
        /// </summary>
        public string? Isim { get; set; }

        /// <summary>
        /// Döviz Cinsi (EN)
        /// </summary>
        public string? CurrencyName { get; set; }

        /// <summary>
        /// Döviz Alış
        /// </summary>
        public decimal ForexBuying { get; set; }

        /// <summary>
        /// Döviz Satış
        /// </summary>
        public decimal ForexSelling { get; set; }

        /// <summary>
        /// Efektif Alış
        /// </summary>
        public decimal BanknoteBuying { get; set; }

        /// <summary>
        /// Efektif Satış
        /// </summary>
        public decimal BanknoteSelling { get; set; }

        /// <summary>
        /// Çapraz Kur (USD)
        /// </summary>
        public decimal? CrossRateUSD { get; set; }

        /// <summary>
        /// Çapraz Kur (Diğer para birimleri)
        /// </summary>
        public decimal? CrossRateOther { get; set; }

        void OnInsert();
    }
}
