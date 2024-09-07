using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DrawingTest.Interop;

public class EventSource
{
    public EventSource(string url)
    {
        this.url = url;

        cts = new CancellationTokenSource();
        client = new HttpClient();
        Task.Run(run, cts.Token);
    }

    public readonly string url;
    private readonly CancellationTokenSource cts;
    private readonly HttpClient client;
    public readonly bool withCredentials;

    public const ushort CONNECTING = 0;
    public const ushort OPEN = 1;
    public const ushort CLOSED = 2;
    public ushort readyState { get; private set; }

    public dynamic? onopen;
    public dynamic? onmessage;
    public dynamic? onerror;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1005", Justification = "")]
    private async void run()
    {
        cts.Token.ThrowIfCancellationRequested();

        try
        {
            using var streamReader = new StreamReader(await client.GetStreamAsync(url, cts.Token));

            if (onopen != null)
                onopen();

            while (!streamReader.EndOfStream && !cts.Token.IsCancellationRequested)
            {
                var message = await streamReader.ReadLineAsync(cts.Token);
                if (onmessage != null)
                    onmessage(message);
            }
        }
        catch (Exception ex)
        {
            if (onerror != null)
                onerror(ex);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822", Justification = "")]
    public void close()
    {
        cts.Cancel();
        client.Dispose();
    }
}
