using CommentSystems.Models;
using System;

namespace CommentSystems.Repositories
{
    public interface ICommentRepository : IDisposable
    {
        PagedList<Comment> GetComments(CommentSearchCondition searchCondition);

        Comment GetById(string id);

        void Add(Comment comment);

        void Delete(string commentId);

        void Update(Comment comment);
    }
}