

int[] queens = new int[] { 0, 5, 2, 1, 5, 2, 3, 7 };

const int nQueensSize = 8;

int iterations = 0;
int deadEnds = 0;
int states = 0;
int statesInMemory = 1;

int[,] SetBoard(int[] queensPos)
{
    int[,] emptyBoard = new int[8,8] 
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

    for(int i = 0; i < 8; i++)
    {
        emptyBoard[queensPos[i], i] = 1;
    }

    return emptyBoard;
}

int[] newQueens = queens.Take(8).ToArray();
bool solved = false;


void WriteScreen(int[,] board)
{
    for (int i = 0; i < 8; i++)
    {
        for (int j = 0; j < 8; j++)
            Console.Write(board[i, j] + " ");
        Console.Write("\n");
    }

}

void IDS()
{
    int totalDepth = 8;
    for(int limit = 1; limit < totalDepth + 1; limit++)
    {
        if (solved) return;
        newQueens = queens.Take(8).ToArray();
        DFSRecursive(limit);
    }
}

void DFSRecursive(int limit = nQueensSize, int depth = 0)
{
    if(depth == limit || solved)
    {
        if (depth == limit) deadEnds++;
        if(!solved)
            if (CheckOnGoalBoard(newQueens))
                solved = true;
        return;
    }
    
    for (int i = 0; i < nQueensSize; i++)
    {
        states++;
        iterations++;
        if (solved) return;
        newQueens[depth] = (newQueens[depth] + i) % nQueensSize;
        DFSRecursive(limit, depth + 1);
        
    }
}


bool CheckOnGoalBoard(int[] queens)
{
    int k = 0;
    foreach(var el in queens)
    {
        for(int i = 0; i < nQueensSize; i++)
        {
            if (k == i) continue;               // перевірка на однаковий елемент

            int diff = Math.Abs(i - k);
            if (Math.Abs(queens[i] - el) == diff)  // перевірка по діагоналям
                return false;

            if (el == queens[i])        // перевірка по горизонталям
                return false;
        }
        k++;
    }
    return true;
}

Main();

void Main()
{
    Console.WriteLine("IDS algorithm");
    Console.WriteLine("initial board:");
    WriteScreen(SetBoard(queens));
    IDS();
    Console.WriteLine();
    Console.WriteLine("solved board:");
    WriteScreen(SetBoard(newQueens));
    Console.WriteLine($"iterations : {iterations}");
    Console.WriteLine($"deadEnds : {deadEnds}");
    Console.WriteLine($"states : {states}");
    Console.WriteLine($"states in memory : {newQueens.Length}");
    //AStar astar = new AStar();
    //astar.Main();
}
