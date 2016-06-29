using Comments.Helpers;
using CommentSystems.Helpers;
using CommentSystems.Models;
using System;
using System.Data.Entity;
using System.Linq;

namespace CommentSystems.Repositories
{
    public class CommentRepository : ICommentRepository, IDisposable
    {
        private CommentDbContext context;

        public CommentRepository(CommentDbContext context)
        {
            this.context = context;
        }

        public Models.Comment GetById(string id)
        {
            return context.Comments.Find(id);
        }

        public void Add(Models.Comment comment)
        {
            comment.Id = Guid.NewGuid().ToString();
            context.Comments.Add(comment);
            Save();
        }

        public void Delete(string id)
        {
            Comment comment = GetById(id);
            context.Entry(comment).State = EntityState.Deleted;
            Save();
        }

        public void Update(Models.Comment comment)
        {
            context.Entry(comment).State = EntityState.Modified;
            Save();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public PagedList<Comment> GetComments(CommentSearchCondition searchCondition)
        {
            return GetParentCommentsByEntityId(searchCondition);
        }

        private void FetchAndIncludeReplies(CommentDbContext dbContext, PagedList<Comment> parentComments, CommentSearchCondition commentSearchCondition, bool fetchCountOnly)
        {
            if (parentComments == null || !parentComments.Any()) return;
            var parentCommentIds = parentComments.Select(x => x.Id).ToArray();
            var childComments = dbContext.Comments.Where(x => parentCommentIds.Contains(x.ParentId));

            foreach (var parent in parentComments)
            {
                if (fetchCountOnly)
                {
                    parent.Replies = childComments.Where(child => child.ParentId == parent.Id).OrderByDescending(p => p.CreatedOn).ToPagedList(true);
                }
                else
                {
                    parent.Replies = childComments.Where(child => child.ParentId == parent.Id).OrderByDescending(p => p.CreatedOn).ToPagedList(commentSearchCondition.PageNumber, commentSearchCondition.PageSize);
                    FetchAndIncludeReplies(dbContext, parent.Replies, commentSearchCondition, true);
                }
            }
        }

        private static IQueryable<Comment> GetComments(CommentDbContext context, CommentSearchCondition commentSearchCondition)
        {
            if (CommonHelpers.IsNotEmptyGuid(commentSearchCondition.ParentId))
            {
                return context.Comments.Where(x => x.ParentId == commentSearchCondition.ParentId);
            }
            else
            {
                return context.Comments.Where(x => x.PostId == commentSearchCondition.PostId && (x.ParentId == null || x.ParentId == ""));
            }
        }

        private PagedList<Comment> GetParentCommentsByEntityId(CommentSearchCondition commentSearchCondition)
        {
            using (var context = new CommentDbContext())
            {
                var parentCommentQuery = GetComments(context, commentSearchCondition);
                var parentComments = parentCommentQuery.OrderByDescending(p => p.CreatedOn).ToPagedList(commentSearchCondition.PageNumber, commentSearchCondition.PageSize);
                FetchAndIncludeReplies(context, parentComments, commentSearchCondition, false);
                return parentComments;
            }
        }

        private void Save()
        {
            try
            {
                context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                string rs = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    rs = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    Console.WriteLine(rs);

                    foreach (var ve in eve.ValidationErrors)
                    {
                        rs += "<br />" + string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw new Exception(rs);
            }
        }
    }
}