﻿using Serilog;
using Microsoft.Extensions.Configuration;

namespace MewingPad.Utils.AudioManager;

public class AudioManager
{
    private readonly HttpClient _client;
    private readonly ILogger _logger = Log.ForContext<AudioManager>();
    private readonly IConfiguration _config;

    public AudioManager(IConfiguration config)
    {
        _config = config;
        _client = new HttpClient { BaseAddress = new Uri(config["ApiSettings:AudioServerAddress"]!) };
    }

    public async Task<bool> GetFileAsync(string srcpath, string savepath)
    {
        _logger.Verbose("Entering GetFileAsync method");

        try
        {
            using var stream = await _client.GetStreamAsync($"audiofiles/{srcpath}");
            using var fileStream = new FileStream(
                Path.Combine(savepath, srcpath),
                FileMode.Create,
                FileAccess.Write,
                FileShare.Read);
            await stream.CopyToAsync(fileStream);
        }
        catch (Exception ex)
        {
            _logger.Error("Exception occurred", ex);
            return false;
        }

        _logger.Information($"File '{srcpath}' saved to '{savepath}'");
        _logger.Verbose("Exiting GetFileAsync method");
        return true;
    }

    public async Task<bool> CreateFileAsync(string filepath)
    {
        _logger.Verbose("Entering CreateFileAsync method");

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
                _logger.Information($"File '{filepath}' uploaded successfully");
            }
            else
            {
                _logger.Error($"Failed to upload file '{filepath}'. Status: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error creating file: {NewPath}. Reason: {ErrorMessage}", filepath, ex.Message);
            return false;
        }

        _logger.Verbose("Exiting CreateFileAsync method");
        return true;
    }

    public async Task<bool> DeleteFileAsync(string filepath)
    {
        _logger.Verbose("Entering DeleteFileAsync method");

        try
        {
            HttpResponseMessage response = await _client.DeleteAsync($"audiofiles/{filepath}");
            if (response.IsSuccessStatusCode)
            {
                _logger.Information($"File '{filepath}' deleted successfully");
            }
            else
            {
                _logger.Error($"Failed to delete file '{filepath}'. Status: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error("Exception occurred", ex);
            return false;
        }

        _logger.Verbose("Exiting DeleteFileAsync method");
        return true;
    }

    public async Task<bool> UpdateFileAsync(string filepath, string newFilepath)
    {
        _logger.Verbose("Entering UpdateFileAsync method");

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
                _logger.Information($"File '{newFilename}' updated successfully");
            }
            else
            {
                _logger.Error($"Failed to update file '{newFilename}'. Status: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error("Exception occurred", ex);
            return false;
        }

        _logger.Verbose("Exiting UpdateFileAsync method");
        return true;
    }
}