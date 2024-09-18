namespace Edent.Api.Models.PlayMobile.Enums
{
    public enum ErrorType
    {
        InternalServerError = 100,
        SyntaxError = 101,
        AccountLockError = 102,
        EmptyChannel = 103,
        InvalidPriority = 104,
        TooMuchIDs = 105,
        EmptyRecipient = 202,
        EmptyEmailAddress = 204,
        EmptyMessageId = 205,
        InvalidVariables = 206,
        InvalidLocalTime = 301,
        InvalidStartDateTime = 302,
        InvalidEndDateTime = 303,
        InvolidAllowedStartTime = 304,
        InvalidAllowedEndTime = 305,
        InvalidSendEvenly = 306,
        EmptyOriginator = 401,
        EmptyApplication = 402,
        EmptyTTL = 403,
        EmptyContent = 404,
        ContentError = 405,
        InvalidContent = 406,
        InvalidTTL = 407,
        InvalidAttachedFiles = 408,
        InvalidRetryAttempts = 410,
        InvalidRetryTimeout = 411
    }
}
