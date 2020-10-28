namespace MotionWrapper
{
    public interface IReadAndSave
    {
        bool Read(string xml);
        bool Save(string xml);
    }
}
