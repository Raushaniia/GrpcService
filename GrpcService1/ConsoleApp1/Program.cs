using Grpc.Net.Client;
using GrpcService.Services;
using GrpcService.Server;

//var channel = GrpcChannel.ForAddress("http://localhost:5298");
//var client = new Greeter.GreeterClient(channel);

////var reply = client.SayHello(new HelloRequest { Name = "test" });

//var reply2 = client.SendTrack(new TrackRequest { Name = "test" });

//Console.WriteLine(reply2);

var channel = GrpcChannel.ForAddress("http://localhost:5298");
var client = new WeatherService.WeatherServiceClient(channel);

//var reply2 = client.GetCurrentWeather(new GetCurrentWeatherforCityRequest ());
using var reply2 = client.GetCurrentWeatherStream(new GetCurrentWeatherforCityRequest());

while (await reply2.ResponseStream.MoveNext(CancellationToken.None))
{
	Console.WriteLine("Temperature: " + reply2.ResponseStream.Current.Temperature);
	Console.WriteLine("Timestamp: " + reply2.ResponseStream.Current.Timestamp);
	// "Greeting: Hello World" is written multiple times
}

//await foreach (var response in reply2.ResponseStream)
//{
//	Console.WriteLine("Greeting: " + (double)response.Temperature);
//	// "Greeting: Hello World" is written multiple times
//}

//Console.WriteLine((double)reply2.Temperature);
//Console.WriteLine((double)reply2.FeelsLike);