
namespace DevsDNA.Application.Controls
{
    using System;
    using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Xamarin.Forms;

    public class LabelTypewriter : Label
	{
		//160 unicode is Non-breaking space. On iOS 160 unicode doesn't work for this purpose
		private readonly char invisibleChar = Device.RuntimePlatform == Device.iOS ? ' ': Convert.ToChar(160);

		public LabelTypewriter()
		{
			Opacity = 0;
		}

		public Task ShowAsync(int miliseconds = 75, bool includedInitialBlink = true)
		{
			return ShowAsync(CancellationToken.None, miliseconds, includedInitialBlink);
		}

		public async Task ShowAsync(CancellationToken ct, int miliseconds = 75, bool includedInitialBlink = true)
		{
			if (string.IsNullOrEmpty(Text) || Text.Length < 2)
				return;

			ct.ThrowIfCancellationRequested();

			string originalText = Text;
			HeightRequest = Height;
			string invisibleText = string.Concat(originalText.Select(c => c != ' ' ? invisibleChar : c));

			Text = invisibleText;
			Opacity = 1;

			if (includedInitialBlink)
			{
				for (int i = 0; i < 3; i++)
				{
					ct.ThrowIfCancellationRequested();
					Text = $"{originalText[0]}{invisibleText.Substring(1, invisibleText.Length - 2)}";
					await Task.Delay(350);
					Text = invisibleText;
					await Task.Delay(350);
				}
			}
			string t;
			for (int i = 0; i < originalText.Length + 1; i++)
			{
				ct.ThrowIfCancellationRequested();
				t = new string(originalText.Take(i).ToArray());
				Text = $"{t}{invisibleText.Substring(i)}";
				await Task.Delay(miliseconds);
			}
		}
	}
}
