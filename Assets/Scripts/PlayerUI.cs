using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("Bottom Panel")]
    [SerializeField] private TextMeshProUGUI name;
    [Header("Hearts")]
    [SerializeField] private Image heart1;
    [SerializeField] private Image heart2;
    [SerializeField] private Image heart3;
    [SerializeField] private Sprite State1;
    [SerializeField] private Sprite State2;
    [Header("Buttons")]
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Button pathButton;

    [Header("Inventory Panel")]
    [SerializeField] private GameObject inventoryPanel;
    [Header("Potion")]
    [SerializeField] private Button potionButton;
    [SerializeField] private TextMeshProUGUI potionText;
    [Header("Axe")]
    [SerializeField] private Button axeButton;
    [SerializeField] private TextMeshProUGUI axeText;
    [Header("Sword")]
    [SerializeField] private Button swordButton;
    [SerializeField] private TextMeshProUGUI swordText;

    [Header("Hide Button")]
    [SerializeField] private Button hideButton;

    private Minotaur minotaur;

    public void Start()
    {
        name.text = Globals.playerName; // assign player name to UI
    }

    public void LateStart()
    {
        minotaur = GameObject.Find("Minotaur(Clone)").GetComponent<Minotaur>();
    }

    public void Update()
    {
        UpdateInventory(); // updates the inventory UI text every frame
        UpdateHealth(); // updates the health UI every frame
    }

    public void Sword()
    {
        if (Globals.inventory.Contains(Collectable.CollectableType.Sword)) // checks to make sure the player has the required item
        {
            minotaur.kill();
            Globals.inventory.Remove(Collectable.CollectableType.Sword); // removes the item from the inventory
        }
    }

    public void UpdateInventory()
    {
        int numSwords = 0;
        int numAxes = 0;
        int numPotions = 0;

        // loops through the inventory and counts the number of each item
        foreach (Collectable.CollectableType item in Globals.inventory)
        {
            switch(item)
            {
                case Collectable.CollectableType.Sword:
                    numSwords++;
                    break;
                case Collectable.CollectableType.Axe:
                    numAxes++;
                    break;
                case Collectable.CollectableType.Potion:
                    numPotions++;
                    break;
                default:
                    break;
            }

            // updates the UI text to display the number of each item
            swordText.text = $"x{numSwords.ToString()}";
            axeText.text = $"x{numAxes.ToString()}";
            potionText.text = $"x{numPotions.ToString()}";
        }
    }

    public void UpdateHealth()
    {
        switch(Globals.health) // checks for each possible health value and updates the UI accordingly
        {
            case 3:
                heart1.sprite = State1;
                heart2.sprite = State1;
                heart3.sprite = State1;
                break;
            case 2:
                heart1.sprite = State1;
                heart2.sprite = State1;
                heart3.sprite = State2;
                break;
            case 1:
                heart1.sprite = State1;
                heart2.sprite = State2;
                heart3.sprite = State2;
                break;
            case 0:
                heart1.sprite = State2;
                heart2.sprite = State2;
                heart3.sprite = State2;
                break;
            default:
                break;
        }
    }
    public void CloseInventory()
    {
        inventoryPanel.SetActive(false); // sets the inventory panel to inactive
    }

    public void OpenInventory()
    {
        inventoryPanel.SetActive(true); // sets the inventory panel to active
    }
}
