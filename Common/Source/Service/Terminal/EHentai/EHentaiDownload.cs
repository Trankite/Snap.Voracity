using Common.Source.Extension;
using Common.Source.Factory.Streams.Text.Abstract;
using Common.Source.Factory.Streams.Text.Interface;
using Common.Source.Factory.Streams.Text.Write;
using Common.Source.Resource.Localization;
using Common.Source.Service.Mission.Gradual.Abstract;
using Common.Source.Service.Mission.Gradual.Metadata;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Interface;
using Common.Source.Service.Terminal.Metadata;
using Common.Source.Web.EHentai;
using Common.Source.Web.EHentai.Download;
using Common.Source.Web.EHentai.Metadata;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Common.Source.Service.Terminal.EHentai
{
    public class EHentaiDownload : AsyncTerminalCommand
    {
        public override string Name => "ehdown";

        public override string FullName => LocalString.ServiceTerminalEHentaiEHentaiDownloadFullName;

        public override string Help => LocalString.ServiceTerminalEHentaiEHentaiDownloadHelp;

        public override ImmutableArray<string> RequiredParameters { get; } = [Param_GalleryLink];

        public override ImmutableArray<string> OptionalParameters { get; } = [Param_IpbMemberId, Param_PathOpen];

        private const string Param_GalleryLink = "link";

        private const string Param_IpbMemberId = TerminalParameters.Id;

        private const string Param_PathOpen = TerminalParameters.PathOpen;

        public override async ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine, ITextStream? textStream = null, CancellationToken cancellationToken = default)
        {
            if (textStream.IsNull())
            {
                return SupportTerminalResponse.MissingUserInteraction();
            }
            string galleryLink = commandLine.GetParameter(Param_GalleryLink);
            if (!Uri.TryCreate(galleryLink, UriKind.Absolute, out Uri? galleryUri))
            {
                return SupportTerminalResponse.UnlawfulParameter();
            }
            string ipbMemberId = commandLine.GetParameter(Param_IpbMemberId);
            bool pathOpen = commandLine.GetBoolParameter(Param_PathOpen);
            return await AsyncInvoke(galleryUri, textStream, ipbMemberId, pathOpen, cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<ITerminalResponse> AsyncInvoke(Uri galleryUri, ITextStream textStream, string? ipbMemberId = default, bool pathOpen = default, CancellationToken cancellationToken = default)
        {
            EHentaiHost ehentaiHost = EHentaiUriHelper.GetHost(galleryUri);
            if (ehentaiHost == EHentaiHost.None)
            {
                return SupportTerminalResponse.UnlawfulParameter();
            }
            EHentaiToken? ehentaiToken = default;
            if (ehentaiHost == EHentaiHost.ExHentai && !EHentaiTokenManage.TryGetTokenOrFirst(ipbMemberId, out ehentaiToken))
            {
                return SupportTerminalResponse.NotFindToken(ipbMemberId);
            }
            TextStreamSource<ITextReader, StopTextWriter>? stopTextStream = default;
            if (textStream.IsNotNull())
            {
                stopTextStream = TextStreamSource.Create(textStream.Reader, new StopTextWriter(textStream.Writer, Stopwatch.StartNew()));
            }
            EHentaiDownloader downloader = EHentaiDownloader.Create(galleryUri, ehentaiToken, stopTextStream);
            while ((await downloader.StartOrRetryAsync(cancellationToken).ConfigureAwait(false)).Status.IsRunnable())
            {
                stopTextStream?.SourceWriter.Stopwatch.Stop();
                if (!await textStream.EnquireAsync(LocalString.ServiceTerminalEHentaiEHentaiDownloadRetry, cancellationToken).ConfigureAwait(false))
                {
                    return new TerminalResponse(false, LocalString.ServiceTerminalEHentaiEHentaiDownloadCanceled);
                }
                stopTextStream?.SourceWriter.Stopwatch.Start();
            }
            if (downloader.Status.IsCompleted())
            {
                return new TerminalResponse(true, FileHelper.PathOpen(downloader.FolderPath, pathOpen));
            }
            return new TerminalResponse(false, downloader.GetExceptionMessage());
        }
    }
}