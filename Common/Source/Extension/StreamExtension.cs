using Common.Source.Model.DataStruct.Disk;
using Common.Source.Resource.Localization;
using System.Diagnostics;

namespace Common.Source.Extension
{
    public static class StreamExtension
    {
        [DebuggerStepThrough]
        public static async Task CopyToAsync(this Stream source, Stream destination, long bytesPerSecond, TimeSpan checkTimeSpan, CancellationToken cancellationToken = default)
        {
            using CancellationTokenSource linkedTokenSource = cancellationToken.CreateLinkedTokenSource();
            Task streamTrackTask = TrackStreamSpeed(destination, bytesPerSecond, checkTimeSpan, linkedTokenSource.Token);
            Task trackCancelTask = linkedTokenSource.CancelAfterTaskCompletedAsync(streamTrackTask);
            try
            {
                await source.CopyToAsync(destination, linkedTokenSource.Token).ConfigureAwait(false);
            }
            catch (OperationCanceledException) { return; }
            finally
            {
                linkedTokenSource.Cancel();
                await trackCancelTask.ConfigureAwait(false);
            }
        }

        [DebuggerStepThrough]
        public static async Task TrackStreamSpeed(this Stream stream, long bytesPerSecond, TimeSpan checkTimeSpan, CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                long currentPosition;
                try
                {
                    long originalPosition = stream.Position;
                    await Task.Delay(checkTimeSpan, cancellationToken).ConfigureAwait(false);
                    currentPosition = stream.Position - originalPosition;
                }
                catch { return; }
                if (currentPosition < bytesPerSecond * checkTimeSpan.TotalSeconds)
                {
                    throw new TimeoutException(LocalString.ExtensionStreamCopyTimeoutException.SafeFormat(new DiskSize(currentPosition), checkTimeSpan.TotalSeconds));
                }
            }
        }
    }
}