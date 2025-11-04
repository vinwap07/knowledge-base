namespace knowledgeBase.Entities;

public class QuestionLog
{
    public int Id { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public int Assessment { get; set; }
    public string UserComment { get; set; }
}