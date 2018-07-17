namespace Scripts
{
    [System.Serializable]
    public class ResultsData
    {
        public User[] Users;
    }

    [System.Serializable]
    public class User
    {
        public string Name;             // "Firstname Lastname"
        public TestTimes TestTimes;
    }

    [System.Serializable]
    public class TestTimes
    {
        public float NeutralTime;       // in seconds, 2 decimal points
        public float CongruentTime;     // in seconds, 2 decimal points
        public float IncongruentTime;   // in seconds, 2 decimal points
    }
}
