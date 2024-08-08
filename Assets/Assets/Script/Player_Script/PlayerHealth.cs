using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHp = 10;
    [SerializeField] private int currentHp;

    void Start()
    {
        currentHp = maxHp;
    }
    private void Update()
    {
        if (currentHp <= 0)
            gameOver();
    }
    //Getters & setters
    public int getCurrentHp() { return currentHp; }
    public int getMaxHp() { return maxHp; }
    public void setCurrentHp(int _hp) { currentHp = _hp; }
    public void setMaxHp(int _maxHp) { maxHp = _maxHp; }

    public void takeDamage(int dmg)
    {
        currentHp -= dmg;
        if (currentHp <= 0) gameOver();
    }

    private void gameOver()
    {
        //GameOver screen and shit...
    }
}
