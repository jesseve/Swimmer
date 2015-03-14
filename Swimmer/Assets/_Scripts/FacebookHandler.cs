using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FacebookHandler : MonoBehaviour {

    public Canvas FBCanvas;
    public Image profilePicture;
    public Text userName;

    public GameObject scoreEntry;
    public Transform scoreEntries;

    private Dictionary<string, string> profile = null;

    private List<object> scoreList;

    void Awake()
    {
        FB.Init(SetInit, OnHideUnity);
    }

    private void SetInit() {
        Debug.Log("FB init done");

        if (FB.IsLoggedIn)
        {
            LevelManager.instance.LogInSuccesfully();
        }
        else
        {
            //FBlogin();
        }        
    }

    private void OnHideUnity(bool isGameShown) {
        if (!isGameShown) 
        {
            Time.timeScale = 0;
        }
        else 
        {
            Time.timeScale = 1;
        }
    }

    public void FBlogin() {
        FB.Login("email,publish_actions", AuthCallback);
    }

    void AuthCallback(FBResult result) {        
        if (FB.IsLoggedIn)
        {
            Debug.Log("FBLogin success");
            LevelManager.instance.LogInSuccesfully();
            FB.API(Util.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, DealWithProfilePicture);
            FB.API("/me?fields=id,first_name", Facebook.HttpMethod.GET, DealWithName);
        }
        else
        {
            Debug.Log("FBLogin fail");
        }
        DealWithMenus(FB.IsLoggedIn);
    }

    void DealWithMenus(bool isLoggedIn) {
        FBCanvas.enabled = isLoggedIn;
        if (isLoggedIn) 
        {
            FB.API(Util.GetPictureURL("me", 128,128), Facebook.HttpMethod.GET, DealWithProfilePicture);
            FB.API("/me?fields=id,first_name", Facebook.HttpMethod.GET, DealWithName);

        }
        else
        {

        }
    }

    void DealWithProfilePicture(FBResult result) 
    {
        if (result.Error != null) 
        {
            Debug.Log("Error getting profile picture");
        }
        else
        {
            Debug.Log("Got picture");
            profilePicture.sprite = Sprite.Create(result.Texture, new Rect(0,0, 128,128), Vector2.zero);
            Color c = profilePicture.color;
            c.a = 1f;
            profilePicture.color = c;
        }
    }

    void DealWithName(FBResult result)
    {
        if (result.Error != null)
        {
            Debug.Log("Error getting profile picture");
            return;
        }
        profile = Util.DeserializeJSONProfile(result.Text);
        userName.text = profile["first_name"];        
        
    }

    public void QueryScore()
    {
        FB.API("/app/scores?fields=score,user.limit(30)", Facebook.HttpMethod.GET, ScoreCallback);
    }

    private void ScoreCallback(FBResult result)
    {
        Debug.Log(result.Text);
        scoreList = Util.DeserializeScores(result.Text);

        foreach(Transform t in scoreEntries)
        {
            GameObject.Destroy(t.gameObject);
        }

        foreach (object s in scoreList)
        {
            var entry = (Dictionary<string, object>) s;
            var user = (Dictionary<string, object>) entry["user"];

            Debug.Log("UN: " + user["name"] + " - Score: " + entry["score"]);

            GameObject SE = Instantiate(scoreEntry) as GameObject;
            SE.transform.SetParent(scoreEntries);

            Image avatar = SE.transform.Find("FriendAvatar").GetComponent<Image>();
            Text name = SE.transform.Find("FriendName").GetComponent<Text>();
            Text score = SE.transform.Find("FriendScore").GetComponent<Text>();

            name.text = user["name"].ToString();
            score.text = entry["score"].ToString();

            FB.API(Util.GetPictureURL(user["id"].ToString(), 128, 128), Facebook.HttpMethod.GET, delegate (FBResult scoreResult) {
                if (scoreResult.Error == null) 
                {
                    avatar.sprite = Sprite.Create(scoreResult.Texture, new Rect(0, 0, 128, 128), Vector2.zero);
                }
                else
                {
                    Debug.Log("Error setting picture:" + scoreResult.Error);
                }
            });
        }
    }

    public void SetScore(float time)
    {
        var scoreData = new Dictionary<string, string>();
        //scoreData["score"] = time.ToString();

        scoreData["score"] = LevelManager.instance.CalculateScore(time);

        Debug.Log(scoreData["score"]);

        FB.API("/me/scores", Facebook.HttpMethod.POST, delegate(FBResult result) {
            Debug.Log("Score submitted: " + result.Text);
        }, scoreData);
    }

    public void ShareWithFriends()
    {
        FB.Feed(
            linkCaption: "I'm playing this awesome game",
            picture: "https://www.google.fi/search?q=kuva&safe=off&biw=1366&bih=643&tbm=isch&imgil=kspEljf4MlfS8M%253A%253BSz-9WDxpId3q0M%253Bhttp%25253A%25252F%25252Fwww.taku.fi%25252Fkuva-arkisto&source=iu&pf=m&fir=kspEljf4MlfS8M%253A%252CSz-9WDxpId3q0M%252C_&usg=__E14zIUaM-J_rUedSz2j2n8SNU3M%3D&ved=0CC4Qyjc&ei=1ND-VNSkJsX5yQPlqoLwDw#imgrc=kspEljf4MlfS8M%253A%3BSz-9WDxpId3q0M%3Bhttp%253A%252F%252Fwww.taku.fi%252Ffiles%252F755%252Fkuva(1).JPG%3Bhttp%253A%252F%252Fwww.taku.fi%252Fkuva-arkisto%3B2592%3B1936",//"users.metropolia.fi/~jesseve/pic.png", //need url
            linkName: "Check out this game",
            link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest")
        );
    }

    public void InviteFriends()
    {
        FB.AppRequest(
            message: "This game is awasome try now",
            title: "Invite your friends to join you now"
        );
    }
}
