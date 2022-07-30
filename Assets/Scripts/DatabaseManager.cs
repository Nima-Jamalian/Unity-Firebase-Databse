using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using TMPro;
using System;
public class DatabaseManager : MonoBehaviour
{
    string userID;
    private DatabaseReference databaseReference;

    [SerializeField] TMP_InputField name;
    [SerializeField] TMP_InputField score;

    public TMP_Text nameText;
    public TMP_Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        //Assigning a unique user id based on device identifier
        userID = SystemInfo.deviceUniqueIdentifier;
        //Get the root refrence location of the database.
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }


    public void WriteNewUser()
    {
        User user = new User(name.text, score.text);
        string json = JsonUtility.ToJson(user);
        databaseReference.Child("users").Child(userID).SetRawJsonValueAsync(json);
    }

    IEnumerator GetUserName(Action<string> onCallBack)
    {
        var userData = databaseReference.Child("users").Child(userID).Child("name").GetValueAsync();
        yield return new WaitUntil(predicate: () => userData.IsCompleted);
        if(userData != null)
        {
            DataSnapshot snapshot = userData.Result;
            onCallBack.Invoke(snapshot.Value.ToString());
        }
    }

    IEnumerator GetUserScore(Action<int> onCallBack)
    {
        var userData = databaseReference.Child("users").Child(userID).Child("score").GetValueAsync();
        yield return new WaitUntil(predicate: () => userData.IsCompleted);
        if (userData != null)
        {
            DataSnapshot snapshot = userData.Result;
            onCallBack.Invoke(int.Parse(snapshot.Value.ToString()));
        }
    }

    public void GetUserData()
    {
        StartCoroutine(GetUserName((string name) =>
        {
            nameText.text = "Name: " + name;
        }));

        StartCoroutine(GetUserScore((int score) =>
        {
            scoreText.text = "Name: " + score.ToString();
        }));
    }

    public void UpdateUserName()
    {
        databaseReference.Child("users").Child(userID).Child("name").SetValueAsync(name.text);
    }

    public void UpdateUserScore()
    {
        databaseReference.Child("users").Child(userID).Child("score").SetValueAsync(score.text);
    }
}
