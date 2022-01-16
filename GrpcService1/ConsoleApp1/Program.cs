using Grpc.Net.Client;
using GrpcService.Services;
using GrpcService.Server;
using Grpc.Core;

Console.WriteLine("Please enter the keywords below:");
var keywords = Console.ReadLine();

var channel = GrpcChannel.ForAddress("http://localhost:5298");
var client = new PostNewsService.PostNewsServiceClient(channel);

var request = new GetNewsForRequest();
request.KeyWords = keywords;

using var replies = client.GetNewsStream(request);

var tokenSource = new CancellationTokenSource();

try
{
	await foreach (var reply in replies.ResponseStream.ReadAllAsync(tokenSource.Token))
	{
		Console.WriteLine("Title: " + reply.Title);
		Console.WriteLine("Description: " + reply.Description);
		Console.WriteLine("TimeStamp: " + reply.Timestamp);
		Console.WriteLine();


		if (Console.KeyAvailable)
		{
			var key = Console.ReadKey(true).Key;
			if (key == ConsoleKey.Enter)
			{
				tokenSource.Cancel();
			}
		}
	}
}
catch (RpcException e) when (e.Status.StatusCode == StatusCode.Cancelled)
{
	Console.WriteLine("Streaming was cancelled from the client!");
}

Console.ReadLine();
