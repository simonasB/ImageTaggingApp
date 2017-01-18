namespace ImageTaggingApp.Console.App.Entities {
    public class Image {
        public string Id { get; set; }
        public string Path { get; set; }
        public ImageMetadata ImageMetadata { get; set; }   
    }
}
