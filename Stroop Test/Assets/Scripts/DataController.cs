using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using UnityEngine;

public class DataController : MonoBehaviour
{
    [SerializeField] private string _dataFileName = "data.json";
    [SerializeField] private string _cred;

    internal static DataController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    internal void SerializeNewUserData(string name, float[] clearTimes)
    {
        ResultsData dataObject = LoadData(_dataFileName) ?? new ResultsData();

        if (dataObject.Users == null)
        {
            dataObject.Users = new List<User>();
        }

        User userSearch = null;

        // possibly redundant
        if (dataObject.Users != null)
        {
            // User is a class (reference type). This will directly modify the user data.
            userSearch = dataObject.Users.FirstOrDefault(x => x.Name == name);
        }

        // User already exists
        if (userSearch != null)
        {
            Debug.Log("User search was not null.");
            userSearch.SecondTestTimes = new TestTimes(clearTimes);
        }
        else
        {
            Debug.Log("User search was null");
            dataObject.Users.Add(new User(name, clearTimes));
            Debug.Log("Added new user with name " + name);
        }

        string filePath = Path.Combine(Application.persistentDataPath, _dataFileName);

        string jsonData = JsonUtility.ToJson(dataObject, true);

        File.WriteAllText(filePath, jsonData);
        print("Saved new data in " + _dataFileName);
    }

    private string LoadDataRawText()
    {
        string filePath = Path.Combine(Application.persistentDataPath, _dataFileName);

        if (File.Exists(filePath)) return File.ReadAllText(filePath);

        Debug.LogError("Could not find " + filePath);
        return null;
    }

    private ResultsData LoadData(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(filePath))
        {
            Debug.LogWarning("Could not find " + filePath);
            return null;
        }

        string jsonData = File.ReadAllText(filePath);
        return JsonUtility.FromJson<ResultsData>(jsonData);
    }

    // Mails the raw text from the serialized data to the email address
    internal void EmailTo(string emailAddress)
    {
        MailMessage mail = new MailMessage("strooptestunity@gmail.com", emailAddress)
        {
            Subject = "Stroop data",
            Body = LoadDataRawText()
        };

        SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("strooptestunity@gmail.com", _cred)
        };

        ServicePointManager.ServerCertificateValidationCallback =
            (s, certificate, chain, sslPolicyErrors) => true;   

        smtp.Send(mail);
        Debug.Log("Email sent!");
    }
}
