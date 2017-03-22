using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Media.Imaging;
using ExifLib;
using ImageTaggingApp.Console.App.Entities;

namespace ImageTaggingApp.Console.App.Services {
    // A few methods that tries to manipulate Image metadata in different ways
    // Reading metadata is straightforward and good libs exist.
    // Writing is more complex and could not find easy and quick solution. Spent researching a lot time.
    // Need to investigate this aproach more and as it seems now will need to create custom solution  for Writing to Image Metadata.
    public class ImageTagsSavingToImageMetadataService : IImageTagsSavingToExternalResourcesService {
        public void Save(BlockingCollection<Image> imagesToSaveToExternalResources, CancellationTokenSource cts, IProgress<double> progressBar) {
            using (ExifReader reader = new ExifReader(@"C:\Users\Simonas\Desktop\ft\IMG_20151103_104951.jpg")) {
                string result;
                reader.GetTagValue(ExifTags.Software, out result);
                int result2;
                reader.GetTagValue(ExifTags.ImageWidth, out result2);
                //ReadIPTCTags();
                WriteMetadata();
            }
        }

        private void ReadIPTCTags() {
            var stream = new FileStream(@"C:\Users\Simonas\Desktop\ft\IMG_20151103_104951.jpg", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            var decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.None);
            var metadata = decoder.Frames[0].Metadata as BitmapMetadata;
            metadata.Keywords = new ReadOnlyCollection<string>(new List<string> {"a", "b", "c"});
            if (metadata?.Keywords != null) {
                System.Console.WriteLine(metadata.Keywords.Aggregate((old, val) => old + "; " + val));
            }
        }

        private void WriteMetadata() {
            Stream pngStream = new System.IO.FileStream(@"C:\Users\Simonas\Desktop\ft\IMG_20151103_104951.jpg", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            JpegBitmapDecoder pngDecoder = new JpegBitmapDecoder(pngStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapFrame pngFrame = pngDecoder.Frames[0];
            InPlaceBitmapMetadataWriter pngInplace = pngFrame.CreateInPlaceBitmapMetadataWriter();
            if (pngInplace.TrySave() == true) {
                pngInplace.SetQuery("/Text/Description", "Have a nice day.");
            }
            pngStream.Close();
        }
    }
}
