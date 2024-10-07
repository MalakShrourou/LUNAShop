using eCommerceAPI.Application.Commands;
using eCommerceAPI.Application.Models;
using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Services
{
    public class ArticleService
    {
        private readonly IRepository<Article> _repository;

        public ArticleService(IRepository<Article> repository)
        {
            _repository = repository;
        }

        public async Task<int> AddArticle(ArticleCommand command)
        {
            Article article = new Article()
            {
                TitleEN = command.TitleEN,
                ContentEN= command.ContentEN,    
                TitleAR = command.TitleAR,
                ContentAR= command.ContentAR,               
                CreatedOn = DateTime.Now
            };
            return await _repository.Create(article);
        }

        public async Task UpdateArticle(ArticleCommand command, int id)
        {
            Article article = await _repository.GetById(id);
            article.TitleEN = command.TitleEN;
            article.ContentEN = command.ContentEN;
            article.TitleAR = command.TitleAR;
            article.ContentAR = command.ContentAR;
            article.UpdatedOn = DateTime.Now;
            await _repository.Update(article);
        }

        public async Task DeleteArticle(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<IList<ArticleModel>> GetAllArticles()
        {
            IList<Article> articles = await _repository.GetAll();
            return articles.Select(a => new ArticleModel
            {
                Id = a.Id,
                TitleEN = a.TitleEN,
                ContentEN = a.ContentEN,
                TitleAR = a.TitleAR,
                ContentAR = a.ContentAR,
                CreatedOn = a.CreatedOn,
                UpdatedOn = a.UpdatedOn
            }).ToList();
        }

        public async Task<ArticleModel> GetArticleById(int id)
        {
            Article article = await _repository.GetById(id);
            if (article == null)
                return null;
            return new ArticleModel
            {
                Id = article.Id,
                TitleEN= article.TitleEN,
                ContentEN = article.ContentEN,
                TitleAR= article.TitleAR,
                ContentAR = article.ContentAR,
                UpdatedOn = article.UpdatedOn,
                CreatedOn = article.CreatedOn
            };
        }
    }
}
