using System.ComponentModel.DataAnnotations;

namespace Todo_Week13.DTOs
{
    public class UpdateTodoDto
    {

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        [RegularExpression("^(High|Medium|Low)$", ErrorMessage = "Priority must be High, Medium, or Low")]
        public string Priority { get; set; } = "Medium";
        public bool IsCompleted { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
