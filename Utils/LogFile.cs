using System.IO.Compression;
using System.Text;

namespace Utils;

public sealed class LogFile : IDisposable
{
    private readonly FileStream _stream;
    private readonly string _lastLog;
    private const string LatestLogName = "latest";
    private readonly object _writeLock = new();
    private readonly string _folder;

    public LogFile(string folder)
    {
        _folder = folder;
        if (!Directory.Exists(_folder)) Directory.CreateDirectory(_folder);
        _lastLog = Path.Combine(_folder, LatestLogName + ".log");
        int i = 0;
        while (File.Exists(_lastLog))
        {
            try
            {
                using var stream = File.Open(_lastLog, FileMode.Open, FileAccess.Read);
                stream.Close();
                break;
            }
            catch 
            {
                _lastLog = Path.Combine(_folder, $"{LatestLogName}{i}.log");
                i++;
            }
        }
        if (File.Exists(_lastLog))
             File.Delete(_lastLog);

        _stream = File.Open(_lastLog, FileMode.OpenOrCreate, FileAccess.Write);
    }

    public void Write(object obj)
    {
        lock (_writeLock)
        {
            _stream.Write(Encoding.UTF8.GetBytes($"{obj}\n"));
            _stream.Flush();
        }
    }

    public void Close()
    {
        lock (_writeLock)
        {
            _stream.Close();
        }
        var name = GenerateZipFileName();
        using var stream = File.OpenWrite(name);
        using var zip = new ZipArchive(stream, ZipArchiveMode.Create);
        var entry = zip.CreateEntry(LatestLogName + ".log");
        using var open = entry.Open();
        open.Write(File.ReadAllBytes(_lastLog));
    }

    private string GenerateZipFileName()
    {
        var now = DateTime.Now;
        var s = Path.Combine(_folder, $"{now:dd_mm_yy}-{now.Hour:00}_{now.Minute:00}-log");
        var i = 0;
        while (File.Exists(s + ".zip"))
        {
            if (i == 0) s += i;
            else s = s[..^1] + i;
            i++;
        }
        return s + ".zip";
    }
    public void Dispose()
    {
        Close();
    }
}