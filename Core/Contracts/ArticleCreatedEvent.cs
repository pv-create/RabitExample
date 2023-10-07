namespace Core.Contracts;

public record ArticleCreatedEvent
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime DateCreated { get; set; }
}