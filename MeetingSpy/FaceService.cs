using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.ProjectOxford.Face;
using System.Collections.Specialized;

namespace MeetingSpy
{
    class FaceService
    {
        public List<Guid> faceIds;

        public async void DetectFaces(string subscriptionID, List<ReferenceFace> referenceFaces)
        {
            faceIds = new List<Guid>();
            FaceServiceClient faceServiceClient = new FaceServiceClient(subscriptionID);

            Microsoft.ProjectOxford.Face.Contract.Face[] faces;
            int faceCount = 0;

            try
            {
                foreach (ReferenceFace refface in referenceFaces)
                {
                    faces = await faceServiceClient.DetectAsync(refface.ImageURL);

                    foreach (var face in faces)
                    {
                        refface.FaceID = face.FaceId;
                        faceIds.Add(face.FaceId);
                        faceCount++;
                    }
                }

                Guid[] faceIDGuids = new Guid[faceCount];
                int count = 0;

                foreach (var faceid in faceIds)
                {
                    faceIDGuids[count] = faceid;
                    count++;
                }

                var results = await faceServiceClient.FindSimilarAsync(faceIds[0], faceIDGuids);

                foreach (ReferenceFace refface in referenceFaces)
                {
                    foreach (var result in results)                
                     {
                        if (result.FaceId == refface.FaceID)
                        {
                            refface.Confidence = result.Confidence;
                            break;
                        }
                        else
                        {
                            refface.Confidence = 0.00;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
