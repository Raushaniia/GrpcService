using System.Text.Json;
using Grpc.Core;
using GrpcService;
using GrpcService1;
using MusicStreaming.Core;

namespace GrpcService.Services
{
	public class GreeterService : Greeter.GreeterBase
	{
		private readonly ILogger<GreeterService> _logger;
		
		public GreeterService(ILogger<GreeterService> logger)
		{
			_logger = logger;
		}

		public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
		{
			return Task.FromResult(new HelloReply
			{
				Message = "Hello " + request.Name
			});
		}

		public override async Task SendTrack(TrackRequest request, IServerStreamWriter<TrackReply> responseStream, ServerCallContext context)
		{
			
			
			//return Task.FromResult(new TrackReply
			//{
			//	Message = "Hello " + request.Name
			//});
		}
	}
}