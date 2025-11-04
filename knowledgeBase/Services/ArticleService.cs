using knowledgeBase.Entities;
using knowledgeBase.Repositories;

namespace knowledgeBase.Services;

public class ArticleService
{
    private ArticleRepository _articleRepository;
    private CategoryRepository _categoryRepository;
    private ArticleCategoryRepository _articleCategoryRepository;

    public Article GetArticleWithAccessCheck(int articleId, int? employeeId)
    {
        
    }

    public bool CreateArticle(Article article, int authorId, List<int> categoryIds)
    {
        
    }

    public List<Article> SearchArticles(string query, int? categoryId = null)
    {
        
    }

    public List<Article> GetArticlesForOnboarding(int departmentId)
    {
        
    }
}