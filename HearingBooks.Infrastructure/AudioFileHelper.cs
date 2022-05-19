using System.Diagnostics;

namespace HearingBooks.Infrastructure;

public static class AudioFileHelper
{
	public static async Task<int> TryGettingDuration(string fileName)
	{
		// var fileInfo = new FileInfo(blobName);
		// var lenghtInSeconds = fileInfo.Length / (16000 * 1 * 16 / 8);
        
		Process process = new Process
		{
			StartInfo = new ProcessStartInfo
			{
				FileName = "bash",
				RedirectStandardInput = true,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false
			}
		};
		process.Start();
		
		var command = $"ffmpeg -i {fileName} 2>&1 | grep Duration";
		await process.StandardInput.WriteLineAsync(command);
		
		var durationString = await process.StandardOutput
			.ReadLineAsync();

		var duration = durationString.Trim()
			.Split(' ')
			.ElementAt(1)
			.Replace(',', ' ')
			.Trim()
			.Split(':');

		var hours = Int32.Parse(duration.ElementAt(0));
		var minutes = Int32.Parse(duration.ElementAt(1));
		var seconds = (int) Double.Parse(duration.ElementAt(2)) + 1;

		var timespan = new TimeSpan(hours, minutes, seconds);

		process.Kill();
		
		return (int) timespan.TotalSeconds;
	}
}