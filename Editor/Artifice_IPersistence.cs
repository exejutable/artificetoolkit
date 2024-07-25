namespace ArtificeToolkit.Editor
{
    public interface Artifice_IPersistence
    {
        public string ViewPersistenceKey { get; set; }

        public void SavePersistedData();
        public void LoadPersistedData();
    }
}
