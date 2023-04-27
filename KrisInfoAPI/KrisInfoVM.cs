namespace KrisInfoAPI
{
    public class KrisInfoVM
    {
        public int Identifier { get; set; }
        public string PushMessage { get; set; }
        public DateTime Published { get; set; }
        public List<Area> Area { get; set; }
    }
}
