namespace ArtificeToolkit.AEditor
{
    public interface IArtifice_Persistence
    {
        public string ViewPersistenceKey { get; set; }

        public void SavePersistedData();
        public void LoadPersistedData();
    }
}
