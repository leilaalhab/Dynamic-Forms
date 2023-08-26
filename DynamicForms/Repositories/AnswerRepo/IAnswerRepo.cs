using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicForms.Models.Answers;

namespace DynamicForms.Repositories.AnswerRepo
{
    public interface IAnswerRepo
    {
        public Task<List<Answer>> GetAllAnswers();
        public Task<Answer> GetAnswer(int Id);
        public Task<Answer> AddAnswer(AddAnswerDto newAnswer);
        public Task<Answer?> UpdateAnswer(UpdateAnswerDto updatedAnswer);
        public void DeleteAnswer(int Id);
    }
}