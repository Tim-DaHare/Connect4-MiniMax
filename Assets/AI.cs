using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public CellState[,] _grid;
    public List<Node> _children;
}

public static class AI
{
    /// <summary>
    /// This is the recursive minimax algorithm, it also includes alpha beta pruning
    /// </summary>
    /// <param name="node"></param>
    /// <param name="depth"></param>
    /// <param name="alpha"></param>
    /// <param name="beta"></param>
    /// <param name="isMax"></param>
    /// <returns></returns>
    public static int MiniMax(Node node, int depth, int alpha, int beta, bool isMax)
    {
        if (depth == 0 || true) // add method which determines if the game is over 
            return 1; // replace 1 with value of usefulness

        if (isMax)
        {
            var maxEval = int.MinValue;
            foreach (var child in node._children)
            {
                var eval = MiniMax(child, depth -1, alpha, beta, false);
                maxEval = Mathf.Max(maxEval, eval);
                alpha = Mathf.Max(alpha, eval);
                if (beta <= alpha)
                    break;
                return maxEval;
            }
        }
        else
        {
            var minEval = int.MaxValue;
            foreach (var child in node._children)
            {
                var eval = MiniMax(child, depth -1, alpha, beta, true);
                minEval = Mathf.Min(minEval, eval);
                beta = Mathf.Min(minEval, eval);
                if (beta <= alpha)
                    break;
                return minEval;
            }
        }
    }
}
