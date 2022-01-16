using System.Web;
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
			var temp = await GetCurrentNewsAsync(httpClient, request.KeyWords);

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

			var newsForLastDay = await GetCurrentNewsAsync(httpClient, request.KeyWords);

			foreach (var news in newsForLastDay.News)
			{
				await responseStream.WriteAsync(
					new NewsResponse
					{
						Description = news.Description,
						Title = news.Title,
						Timestamp = Timestamp.FromDateTime(DateTime.UtcNow)
					});
			}


			while (true)
			{
				if (contextCancellationToken.IsCancellationRequested) return;

				await Task.Delay(5000);

				var temp = await GetLatestNewsAsync(httpClient, request.KeyWords);

				foreach (var news in temp.News)
				{
					await responseStream.WriteAsync(
						new NewsResponse
						{
							Description = news.Description,
							Title = news.Title,
							Timestamp = Timestamp.FromDateTime(DateTime.UtcNow)
						});
				}
			}
		}

		private static async Task<NewsData> GetCurrentNewsAsync(HttpClient httpClient, string keyWords)
		{
			var currentTime = DateTime.UtcNow;
			var ts = new TimeSpan(0, 1, 0, 0, 0);
			var startDate = currentTime - ts;
			var startDateFormat = startDate.ToString("yyyy-MM-ddTHH:mm:ss");

			string longurl = "https://api.currentsapi.services/v1/search?apiKey=-eUpWubJIoT_9BPcc072BRq3e2zHcYOID5q_ZfpZXoNsTqQ0";
			var uriBuilder = new UriBuilder(longurl);
			var query = HttpUtility.ParseQueryString(uriBuilder.Query);
			query["start_date"] = startDateFormat;
			query["keywords"] = keyWords;
			uriBuilder.Query = query.ToString();
			longurl = uriBuilder.ToString();

			var response = await httpClient.GetStringAsync(
				//"");
				longurl);
			////await responseStream.WriteAsync(new TrackReply({Message = }));

			var temp = NewsData.FromJson(response);
			return temp;
		}

		private static async Task<NewsData> GetLatestNewsAsync(HttpClient httpClient, string keyWords)
		{
			var currentTime = DateTime.UtcNow;
			var ts = new TimeSpan(0, 0, 0, 5, 0);
			var startDate = currentTime - ts;
			var startDateFormat = startDate.ToString("yyyy-MM-ddTHH:mm:ss");

			string longurl = "https://api.currentsapi.services/v1/search?apiKey=-eUpWubJIoT_9BPcc072BRq3e2zHcYOID5q_ZfpZXoNsTqQ0";
			var uriBuilder = new UriBuilder(longurl);
			var query = HttpUtility.ParseQueryString(uriBuilder.Query);
			query["start_date"] = startDateFormat;
			query["keywords"] = keyWords;
			uriBuilder.Query = query.ToString();
			longurl = uriBuilder.ToString();

			var response = await httpClient.GetStringAsync(
				//"");
				longurl);
			////await responseStream.WriteAsync(new TrackReply({Message = }));

			var temp = NewsData.FromJson(response);
			return temp;
		}
	}
}
