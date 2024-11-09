namespace EtcsServer.DriverAppDto
{
    public class MovementAuthority
    {
        public string MessageType { get; set; } = "MA";
        public double[] Speeds { get; set; }
        public double[] SpeedDistances { get; set; }
        public double[] Gradients { get; set; }
        public double[] GradientDistances { get; set; }
        public string[] Messages { get; set; }
        public double[] MessageDistances { get; set; }
        public int[] Lines { get; set; }
        public double[] LinesDistances { get; set; }
        public double ServerPosition { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is MovementAuthority other)
            {
                return MessageType == other.MessageType
                    && ServerPosition == other.ServerPosition
                    && (Speeds?.SequenceEqual(other.Speeds) ?? other.Speeds == null)
                    && (SpeedDistances?.SequenceEqual(other.SpeedDistances) ?? other.SpeedDistances == null)
                    && (Gradients?.SequenceEqual(other.Gradients) ?? other.Gradients == null)
                    && (GradientDistances?.SequenceEqual(other.GradientDistances) ?? other.GradientDistances == null)
                    && (Messages?.SequenceEqual(other.Messages) ?? other.Messages == null)
                    && (MessageDistances?.SequenceEqual(other.MessageDistances) ?? other.MessageDistances == null)
                    && (Lines?.SequenceEqual(other.Lines) ?? other.Lines == null)
                    && (LinesDistances?.SequenceEqual(other.LinesDistances) ?? other.LinesDistances == null);
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(MessageType);
            hashCode.Add(ServerPosition);
            if (Speeds != null) foreach (var speed in Speeds) hashCode.Add(speed);
            if (SpeedDistances != null) foreach (var dist in SpeedDistances) hashCode.Add(dist);
            if (Gradients != null) foreach (var gradient in Gradients) hashCode.Add(gradient);
            if (GradientDistances != null) foreach (var dist in GradientDistances) hashCode.Add(dist);
            if (Messages != null) foreach (var message in Messages) hashCode.Add(message);
            if (MessageDistances != null) foreach (var dist in MessageDistances) hashCode.Add(dist);
            if (Lines != null) foreach (var line in Lines) hashCode.Add(line);
            if (LinesDistances != null) foreach (var dist in LinesDistances) hashCode.Add(dist);

            return hashCode.ToHashCode();
        }
    }
}
