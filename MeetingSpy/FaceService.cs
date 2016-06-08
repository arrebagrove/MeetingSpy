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


        /// <summary>
        /// Find similar face in a reference set
        /// </summary>
        /// <param name="subscriptionID">Cognitive Services Subscription ID</param>
        /// <param name="referenceFaces">List of faces to compare against</param>
        /// <param name="lookupFaceUrl">URL of lookup face to find</param>
        public async void DetectFaces(string subscriptionID, List<ReferenceFace> referenceFaces, string lookupFaceUrl)
        {
            faceIds = new List<Guid>();
            FaceServiceClient faceServiceClient = new FaceServiceClient(subscriptionID);

            Microsoft.ProjectOxford.Face.Contract.Face[] faces;
            int faceCount = 0;

            try
            {
                foreach (ReferenceFace refface in referenceFaces)
                {
                    // Find faces and get face ids in reference images
                    faces = await faceServiceClient.DetectAsync(refface.ImageURL);

                    foreach (var face in faces)
                    {
                        refface.FaceID = face.FaceId;
                        faceIds.Add(face.FaceId);
                        faceCount++;
                    }
                }

                // Find faces and get face IDs in lookup face
                Microsoft.ProjectOxford.Face.Contract.Face[] lookupFace;
                lookupFace = await faceServiceClient.DetectAsync(lookupFaceUrl);

                // Get Guid's for all faces that were found in reference set
                Guid[] faceIDGuids = new Guid[faceCount];
                int count = 0;

                foreach (var faceid in faceIds)
                {
                    faceIDGuids[count] = faceid;
                    count++;
                }

                // Get the confidence ranking for all faces
                var results = await faceServiceClient.FindSimilarAsync(lookupFace[0].FaceId, faceIDGuids);

                // Add the confidence ranking back into the reference set
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
