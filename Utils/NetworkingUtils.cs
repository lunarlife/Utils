using System.Net;

namespace Utils;

public static class NetworkingUtils
{
    public static void DownloadFile(string link, string file, bool deleteIfExists = false, Action<int>? precentUpdate = null)
    {
        using var wc = new WebClient();
        while (File.Exists(file))
        {
            try
            {
                using var stream = File.Open(file, FileMode.Open, FileAccess.Read);
                stream.Close();
                break;
            }
            catch
            {
                Thread.Sleep(1);
                throw;
            }
        }
        if (deleteIfExists && File.Exists(file)) File.Delete(file);
        if(precentUpdate is not null)
            wc.DownloadProgressChanged += (sender, e) => precentUpdate.Invoke(e.ProgressPercentage);
        wc.DownloadFile(link, file);
    }
    public static void DownloadFileAsync(string link, string file, bool deleteIfExists = false, Action<int> precentUpdate = null, Action complete = null)
    {
        using var wc = new WebClient();
        while (File.Exists(file))
        {
            try
            {
                using var stream = File.Open(file, FileMode.Open, FileAccess.Read);
                stream.Close();
                break;
            }
            catch
            {
                Thread.Sleep(1);
                throw;
            }
        }
        if (deleteIfExists && File.Exists(file)) File.Delete(file);
        if (precentUpdate is not null)
            wc.DownloadProgressChanged += (sender, e) => precentUpdate.Invoke(e.ProgressPercentage);
        if (complete is not null)
            wc.DownloadFileCompleted += (sender, e) =>
            {
                wc.Dispose();
                complete.Invoke();
            };
         wc.DownloadFileAsync(new Uri(link), file);
    }
}