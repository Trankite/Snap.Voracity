using System.IO;

namespace StarRailDamage.Source.Service.Terminal.Abstraction
{
    public abstract class TerminalCommand : ITerminalCommand, IDisposable
    {
        private TextWriter _Writer;

        private TextReader _Reader;

        protected bool IsWriterRedirected;

        protected bool IsReaderRedirected;

        public abstract string Name { get; }

        public abstract string FullName { get; }

        public abstract string Help { get; }

        public abstract string[] RequiredParameters { get; }

        public abstract string[] OptionalParameters { get; }

        public CancellationToken CancellationToken { get; set; }

        public TextWriter Writer
        {
            get => _Writer;
            set
            {
                if (!ReferenceEquals(_Writer, value))
                {
                    _Writer = value;
                    IsWriterRedirected = true;
                }
            }
        }

        public TextReader Reader
        {
            get => _Reader;
            set
            {
                if (!ReferenceEquals(_Reader, value))
                {
                    _Reader = value;
                    IsReaderRedirected = true;
                }
            }
        }

        public TerminalCommand()
        {
            _Writer = Console.Out;
            _Reader = Console.In;
        }

        public abstract ITerminalResponse Invoke(ITerminalCommandLine commandLine);

        public void Dispose()
        {
            if (IsWriterRedirected)
            {
                Writer?.Dispose();
            }
            if (IsReaderRedirected)
            {
                Reader?.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}