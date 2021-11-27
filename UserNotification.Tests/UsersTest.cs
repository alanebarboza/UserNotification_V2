using System;
using System.Collections.Generic;
using System.Linq;
using UserNotification.Domain.Commands;
using UserNotification.Domain.Validators;
using UserNotification.Shared;
using UserNotification.Shared.Entities;
using Xunit;

namespace UserNotification.Tests
{
    public class UsersTest
    {
        [Fact]
        public void CreateUsersCommandTest_ReturnTrue()
        {
            var userNOtification = new UpdateUsersNotificationCommand(0, NotificationTypeEnum.Bills, "Description Test", 123, null, DateTime.Now.AddDays(10), null, "", NotificationNotifyEnum.EmailAndPhone);
            var list = new List<UpdateUsersNotificationCommand>();
            list.Add(userNOtification);

            var user = new CreateUsersCommand( "UserTest", "123456", "User Test", "17991188445",  "new@new.com", list);
            var validator = new CreateUsersCommandValidator();
            var validatorResult = validator.Validate(user);

            Assert.True(validatorResult.IsValid, string.Join(",", validatorResult.Errors.Select(v => v.ErrorMessage).ToList()));
        }

        [Fact]
        public void UpdateUsersCommandTest_ReturnTrue()
        {
            var userNOtification = new UpdateUsersNotificationCommand(0, NotificationTypeEnum.Bills, "Description Test - Changed", 123, null, DateTime.Now.AddDays(10), null, "", NotificationNotifyEnum.EmailAndPhone);
            var list = new List<UpdateUsersNotificationCommand>();
            list.Add(userNOtification);

            var user = new UpdateUsersCommand(1,"UserTest - Changed", "123456 - Changed", "User Test - Changed", "17991188445", "changed@change.com", list);
            var validator = new UpdateUsersCommandValidator();
            var validatorResult = validator.Validate(user);

            Assert.True(validatorResult.IsValid, string.Join(",", validatorResult.Errors.Select(v => v.ErrorMessage).ToList()));
        }

        [Fact]
        public void UpdateUsersNotificationTest_ReturnTrue()
        {
            var userNOtification = new UpdateUsersNotificationCommand(0, NotificationTypeEnum.Bills, "Description Test - Changed", 123, null, DateTime.Now.AddDays(10), null, "", NotificationNotifyEnum.EmailAndPhone);
            
            var validator = new UpdateUsersNotificationCommandValidator();
            var validatorResult = validator.Validate(userNOtification);

            Assert.True(validatorResult.IsValid, string.Join(",", validatorResult.Errors.Select(v => v.ErrorMessage).ToList()));
        }

        [Fact]
        public void LoginTest_ReturnTrue()
        {
            var login = new LoginCommand("userName", "password");
            var validator = new LoginCommandValidator();
            var validatorResult = validator.Validate(login);

            Assert.True(validatorResult.IsValid, string.Join(",", validatorResult.Errors.Select(v => v.ErrorMessage).ToList()));
        }

        [Fact]
        public void EmailTest_ReturnTrue()
        {
            var email = new EmailUsersCommand("123456789", "email@email.com", NotificationTypeEnum.Bills, "Test", 123, null, DateTime.Now.AddDays(10), NotificationNotifyEnum.Email);
            var validator = new EmailUsersCommandValidator();
            var validatorResult = validator.Validate(email);

            Assert.True(validatorResult.IsValid, string.Join(",", validatorResult.Errors.Select(v => v.ErrorMessage).ToList()));
        }
    }
}
