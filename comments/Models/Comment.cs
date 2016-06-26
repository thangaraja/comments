using System;

namespace CommentSystems.Models
{
    public class Comment
    {
        public Guid Id { get; set; }

        public Nullable<Guid> ParentId { get; set; }

        public string Message { get; set; }

        public PagedList<Comment> Replies { get; set; }

        public int TotalReplyCount { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Nullable<Guid> UpdatedBy { get; set; }

        public Guid PostId { get; set; }

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