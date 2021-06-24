using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models.EssayExercise;
using WebAPI.Models.MultipleChoicesExercise;

namespace WebAPI.Models.Test
{
    public class TestModel
    {
        public int Id { get; set; }
        public TestType Type { get; set; }
        public string Name { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Utility { get; set; }
    }

    public class TestCompetitiveResponseModel
    {
        public List<TestModel> RemainExam { get; set; }
        public List<TestModel> DoneExam { get; set; }
    }
    public class TestRequestModel
    {
        public TestType Type { get; set; }
        public string Name { get; set; }
        public string Utility { get; set; }
    }

    public class TestCreateModel
    {
        public TestType Type { get; set; }
        public string Name { get; set; }
        public string Utility { get; set; }
    }
    public enum TestType
    {
        Mathematic = 1,
        Science = 2,
        History = 3,
        Geographic = 4,
        Social = 5,
        English = 6,
        SampleTest = 7,
        Competitive = 8
    }
    public class TestContainerResponseModel
    {
        public int Id { get; set; }
        public TestType Type { get; set; }
        public string Name { get; set; }
        public string Utility { get; set; }
        public List<MultipleChoicesExerciseResponseModel> MultipleChoicesExerciseResponseModels { get; set; }
        public List<EssayExerciseResponseModel> EssayExerciseResponseModels { get; set; }

    }

    public class TestContainerResponseCompetitiveModel
    {
        public int Id { get; set; }
        public TestType Type { get; set; }
        public string Name { get; set; }
        public string Utility { get; set; }
        public List<MultipleChoicesExerciseResponseCompetitiveModel> MultipleChoicesExerciseResponseModels { get; set; }
        public List<EssayExerciseResponseCompetitiveModel> EssayExerciseResponseModels { get; set; }
    }
    public class TestContainerRequestModel
    {
        public TestType Type { get; set; }
        public string Name { get; set; }
        public string Utility { get; set; }
        public List<MultipleChoicesExerciseRequestModel> MultipleChoicesExerciseRequestModels  { get; set; }
        public List<EssayExerciseRequestModel> EssayExerciseRequestModels { get; set; }
    }
    public class TestContainerUpdateRequestModel
    {
        public int Id { get; set; }
        public TestType Type { get; set; }
        public string Name { get; set; }
        public string Utility { get; set; }
        public List<MultipleChoicesExerciseUpdateRequestModel> MultipleChoicesExerciseUpdateRequestModels { get; set; }
        public List<EssayExerciseUpdateRequestModel> EssayExerciseUpdateRequestModels { get; set; }
    }

}
