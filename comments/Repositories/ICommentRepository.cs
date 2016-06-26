using CommentSystems.Models;
using System;
using System.Collections.Generic;

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