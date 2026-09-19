using Common.Source.Model.DataStruct.Ticket;
using Common.Source.Web.Download;

namespace Common.Source.Web.EHentai.Download
{
    public sealed class EHentaiDownloadInfo : IDisposable
    {
        public FileDownloader Downloader { get; set; }

        public QueueTicket<string> ImageTicket { get; set; }

        public EHentaiDownloadInfo(FileDownloader downloader, QueueTicket<string> imageTicket)
        {
            Downloader = downloader;
            ImageTicket = imageTicket;
        }

        public void Dispose()
        {
            Downloader.Dispose();
        }
    }
}