using ImageTaggingApp.Console.App.Domain.Implementation;
using ImageTaggingApp.Console.App.Infrastructure.Implementation;

namespace ImageTaggingApp.Console.App.Application.Implementation {
    public class ApplicationServices  {
        private readonly DomainServices _domainServices;

        public ApplicationServices(DomainServices domainServices) {
            _domainServices = domainServices;
        }

        public void Tag(Image image) {
            _domainServices.Tag(image);
        }
    }
}
