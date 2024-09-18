namespace Edent.Api.ViewModels
{
    public class ApiResult<TResult>
    {
        public bool Succeeded { get; set; }
        public TResult Result { get; set; }
    }
}
