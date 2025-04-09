namespace LootchasersAPI.Services
{
    public interface Event
    {
        public void CreateNew();
        public void End();
        public JsonContent? ViewStandings();
        public void PublishResults();

    }

    public class EventHandler
    {
        public class PowerHour : Event
        {
            public record EventRecord(int dinkHash, string username, int value);

            public void CreateNew()
            {
                
            }

            public void End()
            {

            }

            public JsonContent? ViewStandings()
            {
                return null;
            }

            public void PublishResults()
            {

            }
        }
    }
}
