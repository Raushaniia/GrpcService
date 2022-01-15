using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NewsStreaming.Core;
using GrpcService.Server;

namespace GrpcService.Services
{
	public class PostNewsService : Server.PostNewsService.PostNewsServiceBase
	{
		private IHttpClientFactory _httpClientFactory;

		public PostNewsService(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public override async Task<NewsResponse> GetNews(GetNewsForRequest request, ServerCallContext context)
		{
			var httpClient = _httpClientFactory.CreateClient();
			var temp = await GetCurrentNewsAsync(httpClient);

			Console.WriteLine(temp);

			var d = temp.News.First();
			Console.WriteLine(d);

			return new NewsResponse
			{
				Description = temp.News.First().Description,
				Title = temp.News.First().Title,
				Timestamp = Timestamp.FromDateTime(DateTime.UtcNow)
			};
		}

		public override async Task GetNewsStream(GetNewsForRequest request, IServerStreamWriter<NewsResponse> responseStream,
			ServerCallContext context)
		{
			var httpClient = _httpClientFactory.CreateClient();
			var contextCancellationToken = context.CancellationToken;

			while(true)
			{
				if (contextCancellationToken.IsCancellationRequested) return;

				var temp = await GetCurrentNewsAsync(httpClient);
				await responseStream.WriteAsync(
					new NewsResponse
					{
						Description = temp.News.First().Description,
						Title = temp.News.First().Title,
						Timestamp = Timestamp.FromDateTime(DateTime.UtcNow)
					});
				await Task.Delay(5000);
			}
		}

		private static async Task<NewsData> GetCurrentNewsAsync(HttpClient httpClient)
		{
			var response = await httpClient.GetStringAsync(
				//"");
				"https://api.currentsapi.services/v1/search?keywords=virus&limit=1&apiKey=-eUpWubJIoT_9BPcc072BRq3e2zHcYOID5q_ZfpZXoNsTqQ0");
			////await responseStream.WriteAsync(new TrackReply({Message = }));

			var temp = NewsData.FromJson(response);
			return temp;
		}
	}
}
