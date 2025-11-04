using System.Net;

namespace knowledgeBase.Middleware;

public class StaticFilesMiddleware : IMiddleware
{
    private readonly string _staticFilesDirectory;
    private readonly string[] _staticFilesExtensions;

    public StaticFilesMiddleware(string staticFilesDirectory = "public")
    {
        _staticFilesDirectory = staticFilesDirectory;
        _staticFilesExtensions = new string[] {
            ".css", ".js", ".png", ".jpg", ".jpeg", ".gif", ".ico",
            ".svg", ".woff", ".woff2", ".ttf", ".eot", ".json", ".xml",
            ".txt", ".pdf", ".zip", ".mp4", ".webm", ".mp3"
        };
        
        if (!Directory.Exists(_staticFilesDirectory))
        {
            Directory.CreateDirectory(_staticFilesDirectory);
        }
    }
    public async Task InvokeAsync(HttpListenerContext context, Func<Task> next)
    {
        var request = context.Request;
        var response = context.Response;

        if (IsStaticFileRequest(request) && TryGetStaticFilePath(request, out string filePath))
        {
            if (File.Exists(filePath))
            {
                await ServeStaticFile(context, filePath);
                return;
            }
            else
            {
                throw new FileNotFoundException($"Static file not found: {Path.GetFileName(filePath)}");
            }
        }
        
        await next();
    }

    public bool IsStaticFileRequest(HttpListenerRequest request)
    {
        if (request.HttpMethod != "GET")
        {
            return false;
        }
        
        var path = request.Url.AbsolutePath;

        foreach (var extension in _staticFilesExtensions)
        {
            if (path.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        
        return false;
    }

    public bool TryGetStaticFilePath(HttpListenerRequest request, out string filePath)
    {
        filePath = null;
        var requestPath = request.Url.AbsolutePath;

        try
        {
            var relativePath = requestPath.TrimStart('/');
            filePath = Path.Combine(_staticFilesDirectory, relativePath);
            filePath = Path.GetFullPath(filePath);

            if (!filePath.StartsWith(Path.GetFullPath(_staticFilesDirectory)))
            {
                filePath = null;
                return false;
            }

            return true;
        }
        catch
        {
            filePath = null;
            return false;
        }
    }

    public async Task ServeStaticFile(HttpListenerContext context, string filePath)
    {
        var response = context.Response;

        try
        {
            var fileBytes = File.ReadAllBytes(filePath);
            var contentType = Path.GetExtension(filePath).ToLowerInvariant() switch
            {
                ".css" => "text/css",
                ".js" => "application/javascript",
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".gif" => "image/gif",
                ".ico" => "image/x-icon",
                ".svg" => "image/svg+xml",
                ".woff" => "font/woff",
                ".woff2" => "font/woff2",
                ".ttf" => "font/ttf",
                ".html" => "text/html",
                ".json" => "application/json",
                ".xml" => "application/xml",
                ".txt" => "text/plain",
                ".pdf" => "application/pdf",
                ".zip" => "application/zip",
                ".mp4" => "video/mp4",
                ".webm" => "video/webm",
                ".mp3" => "audio/mp3",
                _ => "application/octet-stream"
            };
            
            response.ContentType = contentType;
            response.ContentLength64 = fileBytes.Length;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.OutputStream.Write(fileBytes, 0, fileBytes.Length);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error serving static file: {ex.Message}");
        }
    }
}