using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities.NotificationAggregate;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Notification;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : BaseApiController
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IService _service;
        private readonly IMapper _mapper;
        #endregion

        #region ctor
        public NotificationController(IUnitOfWork unitOfWork,
            IService service,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _service = service;
            _mapper = mapper;
        }
        #endregion
        //create
        //[AllowAnonymous]
        //[HttpPost("create")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> CreateNotification([FromBody] NotificationRequestModel model)
        //{
        //    try
        //    {
        //        var newNotification = new NotificationResponseModel();
        //        if (!ModelState.IsValid)
        //        {
        //            ValidModel();
        //        }
        //        //check if user is existing

        //        //else -> add answer
        //        var newNoti = new Notification()
        //        {
        //            Content = model.Content,
        //            IsChecked = model.IsChecked,
        //            Type = model.Type,
        //            UserId = model.UserId,
        //            TestId = model.TestId
        //        };
        //        await _service.NotificationService.AddAsync(newNoti);

        //        var responseModel = new NotificationResponseModel()
        //        {
        //            Id = newNoti.Id,
        //            Content = newNoti.Content,
        //            TimeStamp = newNoti.CreatedOnUtc,
        //            IsChecked = newNoti.IsChecked,
        //            Type = newNoti.Type,
        //            UserId = newNoti.UserId
        //        };


        //        return SuccessResult(responseModel, "Created Notification successfully.");
        //    }
        //    catch (Exception e)
        //    {
        //        return FailResult(e.Message);
        //    }
        //}

        ////[AllowAnonymous]
        //[HttpPost("create/global")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> CreateGlobalNotification([FromBody] GlobalNotificationRequestModel model)
        //{
        //    try
        //    {
        //        var newNotification = new NotificationResponseModel();
        //        if (!ModelState.IsValid)
        //        {
        //            ValidModel();
        //        }
        //        //check if user is existing

        //        //else -> add answer
        //        var admins = await _service.IdentityService.GetAllAdminPaging(1, 1);
        //        var admin = admins.FirstOrDefault();
        //        var newNoti = new Notification()
        //        {
        //            Title = model.Title,
        //            Content = model.Content,
        //            Type = 4,
        //            UserId = admin.Id
        //        };
        //        await _service.NotificationService.AddAsync(newNoti);

        //        var responseModel = new GlobalNotificationResponseModel()
        //        {
        //            Id = newNoti.Id,
        //            Content = newNoti.Content,
        //            TimeStamp = newNoti.CreatedOnUtc,
        //            Type = newNoti.Type,
        //            Title = newNoti.Title
        //        };
        //        return SuccessResult(responseModel, "Created Notification successfully.");
        //    }
        //    catch (Exception e)
        //    {
        //        return FailResult(e.Message);
        //    }
        //}

        ////get all globalNotification
        ////[AllowAnonymous]
        //[HttpGet("get/all/global")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> GetAll()
        //{
        //    var newTests = new List<GlobalNotificationResponseModel>();
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            ValidModel();
        //        }
        //        try
        //        {
        //            var newNotiList = await _service.NotificationService.GetAllAsync();
        //            try
        //            {
        //                var globalPreNotiList = newNotiList.Where(x => x.Type == 4).ToList().OrderByDescending(x=>x.CreatedOnUtc);

        //                var globalNotiList = globalPreNotiList.Take(10);

        //                foreach (var noti in globalNotiList)
        //                {
        //                    var newNotiModel = new GlobalNotificationResponseModel()
        //                    {
        //                        Id = noti.Id,
        //                        Content = noti.Content,
        //                        Type = noti.Type,
        //                        TimeStamp = noti.CreatedOnUtc,
        //                        Title = noti.Title
        //                    };
        //                    newTests.Add(newNotiModel);
        //                }
        //                return SuccessResult(newTests, "Get all Notifications successfully.");
        //            }
        //            catch (Exception e)
        //            {
        //                return FailResult(e.Message, "There's no global notification");
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            return FailResult(e.Message, "Error(s) occur(s) - failed in try catch");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return FailResult(e.Message, "Error(s) occur(s) - failed in try catch");
        //    }

        //}


        ////get by Id
        //[HttpGet("{id}")]
        ////[AllowAnonymous]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> GetNotificationById([FromRoute] int id)
        //{
        //    try
        //    {
        //        var notification = await _service.NotificationService.GetByIdAsync(id);
        //        if (notification == null)
        //        {
        //            return FailResult($"Can not found Notification with Id: {id}");
        //        }
        //        var notificationRes = new NotificationResponseModel()
        //        {
        //            Id = notification.Id,
        //            Content = notification.Content,
        //            TimeStamp = notification.CreatedOnUtc,
        //            IsChecked = notification.IsChecked,
        //            Type = notification.Type,
        //            UpdatedOnUtc = notification.UpdatedOnUtc,
        //            UserId = notification.UserId
        //        };
        //        return SuccessResult(notificationRes, "Get Notification successfully.");
        //    }
        //    catch (Exception e)
        //    {
        //        return FailResult(e.ToString());
        //    }
        //}

        ////get all by userId
        //[HttpGet("user/{userId}")]
        ////[AllowAnonymous]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> GetNotificationByUserId([FromRoute] int userId)
        //{
        //    try
        //    {
        //        var notificationsRes = new List<NotificationResponseModel>();
        //        //check is avtivated ? 
        //        bool isActivated = false;
        //        try
        //        {
        //            isActivated = await _service.IdentityService.CheckIsActive(userId);
        //        }
        //        catch( Exception e)
        //        {
        //            return SuccessResult(notificationsRes, "User has not been activated.");
        //        }

        //        //return only activated notification
        //        if (isActivated != true)
        //        {
        //            try
        //            {
        //                var notification = await _service.NotificationService.GetExpiredByUserIdAsync(userId);
        //                var newNotification = new NotificationResponseModel()
        //                {
        //                    Id = notification.Id,
        //                    Content = notification.Content,
        //                    TimeStamp = notification.CreatedOnUtc,
        //                    IsChecked = notification.IsChecked,
        //                    Type = notification.Type,
        //                    UpdatedOnUtc = notification.UpdatedOnUtc,
        //                    UserId = notification.UserId,
        //                };
        //                notificationsRes.Add(newNotification);
        //                return SuccessResult(notificationsRes, "Get Notifications successfully.");
        //            }
        //            catch (Exception)
        //            {
        //                return SuccessResult(notificationsRes, "User has not been activated. Cannot find expire notification.");
        //            }
        //        }
        //        else
        //        {
        //            var notifications = await _service.NotificationService.GetByUserIdAsync(userId);
        //            if (notifications == null)
        //            {
        //                return FailResult($"Can not found Notification with Id: {userId}");
        //            }
        //            foreach (var notification in notifications)
        //            {
        //                var newNotification = new NotificationResponseModel()
        //                {
        //                    Id = notification.Id,
        //                    Content = notification.Content,
        //                    TimeStamp = notification.CreatedOnUtc,
        //                    IsChecked = notification.IsChecked,
        //                    Type = notification.Type,
        //                    UpdatedOnUtc = notification.UpdatedOnUtc,
        //                    UserId = notification.UserId,
        //                };
        //                //check type
        //                try
        //                {
        //                    var test = await _service.TestService.GetByIdAsync(notification.TestId);
        //                    newNotification.TestType = (WebAPI.Models.Notification.NotificationTestType)test.Type;
        //                }
        //                catch (Exception e)
        //                {
        //                    newNotification.TestType = 0;
        //                }
        //                notificationsRes.Add(newNotification);
        //            }

        //            return SuccessResult(notificationsRes, "Get Notifications successfully.");
        //        }

                
        //    }
        //    catch (Exception e)
        //    {
        //        return FailResult(e.ToString());
        //    }
        //}

        ////get all not read


        ////get all have read


        ////update
        ////[AllowAnonymous]
        //[HttpPost("update")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> UpdateNotification([FromBody] NotificationResponseModel model)
        //{
        //    try
        //    {
        //        var notiOnServer = await _unitOfWork.NotificationRepository.GetByIdAsync(model.Id);

        //        var newNotification = new NotificationResponseModel();
        //        if (!ModelState.IsValid)
        //        {
        //            ValidModel();
        //        }

        //        // check notification model
        //        if (model.Type.ToString() != null)
        //        {
        //            notiOnServer.Type = model.Type;
        //        }
        //        if (model.IsChecked.ToString() != null)
        //        {
        //            notiOnServer.IsChecked = model.IsChecked;
        //        }
        //        if (model.Content != null)
        //        {
        //            notiOnServer.Content = model.Content;
        //        }

        //        await _service.NotificationService.UpdateAsync(notiOnServer);

        //        var responseModel = new NotificationResponseModel()
        //        {
        //            Id = notiOnServer.Id,
        //            Content = notiOnServer.Content,
        //            TimeStamp = notiOnServer.CreatedOnUtc,
        //            IsChecked = notiOnServer.IsChecked,
        //            Type = notiOnServer.Type,
        //            UserId = notiOnServer.UserId
        //        };


        //        return SuccessResult(responseModel, "Update Notification successfully.");
        //    }
        //    catch (Exception e)
        //    {
        //        return FailResult(e.Message);
        //    }
        //}

        ////create api update notification status
        ////[AllowAnonymous]
        //[HttpGet("update/ischecked/{id}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> UpdateNotificationIsChecked([FromRoute] int id)
        //{
        //    try
        //    {
        //        var notiOnServer = await _unitOfWork.NotificationRepository.GetByIdAsync(id);

        //        if (notiOnServer == null)
        //        {
        //            return FailResult("cannot get notification with id = " + id.ToString());
        //        }

        //        var newNotification = new NotificationResponseModel();
        //        if (!ModelState.IsValid)
        //        {
        //            ValidModel();
        //        }
        //        notiOnServer.IsChecked = true;
        //        await _service.NotificationService.DeleteAsync(notiOnServer);

        //        var responseModel = new NotificationResponseModel();
        //        return SuccessEmptyResult("Update Notification successfully.");
        //    }
        //    catch (Exception e)
        //    {
        //        return FailResult(e.Message);
        //    }
        //}

        ////create new test notification
        ////[AllowAnonymous]
        //[HttpDelete("{id}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> DeleteNotificationById([FromRoute] int id)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            ValidModel();
        //        }
        //        try
        //        {
        //            //get all userId
        //            var noti = await _service.NotificationService.GetByIdAsync(id);
        //            await _service.NotificationService.DeleteAsync(noti);
        //            return SuccessResult("delete finished!", "Delete Notifications for Test successfully.");
        //        }
        //        catch (Exception e)
        //        {
        //            return FailResult(e.Message);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return FailResult(e.Message);
        //    }
        //}

        //create api response fix answer
        ////[AllowAnonymous]
        //[HttpPost("create/fix")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> CreateNotificationFixAnswer([FromBody] NotificationRequestModel model)
        //{
        //    try
        //    {
        //        var newNotification = new NotificationResponseModel();
        //        if (!ModelState.IsValid)
        //        {
        //            ValidModel();
        //        }

        //        //create -> new
        //        var notiOnServer = new Notification()
        //        {
        //            Content = model.Content,
        //            IsChecked = false,
        //            Type = 2,
        //            //testId = model.TestId,
        //            testId = model.TestId,
        //            UserId = model.UserId
        //        };

        //        await _service.NotificationService.AddAsync(notiOnServer);

        //        var responseModel = new NotificationResponseModel()
        //        {
        //            Id = notiOnServer.Id,
        //            Content = notiOnServer.Content,
        //            CreatedOnUtc = notiOnServer.CreatedOnUtc,
        //            IsChecked = notiOnServer.IsChecked,
        //            Type = notiOnServer.Type,
        //            UserId = notiOnServer.UserId
        //        };


        //        return SuccessResult(responseModel, "Create Notification successfully.");
        //    }
        //    catch (Exception e)
        //    {
        //        return FailResult(e.Message);
        //    }
        //}

        //create new test notification
        ////[AllowAnonymous]
        //[HttpPost("create/global")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> CreateNotificationNewTest([FromBody] NotificationRequestCreateModel model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            ValidModel();
        //        }
        //        //get all userId
        //        var ids = await _service.IdentityService.GetAllStudentIds();

        //        //foreach -> create notification Maps to userId
        //        if (ids.Count() <= 0)
        //        {
        //            return FailResult("students.count =0 ", "cannot find any student");
        //        }

        //        //create notification to return
        //        var newNotifications = new List<NotificationResponseModel>();

        //        //foreach user id -> create new notification
        //        foreach (var id in ids)
        //        {
        //            //create -> new
        //            var notiOnServer = new Notification()
        //            {
        //                Content = model.Content,
        //                IsChecked = false,
        //                Type = 1,
        //                //testId = model.TestId,
        //                testId = model.TestId,
        //                UserId = id
        //            };

        //            await _service.NotificationService.AddAsync(notiOnServer);

        //            var responseModel = new NotificationResponseModel()
        //            {
        //                Id = notiOnServer.Id,
        //                Content = notiOnServer.Content,
        //                CreatedOnUtc = notiOnServer.CreatedOnUtc,
        //                IsChecked = notiOnServer.IsChecked,
        //                Type = notiOnServer.Type,
        //                UserId = notiOnServer.UserId
        //            };
        //            newNotifications.Add(responseModel);
        //        }
        //        return SuccessResult(newNotifications, "Create Notifications for new Test successfully.");
        //    }
        //    catch (Exception e)
        //    {
        //        return FailResult(e.Message);
        //    }
        //}
    }
}
