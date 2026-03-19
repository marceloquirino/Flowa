using OrderGenerator.Web.Models;

namespace OrderGenerator.Web.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAssync(RequestDto requestDto);
    }
}
