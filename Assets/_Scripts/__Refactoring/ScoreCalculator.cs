public class ScoreCalculator
{
    public static int GetScore(Chip first, Chip second)
    {
        int score = (first.BoardPosition.y + 1) + (second.BoardPosition.y + 1);
    
        if (first.CompareShape(second) &&
            first.CompareColor(second))
        {
            score *= 2;
        }
    
        return score;
    }

    //public int GetScore(int boardLine) => (boardLine + 1) * width;
        
}