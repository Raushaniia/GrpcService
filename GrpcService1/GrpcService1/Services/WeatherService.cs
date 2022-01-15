using System.Text.Json;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcService.Server;
using MusicStreaming.Core;

namespace GrpcService.Services
{
	public class WeatherService : Server.WeatherService.WeatherServiceBase
	{
		private IHttpClientFactory _httpClientFactory;

		public WeatherService(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;

		}

		public override async Task<WeatherResponse> GetCurrentWeather(GetCurrentWeatherforCityRequest request, ServerCallContext context)
		{
			var httpClient = _httpClientFactory.CreateClient();
			var temp = await GetCurrentTemperaturesAsync(httpClient);

			Console.WriteLine(temp);

			var d = temp.List.First().Main.Temp;
			Console.WriteLine(d);

			return new WeatherResponse
			{
				Temperature = temp!.List.First().Main.Temp,
				FeelsLike = temp.List.First().Main.FeelsLike,
				Timestamp = Timestamp.FromDateTime(DateTime.UtcNow)
			};


		}

		public override async Task GetCurrentWeatherStream(GetCurrentWeatherforCityRequest request, IServerStreamWriter<WeatherResponse> responseStream,
			ServerCallContext context)
		{
			var httpClient = _httpClientFactory.CreateClient();
			var ct = new CancellationToken();

			for (int i = 0; i < 30; i++)
			{
				var temp = await GetCurrentTemperaturesAsync(httpClient);
				await responseStream.WriteAsync(
					new WeatherResponse
					{
						Temperature = temp!.List.First().Main.Temp,
						FeelsLike = temp.List.First().Main.FeelsLike,
						Timestamp = Timestamp.FromDateTime(DateTime.UtcNow)
					});
				await Task.Delay(1000);
			}
		}

		private static async Task<Wheather> GetCurrentTemperaturesAsync(HttpClient httpClient)
		{
			var response = await httpClient.GetStringAsync(
				"");
			////await responseStream.WriteAsync(new TrackReply({Message = }));

			var temp = Wheather.FromJson(response);
			return temp;
		}
	}
}
