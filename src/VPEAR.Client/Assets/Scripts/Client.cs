using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VPEAR.Core;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Wrappers;

public class Client : IVPEARClient
{
    private const string c_scheme = "Bearer";
    private const string c_prefix = "/api/v1";
    private readonly string _baseAddress;
    private readonly HttpClient _client;
    private Exception _error;
    private string _errorMessage;
    private string _name;
    private string _password;
    private string _token;
    private DateTimeOffset _expiresAt;

    public Client(string baseAddress, HttpClient client)
    {
        _baseAddress = baseAddress ?? throw new ArgumentNullException(nameof(baseAddress));
        _client = client ?? throw new ArgumentNullException(nameof(client));

        if (Uri.TryCreate(baseAddress, UriKind.Absolute, out var uri))
        {
            _client.BaseAddress = uri;
        }
        else
        {
            throw new ArgumentException("Is not a valid Uri.", nameof(baseAddress));
        }
    }

    public Exception Error
    {
        get => _error;
    }

    public string ErrorMessage
    {
        get => _errorMessage;
    }

    public HttpResponseMessage Response => throw new NotSupportedException();

    public async Task<bool> CanConnectAsync()
    {
        var uri = $"{c_prefix}";
        using var response = await _client.GetAsync(uri);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteDeviceAsync(string deviceId)
    {
        var uri = $"{c_prefix}/device?id={deviceId}";

        return await DeleteAsync(uri);
    }

    public async Task<bool> DeleteUserAsync(string name)
    {
        var uri = $"{c_prefix}/user?name={name}";

        return await DeleteAsync(uri);
    }

    public void Dispose()
    {
        _client?.Dispose();
    }

    public async Task<Container<GetDeviceResponse>> GetDevicesAsync(DeviceStatus? status = null)
    {
        var uri = $"{c_prefix}/device";

        if (status != null)
        {
            uri += $"?status={status}";
        }

        return await GetAsync<Container<GetDeviceResponse>>(uri);
    }

    public async Task<GetFiltersResponse> GetFiltersAsync(string deviceId)
    {
        var uri = $"{c_prefix}/device/filter?id={deviceId}";

        return await GetAsync<GetFiltersResponse>(uri);
    }

    public async Task<GetFirmwareResponse> GetFirmwareAsync(string deviceId)
    {
        var uri = $"{c_prefix}/device/firmware?id={deviceId}";

        return await GetAsync<GetFirmwareResponse>(uri);
    }

    public async Task<Container<GetFrameResponse>> GetFramesAsync(string deviceId, int? start, int? count)
    {
        var uri = $"{c_prefix}/device/frames?id={deviceId}";

        if (start != null)
        {
            uri += $"&start={start}";
        }

        if (count != null)
        {
            uri += $"&count={count}";
        }

        return await GetAsync<Container<GetFrameResponse>>(uri);
    }

    public async Task<GetPowerResponse> GetPowerAsync(string deviceId)
    {
        var uri = $"{c_prefix}/device/power?id={deviceId}";

        return await GetAsync<GetPowerResponse>(uri);
    }

    public async Task<Container<GetSensorResponse>> GetSensorsAsync(string deviceId)
    {
        var uri = $"{c_prefix}/device/sensors?id={deviceId}";

        return await GetAsync<Container<GetSensorResponse>>(uri);
    }

    public async Task<Container<GetUserResponse>> GetUsersAsync(string role = null)
    {
        var uri = $"{c_prefix}/user";

        if (role != null)
        {
            uri += $"?role={role}";
        }

        return await GetAsync<Container<GetUserResponse>>(uri);
    }

    public async Task<GetWifiResponse> GetWifiAsync(string deviceId)
    {
        var uri = $"{c_prefix}/device/wifi?id={deviceId}";

        return await GetAsync<GetWifiResponse>(uri);
    }

    public async Task<bool> LoginAsync(string name, string password)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Is null or empty.", nameof(name));
        }

        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Is null or empty.", nameof(password));
        }

        var uri = $"{c_prefix}/user/login";
        var payload = new PutLoginRequest()
        {
            Name = name,
            Password = password,
        };
        using var response = await _client.PutAsJsonAsync(uri, payload);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<PutLoginResponse>();

            _name = name;
            _password = password;
            _token = result.Token;
            _expiresAt = result.ExpiresAt;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(c_scheme, _token);

            return true;
        }
        else
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            _errorMessage = string.Join(" ", error.Messages);

            return false;
        }
    }

    public void Logout()
    {
        _name = string.Empty;
        _password = string.Empty;
        _token = string.Empty;
        _expiresAt = default;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(c_scheme);
    }

    public async Task<bool> PostDevicesAsync(string address, string subnetMask)
    {
        var uri = $"{c_prefix}/device";
        var payload = new PostDeviceRequest()
        {
            Address = address,
            SubnetMask = subnetMask,
        };

        return await PostAsync(uri, payload);
    }

    public async Task<bool> PutDeviceAsync(string deviceId, string displayName = null, int? frequency = null, int? requiredSensors = null, DeviceStatus? status = null)
    {
        var uri = $"{c_prefix}/device?id={deviceId}";
        var payload = new PutDeviceRequest()
        {
            DisplayName = displayName,
            Frequency = frequency,
            RequiredSensors = requiredSensors,
            Status = status,
        };

        return await PutAsync(uri, payload);
    }

    public async Task<bool> PutFiltersAsync(string deviceId, bool? spot, bool? smooth, bool? noise)
    {
        var uri = $"{c_prefix}/device/filter?id={deviceId}";
        var payload = new PutFilterRequest()
        {
            Noise = noise,
            Smooth = smooth,
            Spot = spot,
        };

        return await PutAsync(uri, payload);
    }

    public async Task<bool> PutFirmwareAsync(string deviceId, string source, string upgrade, bool package = false)
    {
        var uri = $"{c_prefix}/device/firmware?id={deviceId}";
        var payload = new PutFirmwareRequest()
        {
            Package = package,
            Source = source,
            Upgrade = upgrade,
        };

        return await PutAsync(uri, payload);
    }

    public async Task<bool> PutUserAsync(string name, string oldPassword = null, string newPassword = null, bool isVerified = false)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Is null or empty.", nameof(name));
        }

        var uri = $"{c_prefix}/user?name={name}";
        var payload = new PutUserRequest()
        {
            IsVerified = isVerified,
            NewPassword = newPassword,
            OldPassword = oldPassword,
        };

        return await PutAsync(uri, payload);
    }

    public async Task<bool> PutWifiAsync(string deviceId, string ssid, string password, string mode = null)
    {
        var uri = $"{c_prefix}/device/wifi?id={deviceId}";
        var payload = new PutWifiRequest()
        {
            Mode = mode,
            Password = password,
            Ssid = ssid,
        };

        return await PutAsync(uri, payload);
    }

    public async Task<bool> RegisterAsync(string name, string password, bool isAdmin = false)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Is null or empty.", nameof(name));
        }

        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Is null or empty.", nameof(password));
        }

        var uri = $"{c_prefix}/user/register";
        var payload = new PostRegisterRequest()
        {
            Name = name,
            Password = password,
            IsAdmin = isAdmin,
        };
        using var response = await _client.PostAsJsonAsync(uri, payload);

        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            _errorMessage = string.Join(" ", error.Messages);

            return false;
        }
    }

    private async Task<TResult> GetAsync<TResult>(string uri)
    {
        try
        {
            await CheckTokenAsync();

            using var response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TResult>();
            }
            else
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                _errorMessage = string.Join(" ", error.Messages);

                return default;
            }
        }
        catch (Exception exception)
        {
            _error = exception;
            _errorMessage = exception.Message;

            return default;
        }
    }

    private async Task<bool> DeleteAsync(string uri)
    {
        try
        {
            await CheckTokenAsync();

            using var response = await _client.DeleteAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                _errorMessage = string.Join(" ", error.Messages);

                return default;
            }
        }
        catch (Exception exception)
        {
            _error = exception;
            _errorMessage = exception.Message;

            return default;
        }
    }

    private async Task<bool> PostAsync<TPayload>(string uri, TPayload payload)
    {
        try
        {
            await CheckTokenAsync();

            using var response = await _client.PostAsJsonAsync(uri, payload);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                _errorMessage = string.Join(" ", error.Messages);

                return default;
            }
        }
        catch (Exception exception)
        {
            _error = exception;
            _errorMessage = exception.Message;

            return default;
        }
    }

    private async Task<bool> PutAsync<TPayload>(string uri, TPayload payload)
    {
        try
        {
            await CheckTokenAsync();

            using var response = await _client.PutAsJsonAsync(uri, payload);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                _errorMessage = string.Join(" ", error.Messages);

                return default;
            }
        }
        catch (Exception exception)
        {
            _error = exception;
            _errorMessage = exception.Message;

            return default;
        }
    }

    private async Task CheckTokenAsync()
    {
        if (DateTime.UtcNow > _expiresAt)
        {
            await LoginAsync(_name, _password);
        }
    }
}
