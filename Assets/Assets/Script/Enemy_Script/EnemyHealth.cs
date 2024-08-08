using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
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
            die();
    }
    //Getters & setters
    public int getCurrentHp() { return currentHp; }
    public int getMaxHp() { return maxHp; }
    public void setCurrentHp(int _hp) { currentHp = _hp;}
    public void setMaxHp(int _maxHp) { maxHp = _maxHp; }

    public void takeDamage(int dmg)
    {
        currentHp -= dmg;
        if (currentHp <= 0) die();
    }

    private void die()
    {
        Destroy(gameObject);
    }
}
