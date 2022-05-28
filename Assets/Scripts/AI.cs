using UnityEngine;

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
    public static int CalculateMove(Node node, int depth, int alpha, int beta, bool isMax)
    {
        var o = ConnectFour.IsGameOver(node.grid);
        if (depth == 0 || o != CellState.Empty)
        {
            var pColor = isMax ? CellState.Red : CellState.Blue;
            if (o != CellState.Empty)
                return o == pColor ? int.MaxValue : int.MinValue;
             
            node.evalValue = node.Evaluate(isMax);
            return node.Evaluate(isMax);
        }
        
        node.InitializeChildren(7, isMax);

        var i = 0;
        if (isMax)
        {
            var maxEval = int.MinValue;
            foreach (var child in node.children)
            {
                if (ConnectFour.IsColumnFull(child.grid, i))
                {
                    i++;
                    continue;
                }
                
                var eval = CalculateMove(child, depth -1, alpha, beta, false);

                maxEval = Mathf.Max(maxEval, eval);
                alpha = Mathf.Max(alpha, eval);
                if (beta <= alpha)
                    break;

                i++;
            }

            node.evalValue = maxEval;
            return maxEval;
        }

        i = 0;
        var minEval = int.MaxValue;
        foreach (var child in node.children)
        {
            if (ConnectFour.IsColumnFull(child.grid, i))
            {
                i++;
                continue;
            }

            var eval = CalculateMove(child, depth -1, alpha, beta, true);
            
            minEval = Mathf.Min(minEval, eval);
            beta = Mathf.Min(minEval, eval);
            if (beta <= alpha)
                break;
            
            i++;
        }
        
        node.evalValue = minEval;
        return minEval;
    }
}
