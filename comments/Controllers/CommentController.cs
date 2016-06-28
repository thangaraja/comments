using CommentSystems.Attributes;
using CommentSystems.Helpers;
using CommentSystems.Models;
using CommentSystems.Repositories;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Collections;
using System.Collections.Generic;

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
        private ICommentRepository commentRepository;

        private static Dictionary<string,string> users;

        static CommentController()
        {
            users = new Dictionary<string, string>();
            users.Add("486e6f82-5863-46fa-9f71-23ce80f83cde", "Legolas");
            users.Add("b196971f-6f19-4ae2-9454-90f6ae2412c4", "Aragorn");
            users.Add("f0ee3446-44a3-4aba-827b-a1d354bfc4f1", "Gandalf");
        }

        public CommentController()
        {
            this.commentRepository = new CommentRepository(new CommentDbContext());
        }

        [HttpPost]
        [Authorize]
        public JsonResult AddOrReplyComment(Comment comment)
        {
            Comment _comment = new Comment()
            {
                Message = comment.Message,
                ParentId = string.IsNullOrEmpty(comment.ParentId) ? string.Empty : comment.ParentId,
                Id = Guid.NewGuid().ToString(),
                CreatedBy = User.Identity.GetUserId(),
                PostId=comment.PostId,
                CreatedOn = DateTime.Now
            };

            commentRepository.Add(_comment);
            return Json(GetCommentObject(_comment, 10), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public void Edit(Comment comment)
        {
            comment.UpdatedBy = User.Identity.GetUserId();
            comment.UpdatedOn = DateTime.Now;
            commentRepository.Update(comment);
        }

        [HttpDelete]
        [Authorize]
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
                    CreatedBy = comment.CreatedBy,
                    CreatedByName = users[comment.CreatedBy],
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