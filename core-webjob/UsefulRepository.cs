namespace Samples.WebJobs.Core
{
    public interface IUsefulRepository
    {
        string GetFoo();
    }

    public class UsefulRepository : IUsefulRepository
    {
        public string GetFoo()
        {
            return "Foo 2.0.0";
        }
    }
}