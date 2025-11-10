using skipper_group_new.Models;

namespace skipper_group_new.Interface
{
    public interface IBlog
    {
        Task<List<BlogDtl>> GetBlogTblData();
        Task<int> AddBlog(clsBlog product);
        Task<clsBlog> EditBlog(int id);
        Task<int> DeleteBlog(int id);
        Task<int> ChangeBlogStatus(int id);

        Task<List<Blogcatdtl>> GetBlogCatTblData();
        Task<int> AddBlogCat(clsBlogCategory product);
        Task<clsBlogCategory> EditBlogCat(int id);
        Task<int> DeleteBlogCat(int id);
        Task<int> ChangeBlogCatStatus(int id);
    }
}
