using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;

namespace MeetingSpy
{
    internal class FaceService
    {
        public List<Guid> FaceIds;

        /// <summary>
        /// Find similar face in a reference set
        /// </summary>
        /// <param name="subscriptionId">Cognitive Services Subscription ID</param>
        /// <param name="referenceFaces">List of faces to compare against</param>
        /// <param name="lookupFaceUrl">URL of lookup face to find</param>
        /// <returns>New collection of comparison faces with confidence ratings</returns>
        public async Task<List<ReferenceFace>> DetectFacesAsync(string subscriptionId, List<ReferenceFace> referenceFaces, string lookupFaceUrl)
        {
            var resultFaces = new List<ReferenceFace>();

            FaceIds = new List<Guid>();
            var faceServiceClient = new FaceServiceClient(subscriptionId);

            var faceCount = 0;

            try
            {
                foreach (var refface in referenceFaces)
                {
                    // Find faces and get face ids in reference images
                    var faces = await faceServiceClient.DetectAsync(refface.ImageUrl);

                    foreach (var face in faces)
                    {
                        refface.FaceId = face.FaceId;
                        FaceIds.Add(face.FaceId);
                        faceCount++;
                    }
                }

                // Find faces and get face IDs in lookup face
                var lookupFace = await faceServiceClient.DetectAsync(lookupFaceUrl);

                // Get Guid's for all faces that were found in reference set
                var faceIdGuids = new Guid[faceCount];
                var count = 0;

                foreach (var faceid in FaceIds)
                {
                    faceIdGuids[count] = faceid;
                    count++;
                }

                // Get the confidence ranking for all faces
                var results = await faceServiceClient.FindSimilarAsync(lookupFace[0].FaceId, faceIdGuids);
                if (results == null) throw new ArgumentNullException(nameof(results));

                // Add the confidence ranking back into the reference set

                foreach (var refface in referenceFaces)
                {
                    var returnFace = new ReferenceFace { FaceId = refface.FaceId, ImageUrl = refface.ImageUrl, Confidence = refface.Confidence };

                    foreach (var result in results)
                    {
                        if (result.FaceId == refface.FaceId)
                        {
                            returnFace.Confidence = result.Confidence;
                            break;
                        }
                        else
                        {
                            returnFace.Confidence = 0.00;
                        }
                    }
                    resultFaces.Add(returnFace);
                }
            }
            catch
            {
                //todo: fancy exception handling
            }

            return resultFaces;
        }
    }
}