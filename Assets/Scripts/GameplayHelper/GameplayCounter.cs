using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayCounter
{
    private int monsterKilled;
    public int MonsterKilled
    { get { return this.monsterKilled; } }

    private int rubyCollected;
    public int RubyCollected
    { get { return this.rubyCollected; } }

    public static GameplayCounter getInstance()
    {
        return SingletonHelper.INSTANCE;
    }

    private static class SingletonHelper
    {
        public static readonly GameplayCounter INSTANCE = new GameplayCounter();
    }

    public void restartCounter()
    {
        monsterKilled = 0;
        rubyCollected = 0;
    }

    public void addRuby()
    {
        rubyCollected++;
    }

    public void addMonster() {
        monsterKilled++;
    }
}
