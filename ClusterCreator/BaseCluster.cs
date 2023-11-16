namespace ClusterCreator
{
    public class BaseCluster
    {
        #region Constructors
        public BaseCluster(int id, string registration, double latitude, double longitude)
        {
            Id = id;
            LatLonCenter = new LatLong
            {
                Latitude = latitude,
                Longitude = longitude
            };
            Registrations = new List<string>();
            Registrations.Add(registration);
            LatLonList = new List<LatLong>();
            LatLonList.Add(LatLonCenter);
        }

        public BaseCluster(BaseCluster old)
        {
            Id = old.Id;
            LatLonCenter = new LatLong
            {
                Latitude = old.LatLonCenter.Latitude,
                Longitude = old.LatLonCenter.Longitude
            };
            LatLonList = new List<LatLong>(old.LatLonList);
            Registrations = new List<string>(old.Registrations);
        }
        #endregion
        public int Id { get; set; }
        public List<string> Registrations { get; set; }
        public LatLong LatLonCenter { get; set; }
        public List<LatLong> LatLonList { get; set; }
        public DateTime RecordedTimeUTC { get; set; }
    }
}