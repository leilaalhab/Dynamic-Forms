using DynamicForms.Models.Answers;

namespace DynamicForms.Services.AnswerService
{
    public class AnswerService : IAnswerService
    {
        private readonly DataContext _context;

        public AnswerService(DataContext context)
        {
            _context = context;
        }

        // answers must also save whether that input is visible or not
        public async Task<ServiceResponse<List<Answer>>> GetAnswersWithProgressId(int progressId)
        {
            var response = new ServiceResponse<List<Answer>>();
            List<Answer> answers = new();

            answers.AddRange(await _context.DoubleAnswers.Where(c => c.ProgressId == progressId).ToListAsync());
            answers.AddRange(await _context.IntegerAnswers.Where(c => c.ProgressId == progressId).ToListAsync());
            answers.AddRange(await _context.TextAnswers.Where(c => c.ProgressId == progressId).ToListAsync());

            response.Data = answers;

            return response;
        }

        public async Task AddAllAnswers(List<TextAnswer> textAnswers, List<DoubleAnswer> doubleAnswers,
            List<IntegerAnswer> integerAnswers)
        {
            if (textAnswers.Count() != 0)
            {
                foreach (var answer in textAnswers)
                {
                    var temp = _context.TextAnswers.FirstOrDefault(c =>
                        c.ProgressId == answer.ProgressId && c.InputId == answer.InputId);
                    if (temp is null)
                        _context.TextAnswers.Add(answer);
                    else
                        temp.Value = answer.Value;
                }
            }

            if (doubleAnswers.Count() != 0)
                foreach (var answer in doubleAnswers)
                {
                    var temp = _context.DoubleAnswers.FirstOrDefault(c =>
                        c.ProgressId == answer.ProgressId && c.InputId == answer.InputId);
                    if (temp is null)
                        _context.DoubleAnswers.Add(answer);
                    else
                        temp.Value = answer.Value;
                }

            if (integerAnswers.Count() != 0)
                foreach (var answer in integerAnswers)
                {
                    var temp = _context.IntegerAnswers.FirstOrDefault(c =>
                        c.ProgressId == answer.ProgressId && c.InputId == answer.InputId);
                    if (temp is null)
                        _context.IntegerAnswers.Add(answer);
                    else
                        temp.Value = answer.Value;
                }

            await _context.SaveChangesAsync();
        }
    }
}