using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Comments.Models
{
    public class Comment
    {
        public Guid Id { get; set; }

        public Guid ParentId { get; set; }
        
        public string Message { get; set; }
        
        public Nullable<Guid> InReplyToCommentId { get; set; }

        public PagedList<Comment> Replies { get; set; }

        public int TotalReplyCount { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Nullable<Guid> UpdatedBy { get; set; }

        private Nullable<DateTime> _updatedOn;

        public Nullable<DateTime> UpdatedOn
        {
            get { return _updatedOn; }
            set
            {
                _updatedOn = _updatedOn == DateTime.MinValue ? null : value;
            }
        }
    }
}
