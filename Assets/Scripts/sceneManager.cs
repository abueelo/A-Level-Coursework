using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Steamworks;
using TMPro;

public class sceneManager : MonoBehaviour
{
    public enum Scenes { // enum for scenes
        MainMenu,
        RandomLevel,
        CasinoScene
    }

    public TextMeshProUGUI fullscreenText;

    public void ChangeScene(Scenes scene){
        SceneManager.LoadScene(ConvertSceneToString(scene)); // loads scene based on enum
    }

    public string ConvertSceneToString(Scenes scene){ // converts scene enum to string
        switch (scene)
        {
            case Scenes.RandomLevel:
                return "Random Level";
            case Scenes.CasinoScene:
                return "Casino";
            default:
                return "Main Menu";
        }
    }

    public void Continue(){ // loads new random level
        ChangeScene(Scenes.RandomLevel);
    }

    public void Fullscreen(){
        // toggles fullscreen mode and changes text accordingly
        if(Screen.fullScreen){
            fullscreenText.text = "Go Big";
            Screen.fullScreen = false;
        }else{
            fullscreenText.text = "Go Small";
            Screen.fullScreen = true;
        }
    }

    public void Quit(){
        //if current scene is main menu, quit the game
        if(SceneManager.GetActiveScene().name == "Main Menu"){
            Debug.Log("Quitting game");
            Application.Quit();
        }else{ // else go back to main menu
            ChangeScene(Scenes.MainMenu);
        }
    }

    public void PlayButton(){
        SteamAPI.Init(); // attempts to initialize Steam API
        if(SteamManager.Initialized){ // gets steam name if it is initialized
            Debug.Log("Steam is initialized, setting player name.");
            Globals.playerName = SteamFriends.GetPersonaName();
        }else{ // else sets player name to a default value
            Debug.Log("Steam is not initialized, setting player name to anon.");
            Globals.playerName = "anon";
        }
        Continue();
    }

    void Start()
    {
        // sets fullscreen text to corresponding value on scene load
        if(Screen.fullScreen){
            fullscreenText.text = "Go Small";
        }else{
            fullscreenText.text = "Go Big";
        }
    }
}
