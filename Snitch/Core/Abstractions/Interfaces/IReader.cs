namespace Snitch.Api.User.Interfaces
{
    public interface IReader<T> where T:IReadFromFileAble
    {
        public T ReadFromFile(string filename);
    }
}