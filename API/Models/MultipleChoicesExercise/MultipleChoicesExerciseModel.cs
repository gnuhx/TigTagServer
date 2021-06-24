using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.MultipleChoicesExercise
{
    public class MultipleChoicesExerciseResponseModel
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public string Title { get; set; }
        public string RightResult { get; set; }
        public string FalseResult1 { get; set; }
        public string FalseResult2 { get; set; }
        public string FalseResult3 { get; set; }
        public string Image { get; set; }
    }
    public class MultipleChoicesExerciseResponseCompetitiveModel
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public string Title { get; set; }
        public string FalseResult1 { get; set; }
        public string FalseResult2 { get; set; }
        public string FalseResult3 { get; set; }
        public string FalseResult4 { get; set; }
        public string Image { get; set; }
    }
    public class MultipleChoicesExerciseRequestModel
    {
        public int TestId { get; set; }
        public string Title { get; set; }
        public string RightResult { get; set; }
        public string FalseResult1 { get; set; }
        public string FalseResult2 { get; set; }
        public string FalseResult3 { get; set; }
        public string Image { get; set; }
    }
    public class MultipleChoicesExerciseUpdateRequestModel
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public string Title { get; set; }
        public string RightResult { get; set; }
        public string FalseResult1 { get; set; }
        public string FalseResult2 { get; set; }
        public string FalseResult3 { get; set; }
        public string Image { get; set; }
    }
}
