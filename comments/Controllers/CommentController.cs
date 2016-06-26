using Comments.Helpers;
using CommentSystems.Attributes;
using CommentSystems.Helpers;
using CommentSystems.Models;
using System;
using System.Collections.Generic;
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
        private static List<Comment> _Comments;

        static CommentController()
        {
            _Comments = new List<Comment>();

            for (int index = 0; index < 65; index++)
            {
                Comment comment = new Comment()
                {
                    Message = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.-" + index,
                    Id = Guid.NewGuid(),
                    CreatedOn = DateTime.Now.AddDays(index)
                };
                _Comments.Add(comment);
            }
        }

        [HttpPost]
        public JsonResult AddOrReplyComment(Comment comment)
        {
            Comment _comment = new Comment()
            {
                Message = comment.Message,
                ParentId = CommonHelpers.IsNotEmptyGuid(comment.ParentId) ? (Guid?)null : comment.ParentId,
                Id = Guid.NewGuid()
            };

            _Comments.Add(comment);
            return Json(GetCommentObject(_comment, _Comments.Count),JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void Edit(Comment comment)
        {
            var _comment = _Comments.Find(x => x.Id == comment.Id);
            _comment.Message = comment.Message;
        }

        [HttpDelete]
        public void Delete(string commentId, string parentId)
        {
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
        public JsonResult Get(string parentId, int page, int pageSize)
        {
            var result = _Comments.AsQueryable().ToPagedList(page, pageSize);
            var resultToReturn = GetExtractedComments(result, _Comments.Count);
            return Json(resultToReturn, JsonRequestBehavior.AllowGet);
        }
    }
}