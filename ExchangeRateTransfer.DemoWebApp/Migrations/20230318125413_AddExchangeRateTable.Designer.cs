﻿// <auto-generated />
using System;
using ExchangeRateTransfer.DemoWebApp.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ExchangeRateTransfer.DemoWebApp.Migrations
{
    [DbContext(typeof(DemoWebAppDbContext))]
    [Migration("20230318125413_AddExchangeRateTable")]
    partial class AddExchangeRateTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ExchangeRateTransfer.DotNet.Data.Entities.ExchangeRate<long>", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<decimal>("BanknoteBuying")
                        .HasPrecision(18, 5)
                        .HasColumnType("decimal(18,5)");

                    b.Property<decimal>("BanknoteSelling")
                        .HasPrecision(18, 5)
                        .HasColumnType("decimal(18,5)");

                    b.Property<string>("BulletinNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreateDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<decimal?>("CrossRateOther")
                        .HasPrecision(18, 5)
                        .HasColumnType("decimal(18,5)");

                    b.Property<decimal?>("CrossRateUSD")
                        .HasPrecision(18, 5)
                        .HasColumnType("decimal(18,5)");

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CurrencyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ExchangeRateDate")
                        .HasColumnType("Date");

                    b.Property<decimal>("ForexBuying")
                        .HasPrecision(18, 5)
                        .HasColumnType("decimal(18,5)");

                    b.Property<decimal>("ForexSelling")
                        .HasPrecision(18, 5)
                        .HasColumnType("decimal(18,5)");

                    b.Property<string>("Isim")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Kod")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("Date");

                    b.Property<int>("Unit")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExchangeRateDate", "CurrencyCode")
                        .IsUnique();

                    b.ToTable("ExchangeRates", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("ExchangeRate<long>");
                });

            modelBuilder.Entity("ExchangeRateTransfer.DemoWebApp.TransferWrapper.Entities.ExchangeRate", b =>
                {
                    b.HasBaseType("ExchangeRateTransfer.DotNet.Data.Entities.ExchangeRate<long>");

                    b.HasDiscriminator().HasValue("ExchangeRate");
                });
#pragma warning restore 612, 618
        }
    }
}
