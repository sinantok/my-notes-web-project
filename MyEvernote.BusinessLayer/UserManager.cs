using MyEvernote.DataAccesLayer.EntityFramework;
using MyEvernote.Entities;
using MyEvernote.Entities.ViewModels;
using MyEvernote.Entities.ResultModel;
using MyEvernote.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEvernote.Common.Helpers;
using MyEvernote.BusinessLayer.Abstract;

namespace MyEvernote.BusinessLayer
{
    public class UserManager : ManagerBase<EUser>
    {
        //for register user
        public MessageResult<EUser> RegisterUser(RegisterViewModel model)
        {
            MessageResult<EUser> messageResult = new MessageResult<EUser>();

            messageResult.Result = Find(x => x.Username == model.Username || x.Email == model.Email);

            if (messageResult.Result != null)
            {
                if (messageResult.Result.Username == model.Username)
                {
                    messageResult.AddError(ErrorMessageCodes.UserNameAlreadyExists, "UserName already Exists!");
                }
                if (messageResult.Result.Email == model.Email)
                {
                    messageResult.AddError(ErrorMessageCodes.EmailAdresAlreadyExists, "E-Mail address already exists!");
                }
            }
            else
            {
                DateTime now = DateTime.Now;
                int dbResult = base.Insert(new EUser()
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password,
                    ActiveGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false,
                    ProfileImageFileName = "DefaultUser.jpg"
                });

                if (dbResult > 0)
                {
                    messageResult.Result = Find(x => x.Username == model.Username && x.Email == model.Email);

                    //send to mail for active
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActive/{messageResult.Result.ActiveGuid}";
                    string body = $"Hello {messageResult.Result.Username};<br><br>" +
                        $"To active your account <a href='{activateUri}' target='_blank'>click here</a>";
                    MailHelper.SendMail(body, messageResult.Result.Email, "MyNote Account Activation");
                }
            }

            return messageResult;

        }

        //login
        public MessageResult<EUser> LoginUser(LoginViewModel model)
        {
            MessageResult<EUser> messageResult = new MessageResult<EUser>();
            messageResult.Result = Find(x => x.Username == model.Username && x.Password == model.Password);

            if (messageResult.Result == null)
            {
                messageResult.AddError(ErrorMessageCodes.UserNameorPassWrong, "Username or password wrong!");
            }
            else
            {
                if (!messageResult.Result.IsActive)
                {
                    messageResult.AddError(ErrorMessageCodes.UserIsnotActive, "User is not active");
                    messageResult.AddError(ErrorMessageCodes.CheckYourEmail, "Please check your E-Mail.");
                }
            }

            return messageResult;
        }

        //Activate
        public MessageResult<EUser> ActivateUser(Guid guid)
        {
            MessageResult<EUser> messageResult = new MessageResult<EUser>();
            messageResult.Result = Find(x => x.ActiveGuid == guid);
            if (messageResult.Result != null)
            {
                if (messageResult.Result.IsActive)
                {
                    messageResult.AddError(ErrorMessageCodes.UserAlreadyActive, "User already active.");
                    return messageResult;
                }
                messageResult.Result.IsActive = true;
                Update(messageResult.Result);
            }
            else
            {
                messageResult.AddError(ErrorMessageCodes.ActivateIdDoesNotExists, "Activate id does not exists.");
            }

            return messageResult;
        }

        //search by id
        public MessageResult<EUser> GetUserById(int id)
        {
            MessageResult<EUser> messageResult = new MessageResult<EUser>();
            messageResult.Result = Find(x => x.Id == id);
            if (messageResult.Result == null)
            {
                messageResult.AddError(ErrorMessageCodes.UserNotFound, "User not found.");
            }
            return messageResult;
        }

        //Profile update
        public MessageResult<EUser> UpdateProfile(EUser model)
        {
            MessageResult<EUser> res = new MessageResult<EUser>();
            res.Result = Find(x => x.Id != model.Id && (x.Username == model.Username || x.Email == model.Email));

            if (res.Result != null && res.Result.Id != model.Id)
            {
                if (res.Result.Username == model.Username)
                {
                    res.AddError(ErrorMessageCodes.UserNameAlreadyExists, "UserName already exists.");
                }

                if (res.Result.Email == model.Email)
                {
                    res.AddError(ErrorMessageCodes.EmailAdresAlreadyExists, "E-Mail already exists.");
                }

                return res;
            }

            res.Result = Find(x => x.Id == model.Id);
            res.Result.Email = model.Email;
            res.Result.Name = model.Name;
            res.Result.Surname = model.Surname;
            res.Result.Password = model.Password;
            res.Result.Username = model.Username;

            if (string.IsNullOrEmpty(model.ProfileImageFileName) == false)
            {
                res.Result.ProfileImageFileName = model.ProfileImageFileName;
            }

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCodes.ProfileCouldNotUpdated, "Profile could not be updated.");
            }

            return res;
        }

        public MessageResult<EUser> RemoveUserById(int id)
        {
            MessageResult<EUser> res = new MessageResult<EUser>();
            res.Result = Find(x => x.Id == id);
            if(res.Result == null)
            {
                res.AddError(ErrorMessageCodes.UserCouldNotFind, "User could not find.");
                return res;
            }

            if (Delete(res.Result) == 0)
            {
                res.AddError(ErrorMessageCodes.UserCouldNotRemove, "User could not be deleted.");
            }
            return res;
        }

        //Hiding method  //for admin-user-insert
        public new MessageResult<EUser> Insert(EUser model)
        {
            MessageResult<EUser> messageResult = new MessageResult<EUser>();
            
            messageResult.Result = Find(x => x.Username == model.Username || x.Email == model.Email);

            if (messageResult.Result != null)
            {
                if (messageResult.Result.Username == model.Username)
                {
                    messageResult.AddError(ErrorMessageCodes.UserNameAlreadyExists, "You cannot re-register with a registered Username.");
                }
                if (messageResult.Result.Email == model.Email)
                {
                    messageResult.AddError(ErrorMessageCodes.EmailAdresAlreadyExists, "You cannot re-register with a registered E-mail address.");
                }
            }
            else
            {
                messageResult.Result = model;
                messageResult.Result.ProfileImageFileName = "DefaultUser.jpg";
                messageResult.Result.ActiveGuid = Guid.NewGuid();
               
                if (base.Insert(messageResult.Result) == 0 )
                {
                    messageResult.AddError(ErrorMessageCodes.UserCouldNotInserted, "User could not be saved");
                }
            }

            return messageResult;
        }

        public new MessageResult<EUser> Update(EUser model)
        {
            MessageResult<EUser> res = new MessageResult<EUser>();
            res.Result = Find(x => x.Id != model.Id && (x.Username == model.Username || x.Email == model.Email));

            if (res.Result != null && res.Result.Id != model.Id)
            {
                if (res.Result.Username == model.Username)
                {
                    res.AddError(ErrorMessageCodes.UserNameAlreadyExists, "User name already exists.");
                }

                if (res.Result.Email == model.Email)
                {
                    res.AddError(ErrorMessageCodes.EmailAdresAlreadyExists, "E-Mail address already exists.");
                }

                return res;
            }

            res.Result = Find(x => x.Id == model.Id);
            res.Result.Email = model.Email;
            res.Result.Name = model.Name;
            res.Result.Surname = model.Surname;
            res.Result.Password = model.Password;
            res.Result.Username = model.Username;
            res.Result.IsActive = model.IsActive;
            res.Result.IsAdmin = model.IsAdmin;

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCodes.UserCouldNotUpdated, "User could not be update.");
            }

            return res;
        }
    }
}
