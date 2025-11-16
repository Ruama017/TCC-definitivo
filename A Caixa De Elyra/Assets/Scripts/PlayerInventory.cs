using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int flowers = 0;
    public int crystals = 0;
    public int superPower = 0;

    public void Collect(Collectible item)
    {
        switch (item.collectibleType)
        {
            case Collectible.Type.ShieldFlower:
                flowers += item.amount;
                break;

            case Collectible.Type.LifeCrystal:
                crystals += item.amount;
                break;

            case Collectible.Type.SuperPower:
                superPower += item.amount;
                break;
        }
    }
}
