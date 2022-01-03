using System.Media;

namespace MusicStreaming.Player
{
	public class Player
	{
		public Player()
		{
			SoundPlayer sp = new SoundPlayer();
			sp.SoundLocation = @"C:\Users\Akvelon\Downloads\Sample-wav-file.wav";
			sp.Load();
			sp.PlayLooping();
		}
	}
}