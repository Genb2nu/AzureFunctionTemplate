using Core.Business.Entities;
using Core.Business.Models.Domains;
using Core.Business.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Services.Implementations
{
    public class BlogService : IBlogService
    {
        private readonly AppDbContext _dbContext;

        public BlogService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<IEnumerable<Blog>>> GetAllBlogs()
        {
            try
            {
                var blogs = await _dbContext.Blogs.ToListAsync();
                return new Response<IEnumerable<Blog>>
                {
                    Data = blogs,
                    Success = true,
                    Message = "Successful",
                    MessageCode = "200"
                };

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Response<bool>> CreateBlog(Blog newBlog)
        {
            try
            {
                _dbContext.Blogs.Add(newBlog);
                _dbContext.SaveChanges();

                return new Response<bool>
                {
                    Data = true,
                    Success = true,
                    Message = "Blog created",
                    MessageCode = "200"
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Response<bool>> DeleteBlog(long blogId)
        {
            try
            {
                var blog = _dbContext.Blogs.FirstOrDefault(x => x.BlogId == blogId);
                if (blog == null)
                {
                    return new Response<bool>
                    {
                        Data = false,
                        Success = false,
                        Message = "Blog not found",
                        MessageCode = "404"
                    };
                }

                _dbContext.Blogs.Remove(blog);
                _dbContext.SaveChanges();

                return new Response<bool>()
                {
                    Data = true,
                    Success = true,
                    Message = "Blog deleted",
                    MessageCode = "200"
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        Task<Response<Blog>> IBlogService.GetBlogById(long blogId)
        {
            throw new NotImplementedException();
        }

        Task<Response<bool>> IBlogService.UpdateBlog(long blogId, Blog blogUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
