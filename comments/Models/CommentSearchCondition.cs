
using System;
namespace CommentSystems.Models
{
    public class CommentSearchCondition
    {
        public string CommentId { get; set; }
        public string PostId { get; set; }
        public string ParentId { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }        
    }
}
