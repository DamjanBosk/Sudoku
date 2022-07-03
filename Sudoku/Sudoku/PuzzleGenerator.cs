using System;
using System.Collections.Generic;
using System.Text;

namespace Sudoku
{
    public class PuzzleGenerator
    {
        public int[,] SolvedGrid;
        public int[,] MaskedGrid;
        public const int GRID_SIZE = 9; // number of columns/rows.

        private int sqrOfGridSize; // square root of N
        private Random random = new Random();

        // Constructor
        public PuzzleGenerator()
        {
            // Compute square root of N
            double SRNd = Math.Sqrt(GRID_SIZE);
            sqrOfGridSize = (int)SRNd;

            SolvedGrid = new int[GRID_SIZE, GRID_SIZE];
            MaskedGrid = new int[GRID_SIZE, GRID_SIZE];
        }

        // Sudoku Generator
        public void generateGrids(int NumOfMaskedFields)
        {
            // Fill the diagonal of SqrOfSize x SqrOfSize matrices
            fillDiagonal();

            // Fill remaining blocks
            fillRemaining(0, sqrOfGridSize);

            GenerateMaskedGrid(NumOfMaskedFields);
        }

        // K is no. of digits to be masked
        public void GenerateMaskedGrid(int NumOfMaskedFields)
        {
            int count = NumOfMaskedFields;
            MaskedGrid = SolvedGrid.Clone() as int[,];
            while (count != 0)
            {
                int cellId = randomGenerator(GRID_SIZE * GRID_SIZE) - 1;

                int i = (cellId / GRID_SIZE);
                int j = cellId % 9;
                if (j != 0)
                    j = j - 1;

                if (MaskedGrid[i, j] != 0)
                {
                    count--;
                    MaskedGrid[i, j] = 0;
                }
            }
        }

        // Fill the diagonal SqrOfSize number of SqrOfSize x SqrOfSize matrices
        void fillDiagonal()
        {

            for (int i = 0; i < GRID_SIZE; i = i + sqrOfGridSize)

                // for diagonal box, start coordinates->i==j
                fillBlock(i, i);
        }

        // Fill a 3 x 3 matrix.
        void fillBlock(int row, int col)
        {
            int num;
            for (int i = 0; i < sqrOfGridSize; i++)
            {
                for (int j = 0; j < sqrOfGridSize; j++)
                {
                    do
                    {
                        num = randomGenerator(GRID_SIZE);
                    }
                    while (!unUsedInBlock(row, col, num));

                    SolvedGrid[row + i, col + j] = num;
                }
            }
        }

        // A recursive function to fill remaining matrix
        bool fillRemaining(int i, int j)
        {
            if (j >= GRID_SIZE && i < GRID_SIZE - 1)
            {
                i = i + 1;
                j = 0;
            }
            if (i >= GRID_SIZE && j >= GRID_SIZE)
                return true;

            if (i < sqrOfGridSize)
            {
                if (j < sqrOfGridSize)
                    j = sqrOfGridSize;
            }
            else if (i < GRID_SIZE - sqrOfGridSize)
            {
                if (j == (int)(i / sqrOfGridSize) * sqrOfGridSize)
                    j = j + sqrOfGridSize;
            }
            else
            {
                if (j == GRID_SIZE - sqrOfGridSize)
                {
                    i = i + 1;
                    j = 0;
                    if (i >= GRID_SIZE)
                        return true;
                }
            }

            for (int num = 1; num <= GRID_SIZE; num++)
            {
                if (CheckIfSafe(i, j, num))
                {
                    SolvedGrid[i, j] = num;
                    if (fillRemaining(i, j + 1))
                        return true;

                    SolvedGrid[i, j] = 0;
                }
            }
            return false;
        }

        // Random generator
        int randomGenerator(int num)
        {
            return (int)Math.Floor((double)(random.NextDouble() * num + 1));
        }

        // Check if safe to put in cell
        bool CheckIfSafe(int i, int j, int num)
        {
            return (unUsedInRow(i, num) &&
                    unUsedInCol(j, num) &&
                    unUsedInBlock(i - i % sqrOfGridSize, j - j % sqrOfGridSize, num));
        }

        // Returns false if given 3 x 3 block contains num.
        bool unUsedInBlock(int rowStart, int colStart, int num)
        {
            for (int i = 0; i < sqrOfGridSize; i++)
                for (int j = 0; j < sqrOfGridSize; j++)
                    if (SolvedGrid[rowStart + i, colStart + j] == num)
                        return false;

            return true;
        }

        // check in the row for existence
        bool unUsedInRow(int i, int num)
        {
            for (int j = 0; j < GRID_SIZE; j++)
                if (SolvedGrid[i, j] == num)
                    return false;
            return true;
        }

        // check in the row for existence
        bool unUsedInCol(int j, int num)
        {
            for (int i = 0; i < GRID_SIZE; i++)
                if (SolvedGrid[i, j] == num)
                    return false;
            return true;
        }
    }
}
