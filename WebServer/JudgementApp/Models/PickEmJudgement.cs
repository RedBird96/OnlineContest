namespace JudgementApp.Models
{
    public class PickEmJudgment
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Stocks { get; set; }
        public string Amounts { get; set; }
        public long CompanyId { get; set; }
        public long ContestId { get; set; }
        public long QuestionId { get; set; }
        
    }
}