using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BaseApiController : ControllerBase
    {
        /// <summary>
        /// prepare error result
        /// </summary>
        /// <param name="errorMessages"></param>
        /// <returns></returns>
        [NonAction]
        public IActionResult ErrorResult(List<string> errorMessages)
        {
            var dataResult = new
            {
                Status = false,
                ErrorMessages = errorMessages
            };
            return BadRequest(dataResult);
        }

        /// <summary>
        /// prepare error result
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        [NonAction]
        public IActionResult ErrorResult(string errorMessage)
        {
            var errorMessages = new List<string> { errorMessage };
            var dataResult = new
            {
                Status = false,
                ErrorMessages = errorMessages
            };
            return BadRequest(dataResult);
        }

        /// <summary>
        /// prepare error result
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        [NonAction]
        public IActionResult ErrorResult(Exception error)
        {
            var errorMessages = new List<string> { error.Message };
            var dataResult = new
            {
                Status = false,
                ErrorMessages = errorMessages
            };
            return BadRequest(dataResult);
        }

        /// <summary>
        /// prepare error result
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        [NonAction]
        public IActionResult ErrorResult(string errorMessage, int errorCode)
        {
            var dataResult = new
            {
                Status = false,
                ErrorMessages = errorMessage,
                ErrorCode = errorCode
            };
            return BadRequest(dataResult);
        }

        /// <summary>
        /// prepare success result
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [NonAction]
        public IActionResult SuccessResult(string message)
        {
            var dataResult = new
            {
                Status = true,
                Message = message
            };
            return Ok(dataResult);
        }
        [NonAction]
        public IActionResult SuccessEmptyResult(string message)
        {
            var dataResult = new
            {
                Status = true,
                Message = "Success",
                Data = message
            };
            return Ok(dataResult);
        }


        /// <summary>
        /// prepare success result
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [NonAction]
        public IActionResult SuccessResult(object obj, string message = "")
        {
            var dataResult = new
            {
                Status = true,
                Message = message,
                Data = obj,
            };
            return Ok(dataResult);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [NonAction]
        public IActionResult FailResult(object obj, string message = "")
        {
            var dataResult = new
            {
                Status = false,
                Message = message,
                Data = obj
            };
            return BadRequest(dataResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [NonAction]
        public IActionResult FailResult(object resultMessage)
        {
            var dataResult = new
            {
                Status = false,
                Message = "Failed - Maybe at verify (get failed) return from savechange(s) error",
                Data= resultMessage
            };
            return BadRequest(dataResult);
        }

        /// <summary>
        /// File Result
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        [NonAction]
        public IActionResult FileResult(byte[] file, string fileName, string contentType)
        {
            if (file == null) return ErrorResult("No results were found for your selections.");
            return File(file, contentType);
        }

        /// <summary>
        /// ValidModel
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public IActionResult ValidModel()
        {
            foreach (var value in ModelState.Keys.Where(value => ModelState[value].Errors.Count > 0))
            {
                return ErrorResult(!string.IsNullOrEmpty(ModelState[value].Errors[0].ErrorMessage)
                    ? ModelState[value].Errors[0].ErrorMessage
                    : ModelState[value].Errors[0].Exception.Message);
            }

            return ErrorResult("Request model invalid");
        }

    }
}