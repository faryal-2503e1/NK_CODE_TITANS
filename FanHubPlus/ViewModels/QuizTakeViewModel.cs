public class QuizTakeViewModel
{
    public int QuizId { get; set; }
    public string QuizTitle { get; set; }
    public List<QuizQuestionAnswerViewModel> Questions { get; set; } = new();
}

public class QuizQuestionAnswerViewModel
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; }
    public string OptionA { get; set; }
    public string OptionB { get; set; }
    public string OptionC { get; set; }
    public string OptionD { get; set; }
    public string? SelectedOption { get; set; }
}
