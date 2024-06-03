using Core.Business.Entities;
using Core.Business.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Services.Interfaces
{
    public interface IBlogService
    {
        Task<Response<IEnumerable<Blog>>> GetAllBlogs();
        Task<Response<Blog>> GetBlogById(long blogId);
        Task<Response<bool>> CreateBlog(Blog newBlog);
        Task<Response<bool>> DeleteBlog(long blogId);
        Task<Response<bool>> UpdateBlog(long blogId, Blog blogUpdate);
    }
}
