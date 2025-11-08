using knowledgeBase.Entities;
using knowledgeBase.Repositories;

namespace knowledgeBase.Services;

public class ArticleService
{
    private ArticleRepository _articleRepository;
    private CategoryRepository _categoryRepository;
    private ArticleCategoryRepository _articleCategoryRepository;
    private SessionRepository _sessionRepository;
    private UserArticleRepository _userArticleRepository;
    public async Task<bool> CreateArticle(Article article, List<string> categories, string sessionId)
    {
        var userRole = await _sessionRepository.GetRoleBySessionId(sessionId);

        if (userRole != "admin")
        {
            return false;
        }
        // TODO: валидация статьи
        
        var isArticleAdded = await _articleRepository.Create(article);
        var articles = await _articleRepository.GetByTitle(article.Title);
        int articleId = 0;
        
        if (isArticleAdded)
        {
            foreach (var art in articles)
            {
                if (art.Title == article.Title)
                {
                    articleId = art.Id;
                    break;
                }
            }

            foreach (var category in categories)
            {
                await _articleCategoryRepository.Create(new ArticleCategory()
                    { ArticleId = articleId, CategoryId = category });
            }
        }
        
        return isArticleAdded;
    }

    public async Task<List<Article>> SearchArticles(int? categoryId = null)
    {
        var articles = new List<Article>();
        
        if (categoryId.HasValue)
        {
            articles = await _articleRepository.GetByCategoryId(categoryId.Value);
        }
        else
        {
            articles = await _articleRepository.GetAll();
        }

        return articles;
    }

    public async Task<bool> DeleteArticle(Article article, string sessionId)
    {
        var user = await _sessionRepository.GetUserBySessionId(sessionId);
        var isDeleted = false;
        if (article.Author == user.Email)
        {
            isDeleted = await _articleRepository.Delete(article.Id); 
        }
        
        return isDeleted;
    }

    public async Task<bool> AddToFavorite(int articleId, string sessionId)
    {
        var user = await _sessionRepository.GetUserBySessionId(sessionId);
        return await _userArticleRepository.Create(new UserArticle() {Article = articleId, User = user.Email});
    }

    public async Task<bool> RemoveFromFavorite(int articleId, string sessionId)
    {
        var user = await _sessionRepository.GetUserBySessionId(sessionId);
        return await _userArticleRepository.Delete((user.Email, articleId));
    }
}