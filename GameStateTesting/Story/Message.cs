using System;

namespace GameStateTesting.Story
{
    public class Message
    {
        public string Id { get; set; }

        public string Story { get; set; }
        public string GoodDecision { get; set; }

        public string BadDecision { get; set; }

        public static implicit operator Message(string v)
        {
            throw new NotImplementedException();
        }
    }
}