using System;

[System.Serializable]
public class ScoreData : IComparable<ScoreData>
{
    public int score;

    public int CompareTo(ScoreData other)
    {
        if (other == null)
        {
            return 1;
        }
        else if (this.score > other.score)
        {
            return 1;
        }
        else if (this.score < other.score)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}
