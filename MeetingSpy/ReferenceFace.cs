using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingSpy
{
    class ReferenceFace
    {
        private Guid faceID;
        private string imageURL;
        private double confidence;

        public ReferenceFace()
        {

        }

        public ReferenceFace(string _imageURL)
        {
            imageURL = _imageURL;
        }

        public Guid FaceID
        {
            get
            {
                return faceID;
            }

            set
            {
                faceID = value;
            }
        }
        public string ImageURL
        {
            get
            {
                return imageURL;
            }

            set
            {
                imageURL = value;
            }
        }

        public double Confidence
        {
            get
            {
                return confidence;
            }

            set
            {
                confidence = value;
            }
        }
    }
}
