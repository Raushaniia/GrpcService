using Grpc.Net.Client;
using GrpcService.Services;
using GrpcService.Server;
using Grpc.Core;

var channel = GrpcChannel.ForAddress("http://localhost:5298");
var client = new PostNewsService.PostNewsServiceClient(channel);

using var replies = client.GetNewsStream(new GetNewsForRequest());

var tokenSource = new CancellationTokenSource();
int n = 0;

try
{
	await foreach (var reply in replies.ResponseStream.ReadAllAsync(tokenSource.Token))
	{
		Console.WriteLine("Title: " + reply.Title);
		Console.WriteLine("Description: " + reply.Description);

		if (++n == 2)
		{
			tokenSource.Cancel();
		}
	}
}
catch (RpcException e) when (e.Status.StatusCode == StatusCode.Cancelled)
{
	Console.WriteLine("Streaming was cancelled from the client!");
}

Console.ReadLine();
