namespace MewingPad.Utils.AudioManager;

public class AudioManager
{
    private static readonly HttpClient _client = new() { BaseAddress = new Uri("http://192.168.0.10:9877/") };

    public static async Task<bool> GetFileAsync(string srcpath, string savepath)
    {
        try
        {
            using (var stream = await _client.GetStreamAsync($"audiofiles/{srcpath}"))
            using (var fileStream = new FileStream(Path.Combine(savepath, srcpath), FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                await stream.CopyToAsync(fileStream);
            }
            Console.WriteLine($"File downloaded to: {savepath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading: {ex.Message}");
            return false;
        }
        return true;
    }

    public static async Task<bool> CreateFileAsync(string filepath)
    {
        try
        {
            using var fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            using var content = new MultipartFormDataContent
            {
                { new StreamContent(fileStream), "audio", Path.GetFileName(filepath) }
            };

            using var response = await _client.PostAsync("audiofiles", content);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("File uploaded successfully");
            }
            else
            {
                Console.WriteLine($"Failed to upload file. Status: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error uploading: {ex.Message}");
            return false;
        }
        return true;
    }

    public static async Task<bool> DeleteFileAsync(string filepath)
    {
        try
        {
            HttpResponseMessage response = await _client.DeleteAsync($"audiofiles/{filepath}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"File '{filepath}' deleted successfully");
            }
            else
            {
                Console.WriteLine($"Failed to delete file '{filepath}'. Status: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting file: {ex.Message}");
            return false;
        }
        return true;
    }

    public static async Task<bool> UpdateFileAsync(string filepath, string newFilepath)
    {
        try
        {
            var newFilename = Path.GetFileName(newFilepath);
            using var fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            using var content = new MultipartFormDataContent
            {
                { new StreamContent(fileStream), "audio", newFilename }
            };

            HttpResponseMessage response = await _client.PutAsync($"audiofiles/{newFilename}", content);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"File '{newFilename}' updated successfully");
            }
            else
            {
                Console.WriteLine($"Failed to update file '{newFilename}'. Status: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating file: {ex.Message}");
            return false;
        }
        return true;
    }
}
