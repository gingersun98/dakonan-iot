using System;

[Serializable]
public class BallAheadResult
{
    public int amountOfBall;
    public bool isOnPlayerSide;

    // NEW (important for AI)
    public bool landedOnBase;
    public bool wasEmptyLanding;
    public bool canCapture;
    public bool causesRelay;
}
