namespace AIPlayground.BusinessLogic.DTOs
{
    public class RunDto
    {
        public int Id { get; set; }

        public int Model { get; set; }

        public int Prompt { get; set; }

        public string ActualResponse { get; set; } = string.Empty;

        public double Temperature { get; set; }

        public double Rating { get; set; }

        public double UserRating { get; set; }
    }
}
