using System;

public class MockCharCreationUI
{
    public int[] inputSeries;

    public int focusArea = 0; // corresponds to the "section" of the UI screen that is in focus

    public int headArea = 0; // scrolls through the options for the head (0-3); only active in focusArea 0
    public int faceArea = 0; // scrolls through the options for the head (0-3); only active in focusArea 1
    public int bodyArea = 0; // scrolls through the options for the head (0-3); only active in focusArea 2

    public bool continueFlag = false; // the user can continue when flag is set to true, at focusArea > 2

    public MockCharCreationUI(int[] inputs)
	{
        // inputs are an array of ints, from 1-4, which corresponds to a directional key press
        // 1: up
        // 2: down
        // 3: left
        // 4: right

        inputSeries = inputs;
	}

    public void uiNavigation()
    {
        foreach (int i in inputSeries) 
        {
            continueFlag = false;

            // changes focus area to previous when up is pressed
            if (inputSeries[i] == 1) 
            {
                if (focusArea > 0)
                {
                    focusArea--;
                }
            }
            // changes focus area to next when down is pressed
            if (inputSeries[i] == 2)
            {
                if (focusArea < 5)
                {
                    focusArea++;
                }
            }
            // controls left cycle for each body part
            if (inputSeries[i] == 3)
            {
                switch (focusArea)
                {
                    case 0:
                        headArea--;
                        if (headArea < 0)
                        {
                            headArea = 3;
                        }
                        break;
                    case 1:
                        faceArea--;
                        if (faceArea < 0)
                        {
                            faceArea = 3;
                        }
                        break;
                    case 2:
                        bodyArea--;
                        if (bodyArea < 0)
                        {
                            bodyArea = 3;
                        }
                        break;
                    default:
                        break;
                }
            }
            // controls right cycle for each body part
            if (inputSeries[i] == 4)
            {
                switch (focusArea)
                {
                    case 0:
                        headArea++;
                        if (headArea > 3)
                        {
                            headArea = 0;
                        }
                        break;
                    case 1:
                        faceArea++;
                        if (faceArea > 3)
                        {
                            faceArea = 0;
                        }
                        break;
                    case 2:
                        bodyArea++;
                        if (bodyArea > 3)
                        {
                            bodyArea = 0;
                        }
                        break;
                    default:
                        break;
                }
            }
            // move to the next story screen
            if (focusArea > 2)
            {
                continueFlag = true;
            }
        }
    }
    public bool canContinue()
    {
        uiNavigation();
        return continueFlag;
    }

    public int getHead()
    {
        uiNavigation();
        return headArea;
    }
    public int getFace()
    {
        uiNavigation();
        return faceArea;
    }
    public int getBody()
    {
        uiNavigation();
        return bodyArea;
    }
}
