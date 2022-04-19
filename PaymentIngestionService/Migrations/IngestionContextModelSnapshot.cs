﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PaymentIngestionService.Infrastructure.EntityFramework;

#nullable disable

namespace PaymentIngestionService.Migrations
{
    [DbContext(typeof(IngestionContext))]
    partial class IngestionContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Common.Database.IdempotencyToken", b =>
                {
                    b.Property<Guid>("MessageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Consumer")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getutcdate()");

                    b.HasKey("MessageId", "Consumer");

                    b.ToTable("IdempotencyTokens");
                });

            modelBuilder.Entity("PaymentIngestionService.Domain.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getutcdate()");

                    b.HasKey("Id");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("PaymentIngestionService.Domain.Payment", b =>
                {
                    b.OwnsOne("PaymentIngestionService.Domain.Amount", "Amount", b1 =>
                        {
                            b1.Property<Guid>("PaymentId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Currency");

                            b1.Property<int>("Value")
                                .HasColumnType("int")
                                .HasColumnName("Amount");

                            b1.HasKey("PaymentId");

                            b1.ToTable("Payments");

                            b1.WithOwner()
                                .HasForeignKey("PaymentId");
                        });

                    b.OwnsOne("PaymentIngestionService.Domain.CardDetails", "CardDetails", b1 =>
                        {
                            b1.Property<Guid>("PaymentId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("CVV")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("CVV");

                            b1.Property<string>("CardNumber")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("CardNumber");

                            b1.HasKey("PaymentId");

                            b1.ToTable("Payments");

                            b1.WithOwner()
                                .HasForeignKey("PaymentId");

                            b1.OwnsOne("PaymentIngestionService.Domain.ExpiryDate", "ExpiryDate", b2 =>
                                {
                                    b2.Property<Guid>("CardDetailsPaymentId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<int>("Month")
                                        .HasColumnType("int")
                                        .HasColumnName("ExpiryMonth");

                                    b2.Property<int>("Year")
                                        .HasColumnType("int")
                                        .HasColumnName("ExpiryYear");

                                    b2.HasKey("CardDetailsPaymentId");

                                    b2.ToTable("Payments");

                                    b2.WithOwner()
                                        .HasForeignKey("CardDetailsPaymentId");
                                });

                            b1.Navigation("ExpiryDate");
                        });

                    b.Navigation("Amount");

                    b.Navigation("CardDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
