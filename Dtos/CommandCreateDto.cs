using System.ComponentModel.DataAnnotations;

namespace Commander.Dtos
{
    public class CommandCreateDto 
    {
        // should we be passing an id as input from the client for creation of this DTO?  No because the database takes care of it
        
        //These data annotations will make error handling much easier
        [Required]
        [MaxLength(250)]
        public string HowTo { get; set; }

        [Required]
        public string Line { get; set; }

        [Required]
        public string Platform { get; set; }

    }
}