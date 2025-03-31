using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TheInventory : MonoBehaviour
{
    //LinkedList<item> inventory = new LinkedList<item>();
    public item[] equip = new item[3];
    public item currentWeapon;
    public RandallMovement Randall;
    public GameObject FullInventory;

    public class Node
    {
        public item data;
        public Node Next;

        public Node(item newitem)
        {
            data = newitem;
            Next = null;
        }
    }

    public Node inventory;
    // Start is called before the first frame update
    void Start()
    {
        /*
        SAVE FILE ORGANIZATION:
        FIRST LINE IS THE STATS: LVL/VIT/MNA/EGY/STR/AGI/INT/DEF/LUK/xp
        SECOND LINE IS THE CURRENT CHECKPOINT
        THIRD LINE IS THE NAME OF CURRENT WEAPON AND QUICK SLOTS HERE! WEAPON/SLOT1/SLOT2/SLOT3
        THE NEXT LINES BELOW ARE INVENTORY SLOTS, SAVED AS: itemtype/name/description/damage/defense/hp/type/element/inscriptionamount
        IF INSCRIPTION AMOUNT IS MORE THAN 0, THE LINES BELOW WILL SAVE INSCRIPTIONS WITH FORMAT OF name/description/damage/defense/hp/specialEffect/element
        */

        string filePath = Path.Combine(Application.persistentDataPath, "MSSsave.txt");

        using (StreamReader saveFile = new StreamReader(filePath))
        {
            saveFile.ReadLine();
            saveFile.ReadLine();
            string[] currentequips = saveFile.ReadLine().Split(':');
            string current = saveFile.ReadLine();
            inventory = null;
            while (current != null)
            {
                string[] temp = current.Split(':');
                item tempitem = new item(temp[0], temp[1], temp[2], int.Parse(temp[3]), int.Parse(temp[4]), int.Parse(temp[5]), temp[6], temp[7], new inscription[int.Parse(temp[8])]);
                if (tempitem.inscriptions.Length != 0)
                {
                    for (int i = 0; i < tempitem.inscriptions.Length; i++)
                    {
                        current = saveFile.ReadLine();
                        if (current == "Empty")
                        {

                        }
                        else
                        {
                            string[] tempinscription = current.Split(":");
                            tempitem.addInscription(tempinscription[0], tempinscription[1], int.Parse(tempinscription[2]), int.Parse(tempinscription[3]), int.Parse(tempinscription[4]), tempinscription[5], tempinscription[6], i);
                        }
                    }
                }

                inventory = addItemTop(inventory, tempitem);

                if (tempitem.name == currentequips[0])
                {
                    currentWeapon = tempitem;
                }
                else if (tempitem.name == currentequips[1])
                {
                    equip[0] = tempitem;
                }
                else if (tempitem.name == currentequips[2])
                {
                    equip[1] = tempitem;
                }
                else if (tempitem.name == currentequips[3])
                {
                    equip[2] = tempitem;
                }

                current = saveFile.ReadLine();
            }

            saveFile.Close();
        }

        Node tempNode = inventory;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addNewItem(item newItem)
    {
        if (getSize(inventory) < 12)
        {
            inventory = addItemTop(inventory, newItem);
        }
        else
        {
            FullInventory.SetActive(true);
            Invoke("hideInventoryMessage", 1);
        }
    }

    public void hideInventoryMessage()
    {
        FullInventory.SetActive(false);
    }

    public void weaponUse()
    {
        if (currentWeapon.itemType == "sword")
        {

        }
    }

    public void deleteItem(ref Node temp, string search)
    {
        if (temp != null && temp.data.name == search)
        {
            temp = temp.Next;
            return;
        }

        Node p = temp;
        while (p != null && p.Next != null)
        {
            if (p.Next.data.name == search)
            {
                p.Next = p.Next.Next;
                return;
            }
            p = p.Next; 
        }
    }

    public Node addItemTop(Node head, item newData)
    {
        Node newNode = new Node(newData);
        newNode.Next = head;
        return newNode;
    }
    public Node addItemEnd(Node head, item newData)
    {
        Node tempNode = new Node(newData);
        if (head == null)
        {
            return tempNode;
        }

        Node last = head;

        while (last.Next != null)
        {
            last = last.Next;
        }

        last.Next = tempNode;

        return head;
    }

    public int getSize(Node temp)
    {
        int result = 0;
        while (temp != null)
        {
            result++;
            temp = temp.Next;
        }
        return result;
    }

    public item getCurrentWeapon()
    {
        return currentWeapon;
    }

    public item[] getQuickItems()
    {
        return equip;
    }

    public void setQuickItem(Node temp, string search, int slot)
    {
        Node p = temp;
        while (p != null)
        {
            if (p.data.name == search)
            {
                equip[slot] = p.data;
                return;
            }
            p = p.Next;
        }
    }

    public void removeQuickItem(int slot)
    {
        equip[slot] = null;
    }

    public item[] deposit(Node temp)
    {
        int size = getSize(temp);
        item[] result = new item[size];
        int i = 0;

        while (temp != null)
        {
            result[i] = temp.data;
            temp = temp.Next;
            i++;
        }

        return result;
    }
}

public class item
{
    public string itemType;
    public string name;
    public string description;
    //if itemType is not weapon, damage will be considered the variable for everything else (ex: healing) and every other value will be 0 as it does not get used.
    public int damage;
    public int defense;
    public int hp; //HP as the player sees is done via hearts. 1/4 heart = 1hp, 1 heart = 4hp.
    //type is beautiful, cute, or quirky
    public string type;
    public string element;
    public inscription[] inscriptions;

    public item(string it, string n, string d, int da, int de, int h, string t, string e, inscription[] i)
    {
        itemType = it;
        name = n;
        description = d;
        damage = da;
        defense = de;
        hp = h;
        type = t;
        element = e;
        inscriptions = i;
    }

    public item()
    {
        itemType = "sword";
        name = "Fae'r";
        description = "A sword forged by Randall himself. It's spirit has been awakened by him as well.";
        damage = 3;
        defense = 3;
        hp = 4;
        type = "Beautiful";
        element = "Light";
        inscriptions = new inscription[2];
    }

    public void updateStats()
    {
        for (int i = 0; i < inscriptions.Length;  i++)
        {
            damage += inscriptions[i].damage;
            defense += inscriptions[i].defense;
            hp += inscriptions[i].hp;
        }
    }

    public void removeStats()
    {
        for (int i = 0; i < inscriptions.Length; i++)
        {
            damage -= inscriptions[i].damage;
            defense -= inscriptions[i].defense;
            hp -= inscriptions[i].hp;
        }
    }

    public void addInscription(string n, string d, int da, int de, int h, string se, string e, int slot)
    {
        inscription newInscription = new inscription(n, d, da, de, h, se, e);
        inscriptions[slot] = newInscription;
    }

    public int checkEmptyInscription()
    {
        for (int i = 0; i < inscriptions.Length; i++)
        {
            if (inscriptions[i] == null)
            {
                return i;
            }
        }
        return -1;
    }
}

public class inscription
{
    public string name;
    public string description;
    //below stats are to determine what the inscription adds. Special effect will be special things like "this weapon explodes when it is swung" or something
    public int damage;
    public int defense;
    public int hp;
    public string specialEffect;
    public string element; //element will be "None" if there is no element the inscription adds. Otherwise, it will be the correct element.

    public inscription(string n, string d, int da, int de, int h, string se, string e)
    {
        name = n;
        description = d;
        damage = da;
        defense = de;
        hp = h;
        specialEffect = se;
        element = e;
    }

    public inscription()
    {
        name = "Basic Inscription";
        description = "An inscription that adds 1 damage";
        damage = 1;
        defense = 0;
        hp = 0;
        specialEffect = "No Special Effect";
        element = "None";
    }
}
