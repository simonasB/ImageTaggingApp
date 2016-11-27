using System;
using ImageTaggingApp.Console.App.Application.Interfaces;
using ImageTaggingApp.Console.App.Domain.Interfaces;
using ImageTaggingApp.Console.App.Presentation.Interfaces;

namespace ImageTaggingApp.Console.App.Application.Implementation {
    public class ApplicationServices : IApplicationServices {
        private readonly IDomainServices _domainServices;

        public ApplicationServices(IDomainServices domainServices) {
            _domainServices = domainServices;
        }

        public void Tag(IImage image) {
            _domainServices.Tag(image);
        }
    }
}
