using System;

namespace MeetingSpy
{
    internal class ReferenceFace
    {
        public ReferenceFace()
        {

        }

        public ReferenceFace(string imageUrl)
        {
            ImageUrl = imageUrl;
        }

        public Guid FaceId { get; set; }

        public string ImageUrl { get; set; }

        public double Confidence { get; set; }
    }
}