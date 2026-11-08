using Common.Source.Model.DataStruct.Ticket;

namespace Common.Source.Web.EHentai.Download
{
    public sealed class EHentaiDownloadInfo
    {
        public HttpDownloader Downloader { get; set; }

        public QueueTicket<string> ImageTicket { get; set; }

        public EHentaiDownloadInfo(HttpDownloader downloader, QueueTicket<string> imageTicket)
        {
            Downloader = downloader;
            ImageTicket = imageTicket;
        }
    }
}