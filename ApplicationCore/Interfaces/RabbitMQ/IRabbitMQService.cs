using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.RabbitMQ
{
    public interface IRabbitMQService
    {
        Task<bool> SendMessageAsync(RabbitMqMessageModel messageModel);
    }
}
