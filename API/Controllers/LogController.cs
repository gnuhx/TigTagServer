using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities.LogAggregate;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Log;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : BaseApiController
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IService _service;
        private readonly IMapper _mapper;
        #endregion

        #region ctor
        public LogController(IUnitOfWork unitOfWork,
            IService service,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _service = service;
            _mapper = mapper;
        }
        #endregion
        #region check if existing, add, update, delete
        //create Log
        /// <summary>
        /// <response code=200>Request completed Successfully</response>
        /// <response code=400>Bad Request</response>
        /// <response code=401>Don't have permission</response>
        /// <response code=500>Internal server error</response>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[AllowAnonymous]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateLog([FromBody] LogModel model)
        {
            var newLog = new Log();
            try
            {
                if (!ModelState.IsValid)
                {
                    ValidModel();
                }
                newLog = new Log()
                {
                    Action = model.Action,
                    Status = model.Status
                };
                await _service.LogService.AddAsync(newLog);
            }
            catch (Exception e)
            {
                return FailResult(e.Message);
            }
            return SuccessResult(newLog, "Created Log successfully.");
        }

        //get Log by id
        [HttpGet("{id}")]
        //[AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLogById([FromRoute] int id)
        {
            var log = await _service.LogService.GetByIdAsync(id);
            if (log == null)
            {
                return FailResult($"Can not found Log with Id: {id}");
            }
            var logRes = new LogModel
            {
                Action = log.Action,
                Id = log.Id,
                Status = log.Status
            };
            return SuccessResult(logRes, "Get Log successfully.");
        }
        #endregion

        #region get all

        #endregion
    }
}
