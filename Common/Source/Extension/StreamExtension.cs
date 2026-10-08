using Common.Source.Resource.Localization;
using System.Diagnostics;

namespace Common.Source.Extension
{
    public static class StreamExtension
    {
        [DebuggerStepThrough]
        public static async Task CopyToAsync(this Stream source, Stream destination, long bytesPerSecond, TimeSpan checkTimeSpan, CancellationToken cancellationToken = default)
        {
            using CancellationTokenSource LinkedTokenSource = cancellationToken.CreateLinkedTokenSource();
            Task StreamTrackTask = TrackStreamSpeed(destination, bytesPerSecond, checkTimeSpan, LinkedTokenSource.Token);
            Task TrackCancelTask = LinkedTokenSource.CancelAfterAsync(StreamTrackTask);
            try
            {
                await source.CopyToAsync(destination, LinkedTokenSource.Token).ConfigureAwait(false);
            }
            catch (OperationCanceledException) { return; }
            finally
            {
                LinkedTokenSource.Cancel();
                await TrackCancelTask.ConfigureAwait(false);
            }
        }

        [DebuggerStepThrough]
        public static async Task TrackStreamSpeed(this Stream stream, long bytesPerSecond, TimeSpan checkTimeSpan, CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                long CurrentPosition;
                try
                {
                    long OriginalPosition = stream.Position;
                    await Task.Delay(checkTimeSpan, cancellationToken).ConfigureAwait(false);
                    CurrentPosition = stream.Position - OriginalPosition;
                }
                catch { return; }
                if (CurrentPosition < bytesPerSecond * checkTimeSpan.TotalSeconds)
                {
                    throw new TimeoutException(LocalString.ExtensionStreamCopyTimeoutException.SafeFormat(CurrentPosition, checkTimeSpan.TotalSeconds));
                }
            }
        }
    }
}