using System;
using System.Collections.Generic;
using CommandLine;
using ImageTaggingApp.Console.App.APIs;
using ImageTaggingApp.Console.App.Entities;

namespace ImageTaggingApp.Console.App.Commands {
    [Verb(CommandNames.Tag, HelpText = "Tags images using specified arguments.")]
    public class TagCommandOptions {
        private string _configFilePath;

        [Option(shortName: 'p', longName: "path", HelpText = "Folder of file path to images.", Required = true)]
        public string ImagesPath { get; set; }

        [Option(shortName: 'a', longName: "api",
            HelpText = "API to use to tag image. Available: Clarifai, Microsoft, Google.",
            Default = TaggingAPIType.Microsoft )]
        public TaggingAPIType API { get; set; }

        [Option(shortName: 'd', longName: "destination",
            HelpText = "Where to write tag info. Possible: exif or database.", Default = TagDestinationType.ImageMetadata)]
        public TagDestinationType Destination { get; set; }

        [Option(shortName: 't', longName: "min_tag_confidence",
            HelpText = "Minimum confidence for label to be saved in tag info", Default = 80)]
        public double MinTagConfidence { get; set; }

        [Option(shortName: 'b', longName: "min_blur_confidence",
            HelpText = "Minimum confidence for image to be treated like blur.", Default = 80)]
        public double MinBlurConfidence { get; set; }

        [Option(shortName: 'f', longName: "config_file",
            HelpText =
                "Config file path where API credentials and any other required API parameters are stored. Default is application folder, file name config.json" 
            )]
        public string ConfigFilePath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_configFilePath)) {
                    _configFilePath = AppDomain.CurrentDomain.BaseDirectory + "config.json";
                }
                return _configFilePath;
            }
            set { _configFilePath = value; }
        }
    }
}
