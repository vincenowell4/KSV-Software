using Google.Cloud.TextToSpeech.V1;

namespace VotingApp.Models
{
    public partial class CreatedVote
    {
        public static SynthesizeSpeechResponse VoteAudio;
        public void SetVoteAudio()
        {
            VoteAudio = SynthesizeSpeechResponse.Parser.ParseFrom(VoteAudioBytes);
        }
    }
}
