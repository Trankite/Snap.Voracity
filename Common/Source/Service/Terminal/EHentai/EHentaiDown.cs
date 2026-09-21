using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Mission.Gradual.Abstract;
using Common.Source.Service.Mission.Gradual.Metadata;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Metadata;
using Common.Source.Web.EHentai;
using Common.Source.Web.EHentai.Download;
using Common.Source.Web.EHentai.Metadata;

namespace Common.Source.Service.Terminal.EHentai
{
    public class EHentaiDown : AsyncTerminalCommand
    {
        public override string Name => "ehdown";

        public override string FullName => LocalString.ServiceTerminalEHentaiEHentaiDownFullName;

        public override string Help => LocalString.ServiceTerminalEHentaiEHentaiDownHelp;

        public override string[] RequiredParameters => [Param_GalleryLink];

        public override string[] OptionalParameters => [Param_IpbMemberId, Param_PathOpen];

        private const string Param_GalleryLink = "link";

        private const string Param_IpbMemberId = TerminalParameters.Id;

        private const string Param_PathOpen = TerminalParameters.PathOpen;

        public override async ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = null, CancellationToken cancellationToken = default)
        {
            if (linkedStream.IsNull())
            {
                return SupportTerminalResponse.MissingUserInteraction();
            }
            string GalleryLink = commandLine.GetParameter(Param_GalleryLink);
            if (!Uri.TryCreate(GalleryLink, UriKind.Absolute, out Uri? GalleryUri))
            {
                return SupportTerminalResponse.UnlawfulParameter();
            }
            string IpbMemberId = commandLine.GetParameter(Param_IpbMemberId);
            bool PathOpen = commandLine.GetBoolParameter(Param_PathOpen);
            return await AsyncInvoke(GalleryUri, linkedStream, IpbMemberId, PathOpen, cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<ITerminalResponse> AsyncInvoke(Uri galleryUri, ILinkedTextStream linkedStream, string? ipbMemberId = default, bool pathOpen = default, CancellationToken cancellationToken = default)
        {
            EHentaiHost EHentaiHost = EHentaiUriHelper.GetHost(galleryUri);
            if (EHentaiHost == EHentaiHost.None)
            {
                return SupportTerminalResponse.UnlawfulParameter();
            }
            EHentaiToken? EHentaiToken = default;
            if (EHentaiHost == EHentaiHost.ExHentai && !EHentaiTokenManage.TryGetTokenOrFirst(ipbMemberId, out EHentaiToken))
            {
                return SupportTerminalResponse.NotFindToken(ipbMemberId);
            }
            using EHentaiDownloader Downloader = EHentaiDownloader.Create(galleryUri, EHentaiToken, linkedStream);
            while ((await Downloader.StartOrRetryAsync(cancellationToken).ConfigureAwait(false)).IsUnCompleted())
            {
                if (!await linkedStream.EnquireAsync(LocalString.ServiceTerminalEHentaiEHentaiDownloadRetry, cancellationToken).ConfigureAwait(false))
                {
                    return new TerminalResponse(false, LocalString.ServiceTerminalEHentaiEHentaiDownloadCanceled);
                }
            }
            if (Downloader.States == GradualStates.Completed)
            {
                return new TerminalResponse(true, FileHelper.PathOpen(Downloader.FolderPath, pathOpen));
            }
            return new TerminalResponse(false, Downloader.GetMessage());
        }
    }
}