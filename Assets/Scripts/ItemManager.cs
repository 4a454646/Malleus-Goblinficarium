﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;
using System;

public class ItemManager : MonoBehaviour {
    [SerializeField] public TextMeshProUGUI lootText;
    [SerializeField] public TextMeshProUGUI itemDesc;
    [SerializeField] private GameObject item;
    [SerializeField] public GameObject highlight;
    [SerializeField] public GameObject highlightedItem;
    [SerializeField] private Sprite[] commonItemSprites;
    [SerializeField] private Sprite[] rareItemSprites;
    [SerializeField] public Sprite[] weaponSprites;
    [SerializeField] private Sprite[] otherSprites;
    [SerializeField] public List<GameObject> floorItems;
    public List<GameObject> deletionQueue = new List<GameObject>();
    public string[] statArr = new string[] { "green", "blue", "red", "white" };
    public string[] statArr1 = new string[] { "accuracy", "speed", "damage", "parry" };
    public Dictionary<string, Dictionary<string, int>> weaponDict = new Dictionary<string, Dictionary<string, int>>() {
        { "dagger",   new Dictionary<string, int>() { { "green", 2 }, { "blue", 4 }, { "red", 0 }, { "white", 0 } } },
        { "flail",    new Dictionary<string, int>() { { "green", 0 }, { "blue", 3 }, { "red", 1 }, { "white", 0 } } },
        { "hatchet",  new Dictionary<string, int>() { { "green", 1 }, { "blue", 2 }, { "red", 2 }, { "white", 1 } } },
        { "mace",     new Dictionary<string, int>() { { "green", 1 }, { "blue", 3 }, { "red", 2 }, { "white", 0 } } },
        { "maul",     new Dictionary<string, int>() { { "green",-1 }, { "blue",-1 }, { "red", 3 }, { "white", 1 } } },
        { "montante", new Dictionary<string, int>() { { "green", 1 }, { "blue", 1 }, { "red", 3 }, { "white", 2 } } },
        { "rapier",   new Dictionary<string, int>() { { "green", 4 }, { "blue", 2 }, { "red",-1 }, { "white", 1 } } },
        { "scimitar", new Dictionary<string, int>() { { "green", 0 }, { "blue", 2 }, { "red", 1 }, { "white", 2 } } },
        { "spear",    new Dictionary<string, int>() { { "green", 2 }, { "blue",-1 }, { "red", 3 }, { "white", 1 } } },
        { "sword",    new Dictionary<string, int>() { { "green", 1 }, { "blue", 2 }, { "red", 1 }, { "white", 2 } } },
    };  
    public string[] weaponNames = new string[] { "dagger", "flail", "hatchet", "mace", "maul", "montante", "rapier", "scimitar", "spear", "sword" };
    public Dictionary<string, Dictionary<string, int>> modifierDict = new Dictionary<string, Dictionary<string, int>>() {
        { "accurate0", new Dictionary<string, int>() { { "green", 1 }, { "blue", 0 }, { "red", 0 }, { "white", 0 } } },
        { "accurate1", new Dictionary<string, int>() { { "green", 2 }, { "blue",-1 }, { "red", 0 }, { "white", 0 } } },
        { "brisk0",    new Dictionary<string, int>() { { "green", 0 }, { "blue", 1 }, { "red",-1 }, { "white", 0 } } },
        { "brisk1",    new Dictionary<string, int>() { { "green", 0 }, { "blue", 1 }, { "red", 0 }, { "white",-1 } } },
        { "blunt0",    new Dictionary<string, int>() { { "green", 0 }, { "blue", 0 }, { "red", 0 }, { "white", 1 } } },
        { "blunt1",    new Dictionary<string, int>() { { "green", 0 }, { "blue", 0 }, { "red",-1 }, { "white", 1 } } },
        { "common0",   new Dictionary<string, int>() { { "green", 0 }, { "blue", 0 }, { "red", 0 }, { "white", 0 } } },
        { "common1",   new Dictionary<string, int>() { { "green", 0 }, { "blue", 0 }, { "red", 0 }, { "white", 0 } } },
        { "common2",   new Dictionary<string, int>() { { "green", 0 }, { "blue", 0 }, { "red", 0 }, { "white", 0 } } },
        { "common3",   new Dictionary<string, int>() { { "green", 0 }, { "blue", 0 }, { "red", 0 }, { "white", 0 } } },
        { "common4",   new Dictionary<string, int>() { { "green", 0 }, { "blue", 0 }, { "red", 0 }, { "white", 0 } } },
        { "common5",   new Dictionary<string, int>() { { "green", 0 }, { "blue", 0 }, { "red", 0 }, { "white", 0 } } },
        { "heavy0",    new Dictionary<string, int>() { { "green", 0 }, { "blue",-1 }, { "red", 1 }, { "white", 0 } } },
        { "heavy1",    new Dictionary<string, int>() { { "green", 0 }, { "blue",-1 }, { "red", 0 }, { "white", 1 } } },
        { "nimble0",   new Dictionary<string, int>() { { "green", 0 }, { "blue", 1 }, { "red", 0 }, { "white",-1 } } },
        { "nimble1",   new Dictionary<string, int>() { { "green", 0 }, { "blue", 1 }, { "red",-1 }, { "white", 1 } } },
        { "precise0",  new Dictionary<string, int>() { { "green", 1 }, { "blue", 0 }, { "red",-1 }, { "white", 0 } } },
        { "precise1",  new Dictionary<string, int>() { { "green", 1 }, { "blue",-1 }, { "red", 0 }, { "white", 0 } } },
        { "ruthless0", new Dictionary<string, int>() { { "green", 1 }, { "blue",-1 }, { "red",-1 }, { "white", 1 } } },
        { "ruthless1", new Dictionary<string, int>() { { "green", 1 }, { "blue",-1 }, { "red",-1 }, { "white", 0 } } },
        { "stable0",   new Dictionary<string, int>() { { "green",-1 }, { "blue", 0 }, { "red", 0 }, { "white", 1 } } },
        { "stable1",   new Dictionary<string, int>() { { "green", 0 }, { "blue",-1 }, { "red", 0 }, { "white", 1 } } },
        { "sharp0",    new Dictionary<string, int>() { { "green", 0 }, { "blue", 0 }, { "red", 1 }, { "white", 0 } } },
        { "sharp1",    new Dictionary<string, int>() { { "green", 1 }, { "blue",-1 }, { "red", 1 }, { "white", 0 } } },
        { "harsh0",    new Dictionary<string, int>() { { "green", 1 }, { "blue", 0 }, { "red", 0 }, { "white", 0 } } },
        { "quick0",    new Dictionary<string, int>() { { "green", 0 }, { "blue", 1 }, { "red", 0 }, { "white", 0 } } },
    };
    private string[] modifierNames = new string[] { "accurate0", "accurate1", "brisk0", "brisk1", "blunt0", "blunt1", "common0", "common1", "common2", "common3", "common4", "common5", "heavy0", "heavy1", "nimble0", "nimble1", "precise0", "precise1", "ruthless0", "ruthless1", "stable0", "stable1", "sharp0", "sharp1", "harsh0", "quick0" };
    public Dictionary<string, string> descriptionDict = new Dictionary<string, string>() {
        {"armor", "protects from one hit"}, 
        {"arrow", "proceed to the next level"}, 
        {"ankh", "force new draft"}, 
        {"boots of dodge", "pay 1 stamina to become dodgy"}, 
        {"broken helm", ""}, 
        {"cheese", "3"}, 
        {"dagger", "green dice buff damage"}, 
        {"defiled kapala", ""}, 
        {"flail", "start with red die"}, 
        {"hatchet", "enemy can't choose yellow die"},  
        {"helm of might", "pay 3 stamina to gain a yellow die"}, 
        {"kapala", "offer an item to become furious"}, 
        {"mace", "reroll all dice still to be picked"}, 
        {"maul", "any wound is deadly"}, 
        {"montante", ""}, 
        {"necklet", ""}, 
        {"phylactery", "gain \"leech\" buff once wounded"},
        {"potion", ""}, 
        {"rapier", "gain 3 stamina upon kill"}, 
        {"retry", "retry?"}, 
        {"ruined boots", ""}, 
        {"scimitar", "discard enemy's die upon parry"},  
        {"scroll", ""}, 
        {"shuriken", "discard enemy's die"}, 
        {"skeleton key", "escape the fight"}, 
        {"shattered ankh", ""}, 
        {"spear", "always choose first die"}, 
        {"steak", "5"}, 
        {"sword", ""}, 
        {"torch", "find more loot"} 
    };
    public string[] neckletTypes = new string[] { "solidity", "rapidity", "strength", "defense", "arcane", "nothing", "victory" };
    public Dictionary<string, int> neckletStats = new Dictionary<string, int>() 
        { { "green", 0 }, { "blue", 0 }, { "red", 0 }, { "white", 0 } };
    public Dictionary<string, int> neckletCounter = new Dictionary<string, int>() 
        { { "green", 0 }, { "blue", 0 }, { "red", 0 }, { "white", 0 }, { "arcane", 1 } };
    // start with 1 arcane necklet so we don't have to have +1's everywhere
    private string[] scrollTypes = new string[] { "fury", "haste", "dodge", "leech", "courage", "challenge", "nothing" };
    private string[] potionTypes = new string[] { "accuracy", "speed", "strength", "defense", "might", "life", "nothing" };
    private Sprite[] allSprites;
    public float itemSpacing = 1f;
    public float itemY = -5.3f;
    private Scripts scripts;
    public int col = 0;
    public List<GameObject> curList;
    public int discardableDieCounter = 0;
    public bool usedAnkh = false;
    public bool usedHelm = false;
    public bool usedBoots = false;
    public int numItemsDroppedForTrade = 0;
    public bool isCharSelect = false;

    void Start() {
        allSprites = commonItemSprites.ToArray().Concat(rareItemSprites.ToArray()).Concat(weaponSprites.ToArray()).Concat(otherSprites.ToArray()).ToArray();
        // create a list containing all of the sprites
        scripts = FindObjectOfType<Scripts>();
        if (isCharSelect) {
            // in characer selection screen
            curList = floorItems;
            // assign the curlist variable for item selection navigation
            CreateWeaponWithStats("sword", "harsh", 2, 2, 1, 2);
            MoveItemToDisplay();
            CreateItem("steak", "common");
            MoveItemToDisplay();
            GameObject created = CreateItem("torch", "common");
            MoveItemToDisplay();
            // create the items
        }
        else {
            // in game
            lootText.text = "";
            curList = scripts.player.inventory;
            // assign the curlist variable for item selection navigation
            // need to implement a check if continuing or new game
        }
        usedAnkh = Save.game.usedAnkh;
        usedBoots = Save.game.usedBoots;
        usedHelm = Save.game.usedHelm;
        numItemsDroppedForTrade = Save.game.numItemsDroppedForTrade;
        discardableDieCounter = Save.game.discardableDieCounter;
        // assign variables based on the Save, preventing cheating
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) {
            // if pressing one of the ctrl keys
            if (!isCharSelect) {
                // if not in char select scene
                if (curList == scripts.player.inventory) { Select(floorItems, 0); }
                else if (curList == floorItems) { Select(scripts.player.inventory, 0); }
                // swap the curList (used for selection) to the other
                // else { print("invalid list to select from"); }
                // something is wrong here
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && SceneManager.GetActiveScene().name != "CharSelect") {
            // if pressing left and not in the character select screen
            Select(curList, col - 1, false);
            // move the selection left
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && SceneManager.GetActiveScene().name != "CharSelect") {
            // if pressing right and not in the character select screen
            Select(curList, col + 1, false);
            // move the selection to the right 
        }
        else if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))) {
            // if pressing return or enter
            if (scripts.turnManager != null && !scripts.turnManager.isMoving && !isCharSelect) {
                // if not moving and in game
                highlightedItem.GetComponent<Item>().Use();
                // use the item
            }
        }
        else if (Input.GetKeyDown(KeyCode.N)) {
            CreateItem("common");
        }
    } 


    /// <summary>
    /// Give the player their starting items, based on chosen class.
    /// </summary>
    public void GiveStarterItems(int charNum) {
        if (Save.game.newGame) { 
            // new game, so give the base weapons
            if (scripts.player.charNum == 0) { 
                CreateWeaponWithStats("sword", "harsh", 2, 2, 1, 2);
                MoveToInventory(0, true, false, false);

                // CreateWeaponWithStats("maul", "administrative", 10, 10, 10, 10);
                // MoveToInventory(0, true, false, false);
                // CreateItem("scroll", "common", "challenge");
                // MoveToInventory(0, true, false, false);

                CreateItem("steak", "common");
                MoveToInventory(0, true, false, false);
                if (Save.persistent.easyMode) { 
                    CreateItem("torch", "common");
                    MoveToInventory(0, true, false, false);
                }
            }
            else if (scripts.player.charNum == 1) { 
                CreateWeaponWithStats("maul", "common", -1, -1, 3, 1);
                MoveToInventory(0, true, false, false);
                CreateItem("armor", "common");
                MoveToInventory(0, true, false, false);
                if (Save.persistent.easyMode) {
                    CreateItem("helm_of_might", "rare");
                    MoveToInventory(0, true, false, false);
                }
            }
            else if (scripts.player.charNum == 2) { 
                CreateWeaponWithStats("dagger", "quick", 2, 5, 0, 0);
                MoveToInventory(0, true, false, false);
                CreateItem("boots_of_dodge", "rare");
                MoveToInventory(0, true, false, false);
                if (Save.persistent.easyMode) { 
                    CreateItem("ankh", "rare");
                    MoveToInventory(0, true, false, false);
                }
            }
            else if (scripts.player.charNum == 3) {
                CreateWeaponWithStats("mace", "ruthless", 2, 2, 1, 0);
                MoveToInventory(0, true, false, false);
                CreateItem("cheese", "common");
                MoveToInventory(0, true, false, false);
                if (Save.persistent.easyMode) { 
                    CreateItem("kapala", "rare");
                    MoveToInventory(0, true, false, false);
                }
            }
        }
        else { 
            // continuing previously existing game
            CreateWeaponWithStats(Save.game.resumeItemNames[0], Save.game.resumeItemMods[0], Save.game.resumeAcc, Save.game.resumeSpd, Save.game.resumeDmg, Save.game.resumeDef);
            MoveToInventory(0, true, false, false);
            // create their old weapon and given them it
            for (int i = 1; i < 9; i++) {
                if (Save.game.resumeItemNames[i] == "") { break; }
                CreateItem(Save.game.resumeItemNames[i].Replace(' ', '_'), Save.game.resumeItemTypes[i], Save.game.resumeItemMods[i]);
                MoveToInventory(0, true, false, false);
            }
            // create the old items and add them in
        }
        Select(curList, 0, playAudio:false);
        // select the first item
        SaveInventoryItems();
    }

    /// <summary>
    /// Select an item from the given list at the given index. 
    /// </summary>
    public void Select(List<GameObject> itemList, int c, bool forceDifferentSelection=true, bool playAudio=true) {
        if (c <= itemList.Count - 1 && c >= 0) {
            // if the column is within the bounds of the list
            itemList[c].GetComponent<Item>().Select(playAudio);
            // select the object
            curList = itemList;
            col = c;
            // update the variables used for selection 
        }
        else {
            // column is invalid for the list
            if (forceDifferentSelection) {
                // if we want to force a different selection
                if (itemList.Count > 1) {
                    try {
                        // if there is more than 1 item
                        itemList[col - 1].GetComponent<Item>().Select();
                        // select the next item over
                        curList = itemList;
                        col--;
                        // update the variables used for selection
                    }
                    catch { 
                        itemList[0].GetComponent<Item>().Select();
                        // select item 0
                        curList = itemList;
                        col = 0;
                        // update variables
                    }
                }
                else {
                    // player only has weapon
                    if (scripts.player != null) { 
                        scripts.player.inventory[0].GetComponent<Item>().Select(); 
                        curList = scripts.player.inventory;
                    }
                    else { 
                        floorItems[0].GetComponent<Item>().Select(); 
                        curList = floorItems;
                    }
                    col = 0;
                    // select the weapon and update the variables used for selection.
                }
            }
            else {
                highlightedItem.GetComponent<Item>().Select();
                // not forcing a different selection, so select the item regardless
            }
        }
    }

    /// <summary>
    /// Get the sprite of an item given its name.
    /// </summary>
    public Sprite GetItemSprite(string name) { return allSprites[(from a in allSprites select a.name).ToList().IndexOf(name)]; }

    /// <summary>
    /// Create an item with specified type.
    /// </summary>
    private GameObject CreateItem(string itemType, int negativeOffset=0) {
        GameObject instantiatedItem = Instantiate(item, new Vector2(-2.75f + (floorItems.Count - negativeOffset) * itemSpacing, itemY), Quaternion.identity);
        // create an item object at the correct position
        Sprite sprite = null;
        // create a variable of which we can place the sprite upon to depending on the item type
        if (itemType == "weapon") { sprite = weaponSprites[UnityEngine.Random.Range(0, weaponSprites.Length)]; }
        else if (itemType == "common") { 
            int rand = UnityEngine.Random.Range(0, commonItemSprites.Length);
            // get a random item from the list of names
            if (rand == 0 || rand == 8) { 
                if (UnityEngine.Random.Range(0, 2) == 0) { sprite = commonItemSprites[rand]; } 
                else { sprite = commonItemSprites[UnityEngine.Random.Range(0, commonItemSprites.Length)]; }
            }
            // armor and skeleton keys are about 2x rarer than other items
            else { sprite = commonItemSprites[rand]; }
        }
        else if (itemType == "rare") { sprite = rareItemSprites[UnityEngine.Random.Range(0, rareItemSprites.Length)]; }
        // depending on the type, give it a corresponding random sprite.
        // else { print("bad item type to create"); }
        // can only create item types of weapon, common, and rare
        instantiatedItem.GetComponent<SpriteRenderer>().sprite = sprite;
        // give the sprite renderer the proper sprite 
        instantiatedItem.transform.parent = gameObject.transform;
        // make the item childed to this manager
        instantiatedItem.GetComponent<Item>().itemName = sprite.name.Replace("_", " ");
        instantiatedItem.GetComponent<Item>().itemType = itemType;
        // assign the attributes for the name and the type of the item 
        SetItemStatsImmediately(instantiatedItem);
        // if needed, immediately give the item its proper attributes
        floorItems.Add(instantiatedItem);         
        // add the item to the array
        return instantiatedItem;
    }

    /// <summary>
    /// Create an item with the specified name and type.
    /// </summary>
    public GameObject CreateItem(string itemName, string itemType, int negativeOffset=0) {
        GameObject instantiatedItem = Instantiate(item, new Vector2(-2.75f + (floorItems.Count - negativeOffset) * itemSpacing, itemY), Quaternion.identity);
        // instantiate the item
        instantiatedItem.GetComponent<SpriteRenderer>().sprite = GetItemSprite(itemName);
        // give the item the proper sprite
        instantiatedItem.transform.parent = gameObject.transform;
        // make the item childed to this manager
        instantiatedItem.GetComponent<Item>().itemName = instantiatedItem.GetComponent<SpriteRenderer>().sprite.name.Replace("_", " ");
        instantiatedItem.GetComponent<Item>().itemType = itemType;
        // assign the attributes for the name and the type of the item
        SetItemStatsImmediately(instantiatedItem);
        // if needed, immediately give the item its proper attributes
        floorItems.Add(instantiatedItem);
        // add the item to the array
        return instantiatedItem;
    }

    /// <summary>
    /// Create an item with the specified name, type, and modifier.
    /// </summary>
    public GameObject CreateItem(string itemName, string itemType, string modifier, int negativeOffset=0) {
        GameObject instantiatedItem = Instantiate(item, new Vector2(-2.75f + (floorItems.Count - negativeOffset) * itemSpacing, itemY), Quaternion.identity);
        // instantiate the item
        instantiatedItem.GetComponent<SpriteRenderer>().sprite = GetItemSprite(itemName);
        // give the item the proper sprite
        instantiatedItem.transform.parent = gameObject.transform;
        // make the item childed to this manager
        instantiatedItem.GetComponent<Item>().itemName = instantiatedItem.GetComponent<SpriteRenderer>().sprite.name.Replace("_", " ");
        instantiatedItem.GetComponent<Item>().itemType = itemType;
        // assign the attributes for the name and the type of the item
        if (modifier != "") { instantiatedItem.GetComponent<Item>().modifier = modifier; }
        // if needed, immediately give the item its proper attributes
        floorItems.Add(instantiatedItem);
        // add the item to the array
        return instantiatedItem;
    }

    // ^ i love overloading functions!

    /// <summary>
    /// Instantly assign necessary attributes of items (like their modifier).
    /// </summary>
    private void SetItemStatsImmediately(GameObject instantiatedItem) {
        // this needs to be done here rather than in Item.Start() or Awake() because the timing will be off and errors will be thrown
        if (instantiatedItem.GetComponent<Item>().itemName == "necklet") {
            instantiatedItem.GetComponent<Item>().modifier = neckletTypes[UnityEngine.Random.Range(0, 5)];
        }
        else if (instantiatedItem.GetComponent<Item>().itemName == "scroll") {
            instantiatedItem.GetComponent<Item>().modifier = scrollTypes[UnityEngine.Random.Range(0, scrollTypes.Length)];
        }
        else if (instantiatedItem.GetComponent<Item>().itemName == "potion") {
            instantiatedItem.GetComponent<Item>().modifier = potionTypes[UnityEngine.Random.Range(0, potionTypes.Length)];
        }
        // assign a modifier for a necklet, scroll, or potion
    }

    /// <summary>
    /// Create a weapon with randomized modifier and stats. Use to generate a weapon when the player slays the enemy.
    /// </summary>
    public void CreateRandomWeapon() {
        Dictionary<string, int> baseWeapon = new Dictionary<string, int>() { { "green", 0 }, { "blue", 0 }, { "red", 0 }, { "white", 0 } };
        GameObject instantiatedItem = Instantiate(item, new Vector2(-2.75f + floorItems.Count * itemSpacing, itemY), Quaternion.identity);
        // instantiate the item
        int rand = UnityEngine.Random.Range(0, weaponNames.Length);
        // get a random variable of which we will pull weapon information from
        instantiatedItem.GetComponent<SpriteRenderer>().sprite = weaponSprites[rand];
        // get the sprite from the random number
        instantiatedItem.transform.parent = gameObject.transform;
        // child the item to this manager
        instantiatedItem.GetComponent<Item>().itemType = "weapon";
        // set the itemtype to be a weaon
        string modifier = modifierNames[UnityEngine.Random.Range(0, modifierNames.Length)];
        // pull a random modifier from the array
        instantiatedItem.GetComponent<Item>().modifier = modifier.Substring(0, modifier.Length - 1);
        // assign the modifier attribute (cut off the final letter, as modifiers are like 'common0' and 'common1')
        instantiatedItem.GetComponent<Item>().itemName = instantiatedItem.GetComponent<Item>().modifier + " " + weaponNames[rand];
        // concatenate the modifier with weapon name and assign the item's name attribute to be that
        // get the base weapon's stats from the array of dictionaries gotten from the previous random number
        foreach (string key in statArr) { 
            // for every key in the stat array ("green", "blue", etc.)
            baseWeapon[key] = weaponDict[weaponNames[rand]][key] + modifierDict[modifier][key]; 
            // add the modifier stats to the weapon stats
            if (baseWeapon[key] < -1) { baseWeapon[key] = -1; }
            // limit the item so it can't go down to -2 (not in the actual game, but in my modded version later i may do this)
        }
        instantiatedItem.GetComponent<Item>().weaponStats = baseWeapon;
        // assign the weapon stats to the weapon
        floorItems.Add(instantiatedItem);
        // add the item to the array
        Save.game.floorAcc = baseWeapon["green"];
        Save.game.floorSpd = baseWeapon["blue"];
        Save.game.floorDmg = baseWeapon["red"];
        Save.game.floorDef = baseWeapon["white"];
        if (scripts.tutorial is null) { Save.SaveGame(); }
    }

    /// <summary>
    /// Create a weapon with specified name, modifier, and stats.
    /// </summary>
    public GameObject CreateWeaponWithStats(string weaponName, string modifier, int aim, int spd, int atk, int def) {
        Dictionary<string, int> baseWeapon = new Dictionary<string, int>() { { "green", 0 }, { "blue", 0 }, { "red", 0 }, { "white", 0 } };
        GameObject instantiatedItem = Instantiate(item, new Vector2(-2.75f + floorItems.Count * itemSpacing, itemY), Quaternion.identity);
        // instantiaet the item
        Sprite sprite = null;
        // create an empty sprite variable to imprint on
        sprite = GetItemSprite(weaponName);
        // get the sprite based on the weapon name
        instantiatedItem.GetComponent<SpriteRenderer>().sprite = sprite;
        // give the sprite based on the item name
        instantiatedItem.transform.parent = gameObject.transform;
        // child the item to this manager
        instantiatedItem.GetComponent<Item>().itemName = modifier + " " + sprite.name.Replace("_", " ");
        instantiatedItem.GetComponent<Item>().itemType = "weapon";
        instantiatedItem.GetComponent<Item>().modifier = modifier;
        baseWeapon["green"] = aim >= 0 ? aim : -1;
        baseWeapon["blue"] = spd >= 0 ? spd : -1;
        baseWeapon["red"] = atk >= 0 ? atk : -1;
        baseWeapon["white"] = def >= 0 ? def : -1;
        // if >=0 set normally, otherwise limit to -1 (easier for tombstone stat saving)
        instantiatedItem.GetComponent<Item>().weaponStats = baseWeapon;
        // assign the attributes of the item based on the given parameters
        floorItems.Add(instantiatedItem);
        // add the item to the array
        return instantiatedItem;
    }

    /// <summary>
    /// Move the item at index 0 from the floor to the display in the character selection screen.
    /// </summary>
    public void MoveItemToDisplay() {
        if (!isCharSelect) { 
            // print("This function should only be used in character select!"); 
        }
        else {
            for (int i = 0; i < floorItems.Count; i++) { 
                // for every item on the floor
                floorItems[i].transform.position = new Vector2(-4.572f + itemSpacing * i, 6.612f);
                // change its position to be in the display area
            }
        }
    }

    /// <summary>
    /// Move the floor item at the specifed index into the player's inventory.
    /// </summary>
    public void MoveToInventory(int index, bool starter=false, bool playAudio=true, bool SaveData=true) {
        if (floorItems[index] != null) {
            if (scripts.player.inventory.Count < 7 || floorItems[index].GetComponent<Item>().itemType == "weapon") {
                // if the player doesn't have 7 or more items or is trying to pick up weapon 
                if (!starter && playAudio) { scripts.soundManager.PlayClip("click0"); }
                // if the item is not the starter (so it doesn't instantly play a click), play the click sound
                if (floorItems[index].GetComponent<Item>().itemType == "weapon") { 
                    if (!starter) { Save.persistent.weaponsSwapped++; }
                    // if the item being moved is a weapon 
                    floorItems[index].transform.position = new Vector2(-2.75f, 3.16f);
                    // move the item to the weapon slot
                    scripts.player.stats = floorItems[index].GetComponent<Item>().weaponStats;
                    // set the player's stats to be equal to that of the weapon
                    Save.game.resumeAcc = scripts.player.stats["green"];
                    Save.game.resumeSpd = scripts.player.stats["blue"];
                    Save.game.resumeDmg = scripts.player.stats["red"];
                    Save.game.resumeDef = scripts.player.stats["white"];
                    if (!starter) {
                        // if the weapon is not a starter (so player already has a weapon)
                        scripts.turnManager.SetStatusText("you take " + floorItems[index].GetComponent<Item>().itemName.Split(' ')[1]);
                        // notify the player
                        Destroy(scripts.player.inventory[0]);
                        // destroy the previous weapon
                        scripts.player.inventory[0] = floorItems[index]; 
                        // add the new weapon to the player's inventory
                        scripts.statSummoner.SummonStats();
                        scripts.statSummoner.SetDebugInformationFor("player");
                        // update debug, because player just took a new weapon
                        scripts.turnManager.blackBox.transform.position = scripts.turnManager.onScreen;
                    }
                    else {
                        scripts.player.inventory.Add(floorItems[index]);
                        // item is a starter, so just add it to the player's inventory
                    }
                    floorItems.RemoveAt(0);
                    // remove the item at index 0 (which is the weapon, because the weapon is always created first)
                    foreach(GameObject item in floorItems) {
                        item.transform.position = new Vector2(item.transform.position.x - 1f, itemY);
                        // for every item, shift it over now that an item has been removed
                    }
                    Select(curList, 0);
                    // select the item at index 0
                }
                else {
                    // not a weapon
                    Item tempItem = floorItems[index].GetComponent<Item>();
                    if (tempItem.itemName == "necklet") {
                        if (tempItem.modifier == "solidity") { 
                            neckletStats["green"] += neckletCounter["arcane"]; 
                            neckletCounter["green"]++; 
                        } 
                        else if (tempItem.modifier == "rapidity") { 
                            neckletStats["blue"] += neckletCounter["arcane"];
                            neckletCounter["blue"]++; 
                        } 
                        else if (tempItem.modifier == "strength") { 
                            neckletStats["red"] += neckletCounter["arcane"]; 
                            neckletCounter["red"]++; 
                        } 
                        else if (tempItem.modifier == "defense") { 
                            neckletStats["white"] += neckletCounter["arcane"]; 
                            neckletCounter["white"]++; 
                        }
                        else if (tempItem.modifier == "arcane") {
                            neckletCounter["arcane"]++;    
                            foreach (string stat in statArr) { 
                                neckletStats[stat] = neckletCounter["arcane"] * neckletCounter[stat]; 
                            }
                        } 
                        else if (floorItems[index].GetComponent<Item>().modifier == "nothing") {}
                        else if (floorItems[index].GetComponent<Item>().modifier == "victory") {}
                        // depending on the type of the necklet, modify the stats accordingly
                        StartCoroutine(UpdateUIAfterDelay());
                        // set the debug information and summon the new stats
                    }
                    if (!starter) { 
                        // not a starter item
                        if (tempItem.itemType == "weapon") { 
                            scripts.turnManager.SetStatusText($"you take {scripts.itemManager.descriptionDict[tempItem.itemName.Split(' ')[1]]}"); 
                        }
                        if (tempItem.itemName == "necklet")  { 
                            if (tempItem.modifier == "arcane") { scripts.turnManager.SetStatusText($"you take arcane necklet"); }
                            else { scripts.turnManager.SetStatusText($"you take {tempItem.itemName} of {tempItem.modifier}"); }
                        }
                        else if (tempItem.itemName == "potion" || tempItem.itemName == "scroll") { scripts.turnManager.SetStatusText($"you take {tempItem.itemName} of {tempItem.modifier}"); }
                        else { scripts.turnManager.SetStatusText($"you take {tempItem.itemName}"); }
                        // notify the player of which item that they took
                        if (scripts.tutorial != null) { scripts.tutorial.Increment(); }
                    }
                    // if the item is not a starter item, notify the player that they have picked up the item
                    floorItems[index].transform.position = new Vector2(-2.75f + itemSpacing * scripts.player.inventory.Count, 3.16f);
                    // add the item to the proper location
                    scripts.player.inventory.Add(floorItems[index]);
                    // add the item to the player's inventory
                    floorItems.RemoveAt(index);
                    // and remove it from the floor
                    for (int i = index; i < floorItems.Count; i++) {
                        // for every item after where the removed item was
                        floorItems[i].transform.position = new Vector2(floorItems[i].transform.position.x - 1f, itemY);
                        // shift it over to the proper location
                    }
                    if (playAudio) { Select(curList, index); }
                    else { Select(curList, index, playAudio:false); }
                    // attempt to select the next item of where it was
                }
            }
        }
        else {
            Destroy(floorItems[index]);
            floorItems.RemoveAt(index);
            // something went wrong here, so destroy it
        }
        if (SaveData) { 
            SaveInventoryItems();
            if (scripts.levelManager.level == Save.persistent.tsLevel && scripts.levelManager.sub == Save.persistent.tsSub) { SaveTombstoneItems(); }
            else { SaveFloorItems(); }
            // only Save items if necessary
        }
    }

    /// <summary>
    /// A coroutine to set the debug and summon stats after a delay (0.1s).
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateUIAfterDelay() {
        yield return scripts.delays[0.1f];
        scripts.statSummoner.SetDebugInformationFor("player");
        scripts.statSummoner.SummonStats();
    }

    /// <summary>
    /// Spawn the items after the enemy has died. 
    /// </summary>
    public void SpawnItems() {
        lootText.text = "loot:";
        // set the text 
        if (scripts.tutorial == null) { 
            if (scripts.enemy.enemyName.text == "Lich") {
                CreateItem("phylactery", "phylactery");
                // defeated lich, so give phylactery
            }
            else if (scripts.enemy.enemyName.text == "Devil") {
                CreateItem("necklet", "common", "victory");
                // defeated devil, so give necklet of victory
            }
            else {
                // normal enemy
                int torchCount = (from item in scripts.player.inventory where item.GetComponent<Item>().itemName == "torch" select item).Count();
                // count the number of torches
                int spawnCount = Mathf.Clamp(torchCount + scripts.levelManager.level + 1 + UnityEngine.Random.Range(-2, 0), 0, 5);
                // create a spawn count 
                CreateRandomWeapon();
                // create a random weapon at index 0
                for (int i = 0; i < spawnCount; i++) {
                    CreateItem("common");
                    // create a random number of items based on the spawn count
                }
                if (UnityEngine.Random.Range(0, 13) == 0) { CreateItem("rare"); }
                // low chance to produce rare
            }
        }
        else { 
            CreateItem("steak", "common");
            CreateWeaponWithStats("sword", "lame", 1, 1, 1, 1);
        }
        CreateItem("arrow", "arrow");
        // create an arrow to move to the next level
        SaveFloorItems();
    }

    /// <summary>
    /// Spawn the items for which the player can trade with.
    /// </summary>
    public void SpawnTraderItems() {
        int tempOffset = deletionQueue.Count;
        // get the count now so we can spawn items without fear of it changing
        lootText.text = "goods:";
        // set the test
        for (int i = 0; i < 3; i++) { CreateItem("common", tempOffset); }
        // create 3 common items, negativelyoffesting by the deletionq
        CreateItem("arrow", "arrow", tempOffset);
        // create the next level arrow
        SaveFloorItems();
    }
    
    /// <summary>
    /// Destroy floor items when going on to the next level or restarting.
    /// </summary>
    public void DestroyItems() {
        lootText.text = "";
        Select(scripts.player.inventory, 0, playAudio:false);
        foreach (GameObject toBeRemoved in floorItems) {
            Destroy(toBeRemoved);
            // destroy every floor item
        }
        floorItems.Clear(); 
        // clear the array
    }

    /// <summary>
    /// Returns true if the player has an item of the given name.
    /// </summary>
    public bool PlayerHas(string itemName) { return (from a in scripts.player.inventory select a.GetComponent<Item>().itemName).Contains(itemName); }

    /// <summary>
    /// Returns true if the player has a weapon of given type.
    /// </summary>
    public bool PlayerHasWeapon(string weaponName) {
        return (scripts.player.inventory[0].GetComponent<Item>().itemName.Split(' ')[1] == weaponName);
        // return (from a in scripts.player.inventory where a.GetComponent<Item>().itemName.Split(' ').Length > 1 select a.GetComponent<Item>().itemName.Split(' ')[1]).Contains(weaponName);
    }

    /// <summary>
    /// Gets the first item in the player's inventory with given name.
    /// </summary>
    public GameObject GetPlayerItem(string itemName) {
        try { return scripts.player.inventory[(from a in scripts.player.inventory select a.GetComponent<Item>().itemName).ToList().IndexOf(itemName)]; }
        catch { return null; }
    }

    /// <summary>
    /// Fade all torches that have ended their lifespan.
    /// </summary>
    public void AttemptFadeTorches() {
        for (int i = 0; i < scripts.player.inventory.Count; i++) {
            // for every item
            GameObject item = scripts.player.inventory[i];
            if (item.GetComponent<Item>().itemName == "torch") {
                // if the item is a torch
                if ($"{scripts.levelManager.level}-{scripts.levelManager.sub}" == item.GetComponent<Item>().modifier) {
                    // if the fade level matches the current level
                    scripts.turnManager.SetStatusText("your torch runs out");
                    // notify player
                    scripts.player.inventory[scripts.player.inventory.IndexOf(item)].GetComponent<Item>().Remove(torchFade:true);
                }
            }
        }
    }

    /// <summary>
    /// Save all inventory items onto the player's local Savefile.
    /// </summary>
    public void SaveInventoryItems() {
        if (scripts.levelManager != null) { 
            Save.game.resumeItemNames = new string[9];
            Save.game.resumeItemTypes = new string[9];
            Save.game.resumeItemMods = new string[9];
            // clear the data before placing in new
            Item item = scripts.player.inventory[0].GetComponent<Item>();
            Save.game.resumeItemNames[0] = item.itemName.Split(' ')[1];
            Save.game.resumeItemTypes[0] = item.itemType;
            Save.game.resumeItemMods[0] = item.modifier;
            // add the player's weapon first
            for (int i = 1; i < scripts.player.inventory.Count; i++) {
                item = scripts.player.inventory[i].GetComponent<Item>();
                Save.game.resumeItemNames[i] = item.itemName;
                Save.game.resumeItemTypes[i] = item.itemType;
                Save.game.resumeItemMods[i] = item.modifier;
                // add all the remaining items
            }
            if (scripts.tutorial is null) { Save.SaveGame(); }
            // Save to file

        }
    }

    /// <summary>
    /// Save all floor items onto the player's local Savefile.
    /// </summary>
    public void SaveFloorItems() { 
        Save.game.floorItemNames = new string[9];
        Save.game.floorItemTypes = new string[9];
        Save.game.floorItemMods = new string[9];
        // clear the data before placing in new 
        bool arrowFound = false;
        for (int i = 0; i < floorItems.Count; i++) {
            if (!arrowFound) { 
                Item item = floorItems[i].GetComponent<Item>();
                if (item.itemType == "weapon") { Save.game.floorItemNames[i] = item.itemName.Split(' ')[1]; }
                else { Save.game.floorItemNames[i] = item.itemName; }
                
                Save.game.floorItemTypes[i] = item.itemType;
                Save.game.floorItemMods[i] = item.modifier;
                if (Save.game.floorItemNames[i] == "arrow") { arrowFound = true; }
            }
            else {
                Save.game.floorItemNames[i] = "";
                Save.game.floorItemTypes[i] = "";
                Save.game.floorItemMods[i] = "";
            }
        }
        // funky workaround to make it so that the player doesn't get duplicate arrows
        if (scripts.tutorial is null) { Save.SaveGame(); }
    }

    // two separate methods so that we dont have to do any fancy checks, just pull whichever we need as long as we Save it properly

    /// <summary>
    /// Save all tombstone items onto the player's local Savefile.
    /// </summary>
    public void SaveTombstoneItems() { 
        Save.game.floorItemNames = new string[9];
        Save.game.floorItemTypes = new string[9];
        Save.game.floorItemMods = new string[9];
        // clear the data before placing in new 
        bool arrowFound = false;
        for (int i = 0; i < floorItems.Count; i++) {
            if (!arrowFound) { 
                Item item = floorItems[i].GetComponent<Item>();
                if (item.itemType == "weapon") { Save.game.floorItemNames[i] = item.itemName.Split(' ')[1]; }
                else { Save.game.floorItemNames[i] = item.itemName; }
                
                Save.game.floorItemTypes[i] = item.itemType;
                Save.game.floorItemMods[i] = item.modifier;
                if (Save.game.floorItemNames[i] == "arrow") { arrowFound = true; }
            }
            else {
                Save.game.floorItemNames[i] = "";
                Save.game.floorItemTypes[i] = "";
                Save.game.floorItemMods[i] = "";
            }
        }
        // funky workaround to make it so that the player doesn't get duplicate arrows
        if (scripts.tutorial is null) { Save.SaveGame(); }
    }
}
