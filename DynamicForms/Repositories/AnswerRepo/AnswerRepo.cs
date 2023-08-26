using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicForms.Models.Answers;

namespace DynamicForms.Repositories.AnswerRepo
{
    public class AnswerRepo : IAnswerRepo
    {
        readonly private DataContext _context;

        public AnswerRepo(DataContext context) {
            _context = context;
        }
        public Task<Answer> AddAnswer(AddAnswerDto newAnswer)
        {
            throw new NotImplementedException();
        }

        public void DeleteAnswer(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Answer>> GetAllAnswers()
        {
            throw new NotImplementedException();
        }

        public Task<Answer> GetAnswer(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<Answer?> UpdateAnswer(UpdateAnswerDto updatedAnswer)
        {
            throw new NotImplementedException();
        }
    }
}