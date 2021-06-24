using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Answer
{
    public class AnswerEditorModel
    {
        public string ResultFixed { get; set; }
        public double Score { get; set; }
    }
    public class AnswerEditorContainsModel
    {
        public int EssayAnswerId { get; set; }
        public int Score { get; set; }
        public string ResultFixed { get; set; }
    }
    public class AnswerEditorModelList
    {
        public List<AnswerEditorContainsModel> AnswerEditorModels { get; set; }
    }
}
