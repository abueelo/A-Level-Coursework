using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WDUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI deathText;
    [SerializeField] private TextMeshProUGUI winCountText;
    [SerializeField] private GameObject bkg;

    public void Win() // called when the player wins
    {
        bkg.SetActive(true); // activates the death/win panel
        winText.gameObject.SetActive(true); // shows win text
        deathText.gameObject.SetActive(false);  // makes sure death text is hidden
        winCountText.gameObject.SetActive(false); // makes sure win count text is hidden
        gameObject.SetActive(true); // shows the panel to the player
        GameObject.Find("Minotaur(Clone)").GetComponent<Minotaur>().kill(); // kills the minotaur
        StartCoroutine(ExitPause()); // start countdown to next level
    }

    public void Die() // called when the player dies
    {
        bkg.SetActive(true); // activates the death/win panel
        winText.gameObject.SetActive(false); // makes sure win text is hidden
        deathText.gameObject.SetActive(true); // shows death text
        winCountText.gameObject.SetActive(true); // shows win count text
        winCountText.text = $"You've won {Globals.wins} games"; // sets the win count text
        gameObject.SetActive(true); // shows the panel to the player
        StartCoroutine(QuitPause()); // start countdown to quit
    }

    IEnumerator ExitPause()
    {
        yield return new WaitForSeconds(3f); // waits 3 seconds
        SceneManager.LoadScene("Random Level"); // loads the next level
    }
    IEnumerator QuitPause()
    {
        yield return new WaitForSeconds(3f); // waits 3 seconds
        SceneManager.LoadScene("Main Menu"); // loads the main menu
    }
}
