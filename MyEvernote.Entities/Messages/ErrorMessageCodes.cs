
namespace MyEvernote.Entities.Messages
{
    public enum ErrorMessageCodes
    {
        UserNameAlreadyExists = 101,
        EmailAdresAlreadyExists = 102,
        UserIsnotActive = 201,
        UserAlreadyActive = 202,
        UserNameorPassWrong = 203,
        CheckYourEmail = 301,
        ActivateIdDoesNotExists =302,
        UserNotFound = 304,
        ProfileCouldNotUpdated = 305,
        UserCouldNotRemove = 306,
        UserCouldNotFind = 307,
        CategoryCouldNotRemove = 308,
        CategoryCouldNotFind = 309,
        UserCouldNotInserted = 310,
        UserCouldNotUpdated = 311,
        NoteCouldNotFind = 312,
        NoteCouldNotRemove = 313,
        CommentCouldNotFind = 314,
        CommentCouldNotRemove = 315
    }
}
