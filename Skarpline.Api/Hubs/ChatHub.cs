using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.SignalR;
using Skarpline.Api.OAuth;
using Skarpline.Common.Results;
using Skarpline.Dependencies;
using Skarpline.Entities.Domain.Messages;
using Skarpline.Entities.Models.Messages;
using Skarpline.Services;

namespace Skarpline.Api.Hubs
{
    /// <summary>
    /// Chat SignalR Hub
    /// </summary>
    public class ChatHub : Hub
    {
        /// <summary>
        /// Adds some logic when user is connected to chat
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            return base.OnConnected();
        }

        /// <summary>
        /// Adds some logic when user exits the chat
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// Sends message to chat
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="userId">Sender Id</param>
        /// <param name="userName">Sender name</param>
        public void SendMessage(string message, string userId, string userName)
        {
            IService<Message> messageService = IoC.Container.GetInstance<IService<Message>>();
            var messageItem = new Message
            {
                MessageBody = message,
                UserId = userId,
                Date = DateTime.UtcNow
            };
            ServiceResult<Message> serviceResult = messageService.Create(messageItem);
            if (!serviceResult.Succeeded) throw new ApplicationException(serviceResult.ErrorMessage);
            MessageModel messageModel = Mapper.Map<MessageModel>(serviceResult.Result);
            messageModel.Sender = userName;
            Clients.All.addMessage(messageModel);
        }

        /// <summary>
        /// Handle start typing event
        /// </summary>
        /// <param name="userName"></param>
        public void StartTyping(string userName)
        {
            if(string.IsNullOrWhiteSpace(userName)) return;
            Clients.All.startTypig(userName);
        }

        /// <summary>
        /// Handle stop typing event
        /// </summary>
        /// <param name="userName"></param>
        public void StopTyping(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) return;
            Clients.All.stopTypig(userName);
        }
    }
}