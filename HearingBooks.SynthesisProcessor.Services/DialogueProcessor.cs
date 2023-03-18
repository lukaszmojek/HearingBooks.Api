using HearingBooks.Contracts.DialogueSynthesis;

namespace HearingBooks.SynthesisProcessor.Services;

public class DialogueProcessor
{
    public static IEnumerable<(string, int)> SplitDialogueIntoLines(string dialogueText, string lineSeparator) =>
        dialogueText
            .Split(lineSeparator)
            .Select((line, index) => (line.Trim(), index));

    public static string BuildDialogueText(DialogueSynthesisData data, string lineSeparator)
    {
        var linesWithTags = SplitDialogueIntoLines(data.DialogueText, lineSeparator)
            .Select(line => $"{SpeechTagsHelper.LineOpeningTagsForSpeaker(data, line.Item2)}{line.Item1}{SpeechTagsHelper.LineClosingTagsForSpeaker()}")
            .Aggregate((current, next) => $"{current}{next}");

        return $"{SpeechTagsHelper.OpeningTags(data.Language)}{linesWithTags}{SpeechTagsHelper.ClosingTags}";
    }
}