using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.MultipleChoicesAnswer
{
    public class MultipleChoicesAnswerResponseModel
    {
        public int Id { get; set; }
        public int MultipleChoicesExerciseId { get; set; }
        public int AnswerId { get; set; }
        public string Result { get; set; }
        public bool IsBingo { get; set; }
        public int Score { get; set; }
    }
    public class MultipleChoicesAnswerRequestModel
    {
        public int MultipleChoicesExerciseId { get; set; }
        public int AnswerId { get; set; }
        public string Result { get; set; }
        public bool IsBingo { get; set; }
    }
}
