using MediatR;
using Newtonsoft.Json;
using ProductManager.Domain.Constansts;
using ProductManager.Domain.Entities.Currency;
using ProductManager.GenericResponse;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ProductManager.Features.Currency.Command
{
    public class ConvertCurrencyCommand : IRequest<Response<double>>
    {
        public CurrencyType From { get; set; }
        public CurrencyType To { get; set; }
        public double Price { get; set; }
    }

    public class ConvertCurrencyCommandHandler : IRequestHandler<ConvertCurrencyCommand, Response<double>>
    {
        public async Task<Response<double>> Handle(ConvertCurrencyCommand request, CancellationToken cancellationToken)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("apikey", "U7IUaa8rmQg1gjT0sBbCrYWIR4X5fUqS");
            var response = await client.GetStringAsync($"https://api.apilayer.com/currency_data/convert?to={request.To}&from={request.From}&amount={request.Price}");
            if (!string.IsNullOrEmpty(response))
            {
                var currencyLayer = JsonConvert.DeserializeObject<CurrencyLayer>(response);
                if (currencyLayer != null && currencyLayer.success == true)
                    return new Response<double>(ResultCode.Success, currencyLayer.result);
                else
                {
                    return new Response<double>(ResultCode.InternalServerError, 0, "Failed to Convert");
                }
            }
            else
            {
                return new Response<double>(ResultCode.InternalServerError, 0, "Failed to Convert");
            }
        }
    }
}
