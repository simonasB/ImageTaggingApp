using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ImageTaggingApp.Console.App.Entities;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Tag = ImageTaggingApp.Console.App.Entities.Tag;

namespace ImageTaggingApp.Console.App.APIs {
    public class MicrosoftVisionApi : IImageTaggingApi {
        private readonly IVisionServiceClient _visionServiceClient;

        public MicrosoftVisionApi(IVisionServiceClient visionServiceClient) {
            _visionServiceClient = visionServiceClient;
        }

        public async Task<ImageMetadata> Tag(string imagePath) {
            var result = await UploadAndGetTagsForImage(imagePath);
            LogAnalysisResult(result);
            return new ImageMetadata { Tags = new List<Tag>() };
        }

        private async Task<AnalysisResult> UploadAndGetTagsForImage(string imageFilePath) {
            using (Stream imageFileStream = File.OpenRead(imageFilePath)) {
                AnalysisResult analysisResult = await _visionServiceClient.GetTagsAsync(imageFileStream);
                return analysisResult;
            }
        }

        private async Task<AnalysisResult> UploadAndGetTagsForUrl(string imageUrl) {
            return await _visionServiceClient.AnalyzeImageAsync(imageUrl, null, null);
        }

        private void LogAnalysisResult(AnalysisResult result) {
            if (result == null) {
                System.Console.WriteLine("null");                
                return;
            }

            if (result.Metadata != null) {
                System.Console.WriteLine("Image Format : " + result.Metadata.Format);
                System.Console.WriteLine("Image Dimensions : " + result.Metadata.Width + " x " + result.Metadata.Height);
            }

            if (result.ImageType != null) {
                string clipArtType;
                switch (result.ImageType.ClipArtType) {
                    case 0:
                        clipArtType = "0 Non-clipart";
                        break;
                    case 1:
                        clipArtType = "1 ambiguous";
                        break;
                    case 2:
                        clipArtType = "2 normal-clipart";
                        break;
                    case 3:
                        clipArtType = "3 good-clipart";
                        break;
                    default:
                        clipArtType = "Unknown";
                        break;
                }
                System.Console.WriteLine("Clip Art Type : " + clipArtType);

                string lineDrawingType;
                switch (result.ImageType.LineDrawingType) {
                    case 0:
                        lineDrawingType = "0 Non-LineDrawing";
                        break;
                    case 1:
                        lineDrawingType = "1 LineDrawing";
                        break;
                    default:
                        lineDrawingType = "Unknown";
                        break;
                }
                System.Console.WriteLine("Line Drawing Type : " + lineDrawingType);
            }


            if (result.Adult != null) {
                System.Console.WriteLine("Is Adult Content : " + result.Adult.IsAdultContent);
                System.Console.WriteLine("Adult Score : " + result.Adult.AdultScore);
                System.Console.WriteLine("Is Racy Content : " + result.Adult.IsRacyContent);
                System.Console.WriteLine("Racy Score : " + result.Adult.RacyScore);
            }

            if (result.Categories != null && result.Categories.Length > 0) {
                System.Console.WriteLine("Categories : ");
                foreach (var category in result.Categories) {
                    System.Console.WriteLine("   Name : " + category.Name + "; Score : " + category.Score);
                }
            }

            if (result.Faces != null && result.Faces.Length > 0) {
                System.Console.WriteLine("Faces : ");
                foreach (var face in result.Faces) {
                    System.Console.WriteLine("   Age : " + face.Age + "; Gender : " + face.Gender);
                }
            }

            if (result.Color != null) {
                System.Console.WriteLine("AccentColor : " + result.Color.AccentColor);
                System.Console.WriteLine("Dominant Color Background : " + result.Color.DominantColorBackground);
                System.Console.WriteLine("Dominant Color Foreground : " + result.Color.DominantColorForeground);

                if (result.Color.DominantColors != null && result.Color.DominantColors.Length > 0) {
                    string colors = "Dominant Colors : ";
                    foreach (var color in result.Color.DominantColors) {
                        colors += color + " ";
                    }
                    System.Console.WriteLine(colors);
                }
            }

            if (result.Description != null) {
                System.Console.WriteLine("Description : ");
                foreach (var caption in result.Description.Captions) {
                    System.Console.WriteLine("   Caption : " + caption.Text + "; Confidence : " + caption.Confidence);
                }
                string tags = "   Tags : ";
                foreach (var tag in result.Description.Tags) {
                    tags += tag + ", ";
                }
                System.Console.WriteLine(tags);

            }

            if (result.Tags != null) {
                System.Console.WriteLine("Tags : ");
                foreach (var tag in result.Tags) {
                    System.Console.WriteLine("   Name : " + tag.Name + "; Confidence : " + tag.Confidence + "; Hint : " + tag.Hint);
                }
            }
        }
    }
}
