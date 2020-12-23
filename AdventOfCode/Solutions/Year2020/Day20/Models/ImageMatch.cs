namespace AdventOfCode.Solutions.Year2020.Day20.Models
{
    public class ImageMatch
    {
        public ImageMatch(int imageId, bool isFlipped, int rotation, int matchedAtRotation)
        {
            ImageId = imageId;
            IsFlipped = isFlipped;
            Rotation = rotation;
            MatchedAtRotation = matchedAtRotation;
        }

        public int ImageId { get; set; }
        public bool IsFlipped { get; set; }
        public int Rotation { get; set; }
        public int MatchedAtRotation { get; set; }
    }
}
