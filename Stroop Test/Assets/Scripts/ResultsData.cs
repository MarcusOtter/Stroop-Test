using System.Collections.Generic;

[System.Serializable]
public class ResultsData
{
    public List<User> Users;
}

[System.Serializable]
public class User
{
    public string Name;             // "Firstname Lastname"
    public TestTimes FirstTestTimes;
    public TestTimes SecondTestTimes;

    public User(string name, float[] firstTestTimes)
    {
        Name = name;
        FirstTestTimes = new TestTimes(firstTestTimes);
    }
}

[System.Serializable]
public class TestTimes
{
    public string NeutralTime;       // in seconds, 2 decimal points
    public string CongruentTime;     // in seconds, 2 decimal points
    public string IncongruentTime;   // in seconds, 2 decimal points

    public TestTimes(float[] testTimes)
    {
        NeutralTime = testTimes[0].ToString("0.00");
        CongruentTime = testTimes[1].ToString("0.00");
        IncongruentTime = testTimes[2].ToString("0.00");
    }
}
