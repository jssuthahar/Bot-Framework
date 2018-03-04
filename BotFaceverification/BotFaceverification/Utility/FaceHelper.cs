using Microsoft.ProjectOxford.Face;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace BotFaceverification.Utility
{
    static class FaceHelper
    {
        private static readonly IFaceServiceClient faceServiceClient = new FaceServiceClient("59f7249307ae407f8009d22b35ba2bb0");

        public static async Task<string> UploadAndDetectFaces(string imageFilePath)
        {
            try
            {
                var requiredFaceAttributes = new FaceAttributeType[] {
                    FaceAttributeType.Age,
                    FaceAttributeType.Gender,
                    FaceAttributeType.Emotion
                };
                using (WebClient webClient = new WebClient())
                {
                    using (Stream imageFileStream = webClient.OpenRead(imageFilePath))
                    {
                        var faces = await faceServiceClient.DetectAsync(imageFileStream, returnFaceLandmarks: true, returnFaceAttributes: requiredFaceAttributes);
                        var faceAttributes = faces.Select(face => face.FaceAttributes);
                        string result = string.Empty;
                        faceAttributes.ToList().ForEach(f =>
                            result += $"Age: {f.Age.ToString()} Years  Gender: {f.Gender}  Emotion: {f.Emotion.ToString()}{Environment.NewLine}{Environment.NewLine}"
                        );
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

    }
}