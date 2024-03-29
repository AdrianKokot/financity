﻿using Financity.Domain.Common;
using Financity.Domain.Enums;

namespace Financity.Domain.Entities;

public sealed class Category : Entity, IBelongsToWallet
{
    public string Name { get; set; } = string.Empty;

    public Appearance Appearance { get; set; } = new();

    public TransactionType TransactionType { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();

    public Guid WalletId { get; set; }
    public Wallet Wallet { get; set; }
}