
class AStar
{
    const int nQueensSize = 8;
    static int[] queens = new int[] { 1, 7, 7, 3, 3, 3, 5, 5 };

    int iterations = 0;
    int deadEnds = 0;
    int states = 0;

    int[,] SetBoard(int[] queensPos)
    {
        int[,] emptyBoard = new int[nQueensSize, nQueensSize]
        {
        { 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0 }
        };

        for (int i = 0; i < nQueensSize; i++)
        {
            emptyBoard[queensPos[i], i] = 1;
        }

        return emptyBoard;
    }


    void WriteScreen(int[,] board)
    {
        for (int i = 0; i < nQueensSize; i++)
        {
            for (int j = 0; j < nQueensSize; j++)
                Console.Write(board[i, j] + " ");
            Console.Write("\n");
        }

    }
    

    (int[], int) AStarFunc(int[] initialBoard)
    {
        PriorityQueue<(int, int, int[]), int> openQueue = new();      
        List<string> closed = new();
        (int, int, int[]) current;
        EnqueueInitialBoard(initialBoard, openQueue);                               // enque chilren of 0 depth
        while(openQueue.Count != 0)
        {
            current = openQueue.Dequeue();                                          // extracting min element
            if (FindAttackPairs(current.Item3) == 0)
                return (current.Item3, closed.Count + openQueue.Count + 1);          // checking if problem is solved
            closed.Add(String.Join("", current.Item3));
            if (GeneratePosterity(current) == null) continue;                         // checking if elements can generate children
            (int, int, int[])[] posterity = GeneratePosterity(current).Take(8).ToArray();
             foreach (var posterior in posterity)
            {
                iterations++;
                if (!closed.Contains(String.Join("", posterior.Item3)))
                {
                    openQueue.Enqueue(posterior, FindAttackPairs(posterior.Item3) + posterior.Item1); // enqueue children if they not already in queue
                    states++;
                }                                                                          
            }
        
        }
        return (null, 0);
    }

    void EnqueueInitialBoard(int[] initialBoard, PriorityQueue<(int, int, int[]), int> openQueue)
    {
        for (int i = 0; i < 8; i++)
        {
            states++;
            int[] boardState = initialBoard.Take(8).ToArray();
            boardState[0] = (initialBoard[0] + i) % nQueensSize;
            openQueue.Enqueue((0, (initialBoard[0] + i) % nQueensSize, boardState), FindAttackPairs(boardState));
        }
    }

    (int, int, int[])[] GeneratePosterity((int, int, int[]) current)
    {
        if (current.Item1 == 7) 
            return null;
        (int, int, int[])[] result = new (int, int, int[])[nQueensSize];
        for(int i= 0; i < 8; i++)
        {
            int[] boardState = current.Item3.Take(8).ToArray();
            boardState[current.Item1 + 1] = (current.Item2 + i) % nQueensSize;
            result[i] = (current.Item1 + 1, (current.Item2 + i) % nQueensSize, boardState);
        }
        return result;
    }

    int FindAttackPairs(int[] queens)
    {
        int attacked = 0;
        int k = 0;
        bool horizonDimension1=false, horizonDimension2=false, diagDimension1=false,
            diagDimension2=false, diagDimension3=false, diagDimension4=false;
        foreach (var el in queens)
        {
            for (int i = 0; i < nQueensSize; i++)
            {
                if (k == i) continue;               

                int diff = Math.Abs(i - k);
                if (Math.Abs(queens[i] - el) == diff && el - queens[i] > 0 && k - i > 0 && !diagDimension1)
                {
                    attacked++;
                    diagDimension1 = true;
                }

                else if (Math.Abs(queens[i] - el) == diff && el - queens[i] < 0 && k - i > 0 && !diagDimension2)
                {
                    attacked++;
                    diagDimension2 = true;
                }

                else if (Math.Abs(queens[i] - el) == diff && el - queens[i] > 0 && k - i < 0 && !diagDimension3)
                {
                    attacked++;
                    diagDimension3 = true;
                }

                else if (Math.Abs(queens[i] - el) == diff && el - queens[i] < 0 && k - i < 0 && !diagDimension4)
                {
                    attacked++;
                    diagDimension4 = true;
                }

                if (el == queens[i] &&  k - i > 0 && !horizonDimension1)
                {         
                    attacked++;
                    horizonDimension1 = true;
                }
                else if (el == queens[i] && k - i < 0 && !horizonDimension2)
                {
                    attacked++;
                    horizonDimension2 = true;
                }

            }
            horizonDimension1 = false;
            horizonDimension2 = false;
            diagDimension1 = false;
            diagDimension2 = false;
            diagDimension3 = false;
            diagDimension4 = false;
            k++;
        }
        return attacked / 2;
    }

    public void Main()
    {
        Console.WriteLine("A* algorithm");
        Console.WriteLine("initial board:");
        WriteScreen(SetBoard(queens));
        Console.WriteLine();
        Console.WriteLine("solved board:");
        (int[], int) result = AStarFunc(queens);
        WriteScreen(SetBoard(result.Item1));
        Console.WriteLine($"iterations : {iterations}");
        Console.WriteLine($"deadEnds : {deadEnds}");
        Console.WriteLine($"states : {states}");
        Console.WriteLine($"statesInMemory : {result.Item2}");
    }
}


