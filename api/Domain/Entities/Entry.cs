using Financity.Domain.Common;

namespace Financity.Domain.Entities;

public class Entry : AuditableEntity
{
    public DateTime CreationDate { get; set; }
    
    public decimal Amount { get; set; }
    public string? Note { get; set; }

    public int? RecipientId { get; set; }
    public Recipient? Recipient { get; set; }
    
    public int TypeId { get; set; }
    public EntryType Type { get; set; }

    public int AccountId { get; set; }
    public Account Account { get; set; }
    
    public int? CategoryId { get; set; }
    public EntryCategory? Category { get; set; }
    
    public int CurrencyId { get; set; }      
    public Currency Currency { get; set; }   
}