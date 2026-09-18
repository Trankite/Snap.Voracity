using Common.Source.Service.Mission.Gradual.Interface;
using Common.Source.Web.Request.Builder.Abstraction;

namespace Common.Source.Web.Download.Abstract
{
    public interface IFileDownloader : IAsyncGradualTask
    {
        bool FreshDownload { get; }

        long DownloadBytes { get; }

        long FullFileBytes { get; }

        IHttpRequestMessageBuilderFactory BuilderFactory { get; }
    }
}