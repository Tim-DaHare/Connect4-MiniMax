using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public CellState[,] grid;
    public List<Node> children;
    public Node parent;

    public int evalValue;

    public int Evaluate()
    {
        return 1;
    }

    public Node(CellState[,] cGrid, Node cParent = null)
    {
        grid = cGrid.Clone() as CellState[,];
        parent = cParent;
    }

    public void InitializeChildren(int childCount, int gridWidth, int gridHeight)
    {
        for (int i = 0; i < childCount; i++)
        {
            var g = new CellState[gridWidth, gridHeight];
            children.Add(new Node(g, this));
        }
    }
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
        if (depth == 0 || ConnectFour.IsGameOver(node.grid))
            return node.Evaluate();

        if (isMax)
        {
            var maxEval = int.MinValue;
            foreach (var child in node.children)
            {
                var eval = MiniMax(child, depth -1, alpha, beta, false);
                maxEval = Mathf.Max(maxEval, eval);
                alpha = Mathf.Max(alpha, eval);
                if (beta <= alpha)
                    break;
            }
            return maxEval;
        }

        var minEval = int.MaxValue;
        foreach (var child in node.children)
        {
            var eval = MiniMax(child, depth -1, alpha, beta, true);
            minEval = Mathf.Min(minEval, eval);
            beta = Mathf.Min(minEval, eval);
            if (beta <= alpha)
                break;
        }
        return minEval;
    }
}
