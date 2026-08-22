namespace Common.Source.Web.Response
{
    public interface IResponseMessage
    {
        int Code { get; set; }

        string Message { get; set; }
    }
}