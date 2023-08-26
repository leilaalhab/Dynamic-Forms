using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicForms.Models.Answers;

namespace DynamicForms.Services.AnswerService
{
    public interface IAnswerService
    {
        public Task<ServiceResponse<List<Answer>>> GetAnswersWithProgressId(int progressId);
        public Task AddAllAnswers(List<TextAnswer> textAnswers, List<DoubleAnswer> doubleAnswers, List<IntegerAnswer> integerAnswers);

    }
}