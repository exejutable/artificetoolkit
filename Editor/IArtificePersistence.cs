namespace Editor
{
    public interface IArtificePersistence
    {
        public string ViewPersistenceKey { get; set; }

        public void SavePersistedData();
        public void LoadPersistedData();
    }
}
