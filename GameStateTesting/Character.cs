using System;
using Microsoft.Xna.Framework;

public class Character
{
    private string charName;
    private string charPronouns;
    private int[] charCustom;

    public Character(string name, string pronouns, int head, int face, int body)
	{
        charName = name;
        charPronouns = pronouns;
        charCustom = new int[] { head, face, body };
    }

    public void setCharName(string name)
    {
        charName = name;
    }
    public void setCharPronouns(string pronouns)
    {
        charPronouns = pronouns;
    }
    public void setCharCustom(int head, int face, int body)
    {
        charCustom = new int[] { head, face, body };
    }
    public string getCharName()
    {
        return charName;
    }
    public string getCharPronouns()
    {
        return charPronouns;
    }
    public int[] getCharCustom()
    {
        return charCustom;
    }

    public int getCharHead()
    {
        return charCustom[0];
    }
    public int getCharFace()
    {
        return charCustom[1];
    }
    public int getCharBody()
    {
        return charCustom[2];
    }
}