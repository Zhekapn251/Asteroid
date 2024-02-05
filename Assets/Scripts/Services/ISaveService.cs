namespace Services
{
    public interface ISaveService
    {
        void Save<T>(string key, T data);
        T Load<T>(string key, T defaultValue);
        void Delete(string key);
        void ClearAllData();
    }
}
