using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class Main : MonoBehaviour {

	private object aToken;

	private int score = 0;

	private string name;

	private string facebook_id;

	private string email;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Awake ()
	{
		if (!FB.IsInitialized) {
			// Initialize the Facebook SDK
			FB.Init(InitCallback, OnHideUnity);
		} else {
			// Already initialized, signal an app activation App Event
			FB.ActivateApp();
		}
	}

	private void InitCallback ()
	{
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp();
			// Continue with Facebook SDK
			// ...
		} else {
			Debug.Log("Failed to Initialize the Facebook SDK");
		}
	}

	private void OnHideUnity (bool isGameShown)
	{
		if (!isGameShown) {
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		} else {
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
		}
	}

	private void loginFacebook(){
		FB.LogInWithReadPermissions(new List<string>(){"public_profile", "email", "user_friends"}, AuthCallback);
		FB.LogInWithPublishPermissions(new List<string>(){"publish_actions"}, AuthCallback);
	}

	private void AuthCallback (ILoginResult result) {
		if (FB.IsLoggedIn) {
			FB.API("/me?fields=id,name,email", HttpMethod.GET, LoginCallback);
//			getScores();
		} else {
			Debug.Log("User cancelled login");
		}
	}

	private void LoginCallback(IGraphResult result){
		name = result.ResultDictionary["name"].ToString();
		facebook_id = result.ResultDictionary["id"].ToString();
		email = result.ResultDictionary["email"].ToString();

	}

	private void saveScore(){
		HttpUtils httpUtils = new HttpUtils();
        string url = "http://openmultiplayer.com/mobileserver/message/XYZ-ABC-123/ranking";
		Ranking rank = new Ranking();
		rank.facebook_id = facebook_id;
		rank.name = name;
		rank.email = email;
		rank.score = score;
		string json = JsonUtility.ToJson(rank);
        StartCoroutine(httpUtils.sendHttpDataJson(url, json, null));		
	}

    private void ScoreCallBack(IGraphResult result){
		Debug.Log("score saved...." + score);
    }

	private void getScores(){
		FB.API("/me/scores", HttpMethod.GET, GetScoreCallBack);
	}

    private void GetScoreCallBack(IGraphResult result){
		Debug.Log(result);
    }

	void OnGUI() {
		if (facebook_id == null) {
			if (GUI.Button(new Rect(10, 10, 500, 500), "Login")){
				loginFacebook();
			}
		}else{
			if (GUI.Button(new Rect(10, 70, 500, 500), "Click")){
				score++;
			}	
			if (GUI.Button(new Rect(10, 600, 500, 500), "Save")){
				saveScore();
			}	
		}
	}
}

