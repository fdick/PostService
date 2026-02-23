using MassTransit;
using PostService.Core.Abstractions;
using PostService.Core.Models;
using PostService.RabbitMQ.Contracts;
using PostService.RabbitMQ.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostService.RabbitMQ.Consumers
{
    public class UsersConsumer : IConsumer<UserMQEvent>
    {
        private readonly IUserService _userService;

        public UsersConsumer(IUserService userService)
        {
            this._userService = userService;
        }
        public async Task Consume(ConsumeContext<UserMQEvent> context)
        {
            var user = User.Create(
                context.Message.ID,
                context.Message.Nickname,
                context.Message.Name,
                context.Message.LastName,
                context.Message.Email
                );

            if (!string.IsNullOrEmpty(user.Item2))
                throw new Exception();

            switch (context.Message.Operation)
            {
                case OperationTypes.Create:
                    var g = await _userService.CreateUserAsync(user.Item1);
                    break;
                case OperationTypes.Update:
                    await _userService.UpdateUserAsync(
                        user.Item1.ID,
                        user.Item1.Name,
                        user.Item1.LastName,
                        user.Item1.Email,
                        user.Item1.Nickname
                        );
                    break;
                case OperationTypes.Delete:
                    await _userService.DeleteUserAsync(user.Item1.ID);

                    break;
                default:
                    break;
            }

        }
    }
}
