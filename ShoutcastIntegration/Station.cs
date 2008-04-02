namespace ShoutcastIntegration
{
    public class Station
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Bitrate { get; set; }
        public string Genre { get; set; }
        public string CurrentTrack { get; set; }
        public int TotalListeners { get; set; }
        public int ID { get; set; }
    }
}