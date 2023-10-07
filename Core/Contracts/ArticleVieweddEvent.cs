namespace Core.Contracts;

public record ArticleVieweddEvent
{
    public int Id { get; set; }
    public DateTime DateViewed { get; set; }
    public int CountView { get; set; }
}