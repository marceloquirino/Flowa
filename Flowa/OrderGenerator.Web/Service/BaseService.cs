using Newtonsoft.Json;
using OrderGenerator.Web.Models;
using OrderGenerator.Web.Service.IService;
using OrderGenerator.Web.Utilities;
using System.Net;
using System.Text;

namespace OrderGenerator.Web.Service
{
    public class BaseService(IHttpClientFactory httpClientFactory) : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public async Task<ResponseDto?> SendAssync(RequestDto requestDto)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("FlowaAPI");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");

                if (string.IsNullOrWhiteSpace(requestDto?.Url))
                {
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "Url is null or empty"
                    };
                }

                message.RequestUri = new Uri(requestDto.Url);

                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage? apiResponse = null;

                switch (requestDto.ApiType)
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }
                apiResponse = await client.SendAsync(message);

                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "Não encontrado" };

                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Acesso negado" };

                    case HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "Não autorizado" };

                    case HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = "Erro interno no servidor" };

                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                }
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
