using Comments.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Comments.Helpers;

namespace Comments.Controllers
{
    [CamelCasingFilterAttribute]
    public class CommentController : ApiController
    {
        private static List<Comment> _Comments;

        static CommentController()
        {
            _Comments = new List<Comment>();

            for (int index = 0; index < 10; index++)
            {
                Comment comment = new Comment()
                {
                    Message = "Message-" + index,
                    Id = Guid.NewGuid()
                };
                _Comments.Add(comment);

            }
        }

        [HttpPost]
        public IHttpActionResult AddOrReplyComment([FromBody]Comment comment)
        {
            Comment _comment = new Comment()
            {
                Message = comment.Message,
                InReplyToCommentId = CommonHelpers.IsNotEmptyGuid(comment.ParentId) ? (Guid?)null : comment.ParentId,
                Id = Guid.NewGuid()
            };

            _Comments.Add(comment);
            return Ok(GetCommentObject(_comment, _Comments.Count));
        }

        [HttpPost]
        public void Edit([FromBody] Comment comment)
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
                    InReplyToCommentId = CommonHelpers.IsNotEmptyGuid(comment.InReplyToCommentId) ? comment.InReplyToCommentId.ToString() : null,
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
        public IHttpActionResult Get(string parentId, int page, int pageSize)
        {
            var result = _Comments.ToPagedList(page, pageSize, _Comments.Count);
            var resultToReturn = GetExtractedComments(result, _Comments.Count);
            return Ok(resultToReturn);
        }
    }
}