namespace Financity.Domain.Entities;

public class EntryCategory
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public int? ParentCategoryId { get; set; }
    public EntryCategory ParentCategory { get; set; }

    public ICollection<Entry> Entries { get; set; }
}