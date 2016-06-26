using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommentSystems.Models
{

    public class Comment
    {
        [Key]
        public string Id { get; set; }

        public string ParentId { get; set; }

        [Required]
        [StringLength(250)]
        public string Message { get; set; }

        [NotMapped]
        public PagedList<Comment> Replies { get; set; }

        [NotMapped]
        public int TotalReplyCount { get; set; }

        [Required]
        [StringLength(250)]
        public string CreatedBy { get; set; }

        [Required] 
        public DateTime CreatedOn { get; set; }

        [StringLength(250)]
        public string UpdatedBy { get; set; }

        [Required] 
        public string PostId { get; set; }

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