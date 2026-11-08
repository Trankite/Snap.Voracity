using Common.Source.Extension;
using Common.Source.Model.DataStruct.Disk;
using Common.Source.Resource.Localization;

namespace Common.Source.Service.Streams
{
    public sealed class StreamTracker : IDisposable
    {
        private bool Disposed;

        private readonly Stream Stream;

        private readonly bool LeaveOpen;

        public TimeSpan CheckTimeSpan { get; }

        public DiskSize SecondSize { get; }

        public StreamTracker(Stream stream, DiskSize secondSize, TimeSpan checkTimeSpan, bool leaveOpen = default)
        {
            Stream = stream;
            SecondSize = secondSize;
            CheckTimeSpan = checkTimeSpan;
            LeaveOpen = leaveOpen;
        }

        public async Task CopyAsync(Stream source, CancellationToken cancellationToken = default)
        {
            using CancellationTokenSource linkedTokenSource = cancellationToken.CreateSource();
            Task streamCopyTask = linkedTokenSource.CancelAfterTaskAsync(source.CopyToAsync(Stream, linkedTokenSource.Token));
            while (!linkedTokenSource.IsCancellationRequested)
            {
                DiskSize currentSecondSize;
                try
                {
                    currentSecondSize = await GetSpeedAsync(linkedTokenSource.Token).ConfigureAwait(false);
                }
                catch
                {
                    break;
                }
                if (currentSecondSize < SecondSize)
                {
                    linkedTokenSource.Cancel();
                    try
                    {
                        await streamCopyTask.ConfigureAwait(false);
                    }
                    catch (Exception exception)
                    {
                        throw GetTimeoutException(currentSecondSize, exception);
                    }
                    return;
                }
            }
            await streamCopyTask.ConfigureAwait(false);
        }

        private async Task<DiskSize> GetSpeedAsync(CancellationToken cancellationToken = default)
        {
            long position = Stream.Position;
            await Task.Delay(CheckTimeSpan, cancellationToken).ConfigureAwait(false);
            return new DiskSize((int)((Stream.Position - position) / CheckTimeSpan.TotalSeconds));
        }

        private static TimeoutException GetTimeoutException(DiskSize currentSecondSize, Exception? innerException = default)
        {
            return new TimeoutException(LocalString.ServiceStreamsTrackerExceptionTimeout.SafeFormat(currentSecondSize), innerException);
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                if (!LeaveOpen)
                {
                    Stream.Dispose();
                }
                Disposed = true;
            }
        }
    }
}