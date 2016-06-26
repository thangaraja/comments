using CommentSystems.Attributes;
using CommentSystems.Helpers;
using CommentSystems.Models;
using CommentSystems.Repositories;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CommentSystems.Controllers
{
    public abstract class Controller : System.Web.Mvc.Controller
    {
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonDotNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }
    }



    public class CommentController : Controller
    {
        //private static List<Comment> _Comments;

        private ICommentRepository commentRepository;

        public CommentController()
        {
            this.commentRepository = new CommentRepository(new CommentDbContext());
        }

        [HttpPost]
        public JsonResult AddOrReplyComment(Comment comment)
        {
            Comment _comment = new Comment()
            {
                Message = comment.Message,
                ParentId = string.IsNullOrEmpty(comment.ParentId) ? string.Empty : comment.ParentId,
                Id = Guid.NewGuid().ToString(),
                CreatedBy = "Test",
                PostId=comment.PostId,
                CreatedOn = DateTime.Now
            };

            commentRepository.Add(_comment);
            return Json(GetCommentObject(_comment, 10), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void Edit(Comment comment)
        {
            comment.UpdatedBy = "Test";
            comment.UpdatedOn = DateTime.Now;
            commentRepository.Update(comment);
        }

        [HttpDelete]
        public void Delete(string commentId, string parentId)
        {
            commentRepository.Delete(commentId);
        }

        private object GetCommentObject(Comment comment, int totalReplies)
        {
            return new
            {
                Comment = new
                {
                    comment.Id,
                    comment.Message,
                    CreatedOn = comment.CreatedOn.ToString("g"),
                    CreatedBy = "Gandalf",
                    InReplyToCommentId = CommonHelpers.IsNotEmptyGuid(comment.ParentId) ? comment.ParentId.ToString() : null,
                    IsEdited = CommonHelpers.IsNotEmptyGuid(comment.UpdatedBy)
                },
                Replies = GetExtractedComments(comment.Replies, comment.Replies != null ? comment.Replies.TotalCount : 0)
            };
        }

        private object GetExtractedComments(PagedList<Comment> replies, int totalReplies)
        {
            if (totalReplies == 0) return null;
            var extractedReplies = replies.Select(x => GetCommentObject(x, replies.TotalCount));

            if (replies != null)
                return new
                {
                    TotalReplies = totalReplies,
                    Replies = extractedReplies
                };
            return null;
        }

        [HttpGet]
        public JsonResult Get(string postId, string parentId, int page, int pageSize)
        {
            CommentSearchCondition condition = new CommentSearchCondition()
            {
                PostId = postId,
                ParentId = parentId,
                PageNumber = page,
                PageSize = pageSize
            };

            var replies = commentRepository.GetComments(condition);

            var result = GetExtractedComments(replies, replies.TotalCount);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}