﻿// <auto-generated />
using System;
using Financity.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Financity.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20221214144520_AddForeignEntitiesWalletIdKeysInTransaction")]
    partial class AddForeignEntitiesWalletIdKeysInTransaction
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BudgetCategory", b =>
                {
                    b.Property<Guid>("BudgetsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TrackedCategoriesId")
                        .HasColumnType("uuid");

                    b.HasKey("BudgetsId", "TrackedCategoriesId");

                    b.HasIndex("TrackedCategoriesId");

                    b.ToTable("BudgetCategory");
                });

            modelBuilder.Entity("Financity.Domain.Entities.Budget", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<string>("CurrencyId")
                        .IsRequired()
                        .HasColumnType("character varying(16)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("UserId", "Name")
                        .IsUnique();

                    b.ToTable("Budgets");
                });

            modelBuilder.Entity("Financity.Domain.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<Guid?>("ParentCategoryId")
                        .HasColumnType("uuid");

                    b.Property<string>("TransactionType")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<Guid>("WalletId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ParentCategoryId");

                    b.HasIndex("TransactionType");

                    b.HasIndex("WalletId", "Name")
                        .IsUnique();

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Financity.Domain.Entities.Currency", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.HasKey("Id");

                    b.ToTable("Currencies");

                    b.HasData(
                        new
                        {
                            Id = "PLN",
                            Name = "Polski Złoty"
                        },
                        new
                        {
                            Id = "EUR",
                            Name = "Euro"
                        },
                        new
                        {
                            Id = "USD",
                            Name = "United States Dollar"
                        });
                });

            modelBuilder.Entity("Financity.Domain.Entities.Label", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<Guid>("WalletId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("WalletId", "Name")
                        .IsUnique();

                    b.ToTable("Labels");
                });

            modelBuilder.Entity("Financity.Domain.Entities.Recipient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<Guid>("WalletId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("WalletId", "Name")
                        .IsUnique();

                    b.ToTable("Recipients");
                });

            modelBuilder.Entity("Financity.Domain.Entities.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<string>("CategoryName")
                        .HasColumnType("character varying(64)");

                    b.Property<Guid?>("CategoryWalletId")
                        .HasColumnType("uuid");

                    b.Property<string>("CurrencyId")
                        .IsRequired()
                        .HasColumnType("character varying(16)");

                    b.Property<decimal>("ExchangeRate")
                        .HasColumnType("numeric");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("RecipientName")
                        .HasColumnType("character varying(64)");

                    b.Property<Guid?>("RecipientWalletId")
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("TransactionDate")
                        .HasColumnType("date");

                    b.Property<string>("TransactionType")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<Guid>("WalletId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("TransactionType");

                    b.HasIndex("WalletId");

                    b.HasIndex("CategoryWalletId", "CategoryName");

                    b.HasIndex("RecipientWalletId", "RecipientName");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Financity.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .IsUnique()
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Financity.Domain.Entities.Wallet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CurrencyId")
                        .IsRequired()
                        .HasColumnType("character varying(16)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("StartingAmount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric")
                        .HasDefaultValue(0m);

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("OwnerId", "Name")
                        .IsUnique();

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("LabelTransaction", b =>
                {
                    b.Property<Guid>("LabelsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TransactionsId")
                        .HasColumnType("uuid");

                    b.HasKey("LabelsId", "TransactionsId");

                    b.HasIndex("TransactionsId");

                    b.ToTable("LabelTransaction");
                });

            modelBuilder.Entity("UserWallet", b =>
                {
                    b.Property<Guid>("SharedWalletsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UsersWithSharedAccessId")
                        .HasColumnType("uuid");

                    b.HasKey("SharedWalletsId", "UsersWithSharedAccessId");

                    b.HasIndex("UsersWithSharedAccessId");

                    b.ToTable("UserWallet");
                });

            modelBuilder.Entity("BudgetCategory", b =>
                {
                    b.HasOne("Financity.Domain.Entities.Budget", null)
                        .WithMany()
                        .HasForeignKey("BudgetsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Financity.Domain.Entities.Category", null)
                        .WithMany()
                        .HasForeignKey("TrackedCategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Financity.Domain.Entities.Budget", b =>
                {
                    b.HasOne("Financity.Domain.Entities.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Financity.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Financity.Domain.Entities.Category", b =>
                {
                    b.HasOne("Financity.Domain.Entities.Category", "ParentCategory")
                        .WithMany()
                        .HasForeignKey("ParentCategoryId");

                    b.HasOne("Financity.Domain.Entities.Wallet", "Wallet")
                        .WithMany("Categories")
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Financity.Domain.Common.Appearance", "Appearance", b1 =>
                        {
                            b1.Property<Guid>("CategoryId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Color")
                                .HasMaxLength(64)
                                .HasColumnType("character varying(64)");

                            b1.Property<string>("IconName")
                                .HasMaxLength(64)
                                .HasColumnType("character varying(64)");

                            b1.HasKey("CategoryId");

                            b1.ToTable("Categories");

                            b1.WithOwner()
                                .HasForeignKey("CategoryId");
                        });

                    b.Navigation("Appearance")
                        .IsRequired();

                    b.Navigation("ParentCategory");

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("Financity.Domain.Entities.Label", b =>
                {
                    b.HasOne("Financity.Domain.Entities.Wallet", "Wallet")
                        .WithMany("Labels")
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Financity.Domain.Common.Appearance", "Appearance", b1 =>
                        {
                            b1.Property<Guid>("LabelId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Color")
                                .HasMaxLength(64)
                                .HasColumnType("character varying(64)");

                            b1.Property<string>("IconName")
                                .HasMaxLength(64)
                                .HasColumnType("character varying(64)");

                            b1.HasKey("LabelId");

                            b1.ToTable("Labels");

                            b1.WithOwner()
                                .HasForeignKey("LabelId");
                        });

                    b.Navigation("Appearance")
                        .IsRequired();

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("Financity.Domain.Entities.Recipient", b =>
                {
                    b.HasOne("Financity.Domain.Entities.Wallet", "Wallet")
                        .WithMany("Recipients")
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("Financity.Domain.Entities.Transaction", b =>
                {
                    b.HasOne("Financity.Domain.Entities.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Financity.Domain.Entities.Wallet", "Wallet")
                        .WithMany("Transactions")
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Financity.Domain.Entities.Category", "Category")
                        .WithMany("Transactions")
                        .HasForeignKey("CategoryWalletId", "CategoryName")
                        .HasPrincipalKey("WalletId", "Name")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Financity.Domain.Entities.Recipient", "Recipient")
                        .WithMany("Transactions")
                        .HasForeignKey("RecipientWalletId", "RecipientName")
                        .HasPrincipalKey("WalletId", "Name")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Category");

                    b.Navigation("Currency");

                    b.Navigation("Recipient");

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("Financity.Domain.Entities.Wallet", b =>
                {
                    b.HasOne("Financity.Domain.Entities.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Financity.Domain.Entities.User", "Owner")
                        .WithMany("OwnedWallets")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("LabelTransaction", b =>
                {
                    b.HasOne("Financity.Domain.Entities.Label", null)
                        .WithMany()
                        .HasForeignKey("LabelsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Financity.Domain.Entities.Transaction", null)
                        .WithMany()
                        .HasForeignKey("TransactionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UserWallet", b =>
                {
                    b.HasOne("Financity.Domain.Entities.Wallet", null)
                        .WithMany()
                        .HasForeignKey("SharedWalletsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Financity.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersWithSharedAccessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Financity.Domain.Entities.Category", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Financity.Domain.Entities.Recipient", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Financity.Domain.Entities.User", b =>
                {
                    b.Navigation("OwnedWallets");
                });

            modelBuilder.Entity("Financity.Domain.Entities.Wallet", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Labels");

                    b.Navigation("Recipients");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
