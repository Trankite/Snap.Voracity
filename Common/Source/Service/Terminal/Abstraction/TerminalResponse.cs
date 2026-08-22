using Common.Source.Extension;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Service.Terminal.Abstraction
{
    public class TerminalResponse : ITerminalResponse
    {
        public bool Success { get; }

        public string Message { get; } = string.Empty;

        public TerminalResponse() { }

        public TerminalResponse(bool success)
        {
            Success = success;
        }

        public TerminalResponse(bool success, string message) : this(success)
        {
            Message = message;
        }

        public override string ToString() => Message;

        public static TerminalResponse Create(ITerminalResponse response)
        {
            return new TerminalResponse(response.Success, response.Message);
        }

        public static TerminalResponse<TContent> Create<TContent>(bool success, TContent? content = default)
        {
            return new TerminalResponse<TContent>(success, content);
        }

        public static TerminalResponse<TContent> Create<TContent>(bool success, string message, TContent? content = default)
        {
            return new TerminalResponse<TContent>(success, message, content);
        }

        public static TerminalResponse<TContent> Create<TContent>(ITerminalResponse response, TContent? content = default)
        {
            return new TerminalResponse<TContent>(response.Success, response.Message, content);
        }
    }

    public class TerminalResponse<TContent> : TerminalResponse, ITerminalResponse<TContent>
    {
        public TContent? Content { get; set; }

        public TerminalResponse() { }

        public TerminalResponse(bool success) : base(success) { }

        public TerminalResponse(bool success, string message) : base(success, message) { }

        public TerminalResponse(ITerminalResponse response) : base(response.Success, response.Message) { }

        public TerminalResponse(bool success, TContent? content) : base(success)
        {
            Content = content;
        }

        public TerminalResponse(bool success, string message, TContent? content) : this(success, message)
        {
            Content = content;
        }

        public bool TryGetAnalyzedBody([NotNullWhen(true)] out TContent? analyedBody)
        {
            return (analyedBody = Content).IsNotNull() && Success;
        }
    }
}