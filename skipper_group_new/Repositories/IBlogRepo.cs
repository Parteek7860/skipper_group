using skipper_group_new.Models;

namespace skipper_group_new.Repositories
{
    public interface IBlogRepo
    {
        Task<List<BlogDtl>> GetBlogDtl();
        Task<int> AddBlog(clsBlog blog);
        Task<clsBlog> GetBlogById(int id);
        Task<int> DeleteBlog(int id);
        Task<int> ChangeBlogStatus(int id);


        Task<List<Blogcatdtl>> GetBlogCatList();
        Task<int> AddBlogCat(clsBlogCategory blog);
        Task<clsBlogCategory> EditBlogCat(int id);
        Task<int> DeleteBlogCat(int id);
        Task<int> ChangeBlogCatStatus(int id);
    }
}
