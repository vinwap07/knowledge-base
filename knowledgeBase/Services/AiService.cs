using knowledgeBase.Entities;
using knowledgeBase.Repositories;

namespace knowledgeBase.Services;

public class AiService
{
    private QuestionLogRepository _questionLogRepository;
    private ArticleRepository _articleRepository;
    private ArticleService _articleService;

    public string ProcessQuestion(string question, int? employeeId)
    {
        
    }

    public List<Article> FindRelevantArticles(string question)
    {
        
    }

    public void LogInteraction(string question, string answer, int? employeeId, int? articleId)
    {
        
    }

    public void TrainAIWithFeedback()
    {
        
    }
}