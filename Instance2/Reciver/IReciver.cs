namespace Instance2.Reciver
{
    public interface IReciver
    {
        Task ReciveKeyValue(string key, string value);
    }
}
