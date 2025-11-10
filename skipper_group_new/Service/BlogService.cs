using skipper_group_new.Interface;
using skipper_group_new.Models;
using skipper_group_new.Repositories;

namespace skipper_group_new.Service
{
    public class BlogService : IBlog
    {
        private readonly IBlogRepo _repository;

        public BlogService(IBlogRepo repository)
        {
            _repository = repository;
        }

        public Task<List<BlogDtl>> GetBlogTblData()
        {
            return _repository.GetBlogDtl();
        }

        public Task<int> AddBlog(clsBlog blog)
        {
            return _repository.AddBlog(blog);
        }

        public Task<clsBlog> EditBlog(int id)
        {
            return _repository.GetBlogById(id);
        }

        public Task<int> DeleteBlog(int id)
        {
            return _repository.DeleteBlog(id);
        }
        public Task<int> ChangeBlogStatus(int id)
        {
            return _repository.ChangeBlogStatus(id);
        }
        public Task<List<Blogcatdtl>> GetBlogCatTblData()
        {
            return _repository.GetBlogCatList();
        }

        public Task<int> AddBlogCat(clsBlogCategory blog)
        {
            return _repository.AddBlogCat(blog);
        }

        public Task<clsBlogCategory> EditBlogCat(int id)
        {
            return _repository.EditBlogCat(id);
        }

        public Task<int> DeleteBlogCat(int id)
        {
            return _repository.DeleteBlogCat(id);
        }
        public Task<int> ChangeBlogCatStatus(int id)
        {
            return _repository.ChangeBlogCatStatus(id);
        }
    }
}
