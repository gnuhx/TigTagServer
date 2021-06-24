using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models.EssayAnswer;
using WebAPI.Models.MultipleChoicesAnswer;

namespace WebAPI.Models.Answer
{
    public class AnswerResponseModel
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public double Score { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public class FixedAnswerResponseModel
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public double Score { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string TestName { get; set; }
    }

    public class AnswerUnfixedResponseModel
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public double Score { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime DateCreate { get; set; }
        public string Email { get; set; }
        public string TestName { get; set; }
    }
    public class AnswerRequestModel
    {
        public int TestId { get; set; }
        public double Score { get; set; }
        public int UserId { get; set; }
    }
    public class AnswerContainerResponseModel
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public double Score { get; set; }
        public int UserId { get; set; }
        public List<MultipleChoicesAnswerResponseModel> MultipleChoicesAnswerResponseModels { get; set; }
        public List<EssayAnswerResponseModel> EssayAnswerResponseModels { get; set; }
        public DateTime TimeStamp { get; set; }
    }
    public class AnswerContainerRequestModel
    {
        public int TestId { get; set; }
        public double Score { get; set; }
        public int UserId { get; set; }
        public List<MultipleChoicesAnswerRequestModel> MultipleChoicesAnswerRequestModels { get; set; }
        public List<EssayAnswerRequestModel> EssayAnswerRequestModels { get; set; }
    }
}
