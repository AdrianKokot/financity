﻿// <auto-generated />
using System;
using Financity.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Financity.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("Financity.Domain.Common.WalletAccess", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("WalletId")
                        .HasColumnType("uuid");

                    b.Property<int>("WalletAccessLevel")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "WalletId");

                    b.HasIndex("WalletId");

                    b.ToTable("WalletAccesses");
                });

            modelBuilder.Entity("Financity.Domain.Entities.Budget", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Budgets");
                });

            modelBuilder.Entity("Financity.Domain.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("ParentCategoryId")
                        .HasColumnType("uuid");

                    b.Property<int?>("TransactionType")
                        .HasColumnType("integer");

                    b.Property<Guid>("WalletId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ParentCategoryId");

                    b.HasIndex("WalletId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Financity.Domain.Entities.Currency", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Currencies");

                    b.HasData(
                        new
                        {
                            Id = new Guid("4773a5ae-eb68-469f-8afc-d6cda74b50a1"),
                            Code = "PLN",
                            Name = "Polski Złoty"
                        },
                        new
                        {
                            Id = new Guid("66f001bf-6ef5-4f2e-894a-d0f56c830368"),
                            Code = "EUR",
                            Name = "Euro"
                        },
                        new
                        {
                            Id = new Guid("ee9f493b-cf8e-45d1-b48e-d4e2905c7d2f"),
                            Code = "USD",
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
                        .HasColumnType("text");

                    b.Property<Guid>("WalletId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("WalletId");

                    b.ToTable("Labels");
                });

            modelBuilder.Entity("Financity.Domain.Entities.Recipient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("WalletId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("WalletId");

                    b.ToTable("Recipients");
                });

            modelBuilder.Entity("Financity.Domain.Entities.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<Guid?>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CurrencyId")
                        .HasColumnType("uuid");

                    b.Property<float>("ExchangeRate")
                        .HasColumnType("real");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("RecipientId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("TransactionType")
                        .HasColumnType("integer");

                    b.Property<Guid>("WalletId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("RecipientId");

                    b.HasIndex("WalletId");

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
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

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

                    b.Property<Guid>("CurrencyId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

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

            modelBuilder.Entity("Financity.Domain.Common.WalletAccess", b =>
                {
                    b.HasOne("Financity.Domain.Entities.User", "User")
                        .WithMany("AvailableWallets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Financity.Domain.Entities.Wallet", "Wallet")
                        .WithMany("UsersWithAccess")
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("Financity.Domain.Entities.Budget", b =>
                {
                    b.HasOne("Financity.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

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
                                .HasColumnType("text");

                            b1.Property<string>("IconName")
                                .HasColumnType("text");

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
                                .HasColumnType("text");

                            b1.Property<string>("IconName")
                                .HasColumnType("text");

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
                    b.HasOne("Financity.Domain.Entities.Category", "Category")
                        .WithMany("Transactions")
                        .HasForeignKey("CategoryId");

                    b.HasOne("Financity.Domain.Entities.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Financity.Domain.Entities.Recipient", "Recipient")
                        .WithMany("Transactions")
                        .HasForeignKey("RecipientId");

                    b.HasOne("Financity.Domain.Entities.Wallet", "Wallet")
                        .WithMany()
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

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

                    b.Navigation("Currency");
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
                    b.Navigation("AvailableWallets");
                });

            modelBuilder.Entity("Financity.Domain.Entities.Wallet", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Labels");

                    b.Navigation("Recipients");

                    b.Navigation("UsersWithAccess");
                });
#pragma warning restore 612, 618
        }
    }
}
