namespace ImageTaggingApp.Console.App.Domain.Interfaces {
    public interface ITag {
        string Name { get; set; }
        double Probability { get; set; }
    }
}
