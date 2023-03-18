using HearingBooks.Contracts.DialogueSynthesis;

namespace HearingBooks.SynthesisProcessor.Services;

public static class SpeechTagsHelper
{
    public static string OpeningTags(string language) =>
        $"<speak xmlns=\"http://www.w3.org/2001/10/synthesis\" xmlns:mstts=\"http://www.w3.org/2001/mstts\" xmlns:emo=\"http://www.w3.org/2009/10/emotionml\" version=\"1.0\" xml:lang=\"{language}\">";

    public static string ClosingTags => "</speak>";

    public static string LineOpeningTagsForSpeaker(DialogueSynthesisData data, int index)
    {
        var speaker = FirstSpeaker(index) ? data.FirstSpeakerVoice : data.SecondSpeakerVoice;
        return $"<voice name=\"{speaker}\"><prosody rate=\"0%\" pitch=\"0%\">";
    }

    private static bool FirstSpeaker(int index) => index % 2 == 0;

    public static string LineClosingTagsForSpeaker() => "</prosody></voice>";
}