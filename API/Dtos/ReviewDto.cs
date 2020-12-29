using System.ComponentModel.DataAnnotations;

public class ReviewDto
{
        [MaxLength(500, ErrorMessage="Comment cannot exceed 500 characters")]
        public string Comment { get; set; }
        [Required]
        [Range(1, 5)]
        public int? Stars { get; set; }
        [Required]
        public int? ProductId { get; set; }
}