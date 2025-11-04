using knowledgeBase.Entities;

namespace knowledgeBase.View_Models;

public class ArticleViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
    public List<Category> Categories { get; set; }
}