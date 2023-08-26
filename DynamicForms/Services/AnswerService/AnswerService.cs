using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicForms.Models.Answers;

namespace DynamicForms.Services.AnswerService
{
    public class AnswerService : IAnswerService
    {
        private readonly DataContext _context;

        public AnswerService(DataContext context) {
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

        public async Task AddAllAnswers(List<TextAnswer> textAnswers, List<DoubleAnswer> doubleAnswers, List<IntegerAnswer> integerAnswers)
        {
            if (textAnswers.Count() != 0)
                await _context.TextAnswers.AddRangeAsync(textAnswers);
            if (doubleAnswers.Count() != 0)
                await _context.DoubleAnswers.AddRangeAsync(doubleAnswers);
            if (integerAnswers.Count() != 0)
                await _context.IntegerAnswers.AddRangeAsync(integerAnswers);

            await _context.SaveChangesAsync();
        }

    }
}