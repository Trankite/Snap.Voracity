using Common.Source.Web.Response.Interface;

namespace Common.Source.Service.Terminal.Interface
{
    public interface ITerminalResponse : IResponseMessage, IResponseSuccess { }

    public interface ITerminalResponse<TContent> : ITerminalResponse, IResponseContent<TContent> { }
}