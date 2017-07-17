using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flood_Task
{
    class Program
    {
        static List<string> FileExist(string path)
        {
            List<string> input = new List<string>();
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        input.Add(line);
                    }
                }
            }
            else
            {
                Console.WriteLine("File isnot exist");
                Console.ReadLine();
                Environment.Exit(0);
            }
            return input;
        }
        static void DisplayPlane(Cell[,] plane, int maxValueX, int maxValueY)
        {
            for(int i=0; i<maxValueX; i++)
            {
                for (int j = 0; j < maxValueY; j++)
                {
                    switch (plane[i, j].CellState)
                    {
                        case State.Air:
                            Console.Write("  ");
                            break;
                        case State.Water:
                            Console.Write("~ ");
                            break;
                        case State.Border:
                            Console.Write("* ");
                            break;
                    }
                }
                Console.WriteLine();
            }
        }

        static List<Wall> ReadFromFile(out int maxValueX, out int maxValueY, out int wallsNumber)
        {
            int waterBorder = 2;
            int pointsNumber;
            string[] tempArr = new string[2];
            string path = "TestFile.txt";
            List<string> input = FileExist(path);

            pointsNumber = Convert.ToInt32(input[0]);
            Point[] points = new Point[pointsNumber];
            int[] arrXCoord = new int[pointsNumber];
            int[] arrYCoord = new int[pointsNumber];
            for (int i = 1; i <= pointsNumber; i++)
            {
                points[i - 1] = new Point();
                tempArr = input[i].Split(' ');
                arrXCoord[i - 1] = points[i - 1].X = Convert.ToInt32(tempArr[0]);
                arrYCoord[i - 1] = points[i - 1].Y = Convert.ToInt32(tempArr[1]);
            }

            wallsNumber = Convert.ToInt32(input[pointsNumber + 1]);
            List<Wall> walls = new List<Wall>();
            for(int i = pointsNumber + 2; i <= pointsNumber + wallsNumber + 1; i++)
            {
                tempArr = input[i].Split(' ');
                walls.Add(new Wall(points[Convert.ToInt32(tempArr[0]) - 1], points[Convert.ToInt32(tempArr[1]) - 1]));
            }
            maxValueX = arrXCoord.OrderByDescending(x => x).First() + waterBorder;
            maxValueY = arrYCoord.OrderByDescending(x => x).First() + waterBorder;
            return walls;
        }

        static Cell[,] CreatePlane(int maxValueX, int maxValueY)
        {
            Cell[,] plane = new Cell[maxValueX, maxValueY];
            for (int i = 0; i < maxValueX; i++)
            {
                for (int j = 0; j < maxValueY; j++)
                {
                    plane[i, j] = new Cell();
                }
            }
            return plane;
        }

       static void CreateWalls(Cell[,] plane, int wallsNumber, List<Wall> walls)
       {
            for (int i = 0; i < wallsNumber; i++)
            {
                if (walls[i].IsHorizontal())
                {
                    if (walls[i].FirstPoint.Y > walls[i].SecondPoint.Y)
                    {
                        walls[i].Swap();
                    }

                    for (int j = walls[i].FirstPoint.Y; j <= walls[i].SecondPoint.Y; j++)
                    {
                        plane[walls[i].FirstPoint.X, j].CellState = State.Border;
                    }
                }
                if (walls[i].IsVertical())
                {
                    if (walls[i].FirstPoint.X > walls[i].SecondPoint.X)
                    {
                        walls[i].Swap();
                    }

                    for (int j = walls[i].FirstPoint.X; j <= walls[i].SecondPoint.X; j++)
                    {
                        plane[j, walls[i].FirstPoint.Y].CellState = State.Border;
                    }
                }
            }
        }
        
        static void FloodInitialization(Cell[,] plane, int maxValueX, int maxValueY)
        {
            for (int i = 0; i < maxValueX; i++)
                for (int j = 0; j < maxValueY; j++)
                {
                    if ((j == 0) || (i == 0) || (j == maxValueY - 1) || (i == maxValueX - 1))
                    {
                        plane[i, j].CellState = State.Water;
                    }
                }
        }

        static bool SearchForAir(Cell[,] plane, int maxValueX, int maxValueY)
        {
            for(int i=0; i<maxValueX;i++)
                for(int j=0; j < maxValueY; j++)
                {
                    if (plane[i, j].CellState == State.Air)
                        return true;
                }
            return false;
        }

        static void DisplayResult(Cell[,] plane, List<Wall> walls, int wallsNumber)
        {
            int unBrokenWallCount = 0;
            List<int> unBrokenWallIndex = new List<int>();
            for (int i = 0; i < wallsNumber; i++)
            {
                bool unbroken = true;
                if (walls[i].IsHorizontal())
                {
                    for (int j = walls[i].FirstPoint.Y + 1; j < walls[i].SecondPoint.Y; j++)
                    {
                        if (plane[walls[i].FirstPoint.X, j].CellState == State.Border)
                            continue;
                        else
                        {
                            unbroken = false;
                            break;
                        }
                    }
                }
                if (walls[i].IsVertical())
                {
                    for (int j = walls[i].FirstPoint.X + 1; j < walls[i].SecondPoint.X; j++)
                    {
                        if (plane[j, walls[i].FirstPoint.Y].CellState == State.Border)
                            continue;
                        else
                        {
                            unbroken = false;
                            break;
                        }
                    }
                }
                if (unbroken)
                {
                    unBrokenWallCount += 1;
                    unBrokenWallIndex.Add(i);
                }
            }
            Console.WriteLine(unBrokenWallCount);
            foreach (int index in unBrokenWallIndex)
                Console.WriteLine("{0}", index+1);
        }

        static Cell[,] PlaneAlignment(Cell[,] plane, List<Wall> walls, int wallsNumber, ref int maxValueX, ref int maxValueY)
        {
            for (int i = 0; i < wallsNumber; i++)
            {
                if (walls[i].IsHorizontal())
                {
                    for (int j = walls[i].FirstPoint.Y + 1; j < walls[i].SecondPoint.Y; j++)
                    {
                        if (plane[walls[i].FirstPoint.X + 1, j].CellState == State.Border)
                        {
                            for (int k = 0; k < wallsNumber; k++)
                            {
                                if (walls[k].FirstPoint.X > walls[i].FirstPoint.X)
                                {
                                    walls[k].MoveFirstPointTop();
                                }

                                if (walls[k].SecondPoint.X > walls[i].FirstPoint.X)
                                {
                                    walls[k].MoveSecondPointTop();
                                }
                            }
                            maxValueX += 1;
                            plane = CreatePlane(maxValueX, maxValueY);
                            CreateWalls(plane, wallsNumber, walls);
                            break;
                        }
                    }
                }
                if (walls[i].IsVertical())
                {
                    for (int j = walls[i].FirstPoint.X + 1; j < walls[i].SecondPoint.X; j++)
                    {
                        if (plane[j, walls[i].FirstPoint.Y + 1].CellState == State.Border)
                        {
                            for (int k = 0; k < wallsNumber; k++)
                            {
                                if (walls[k].FirstPoint.Y > walls[i].FirstPoint.Y)
                                {
                                    walls[k].MoveFirstPointRight();
                                }
                                if (walls[k].SecondPoint.Y > walls[i].FirstPoint.Y)
                                {
                                    walls[k].MoveSecondPointRight();
                                }
                            }
                            maxValueY += 1;
                            plane = CreatePlane(maxValueX, maxValueY);
                            CreateWalls(plane, wallsNumber, walls);
                            break;
                        }
                    }
                }
            }
            return plane;
        }

        static void FloodAir(Cell[,] plane, List<Wall> walls, int wallsNumber)
        {
            for (int i = 0; i < wallsNumber; i++)
            {
                if (walls[i].IsHorizontal())
                {
                    for (int j = walls[i].FirstPoint.Y + 1; j < walls[i].SecondPoint.Y; j++)
                    {
                        if (plane[walls[i].FirstPoint.X, j].CellState == State.Water)
                        {
                            int k = walls[i].FirstPoint.X;
                            while (plane[k + 1, j].CellState == State.Air)
                            {
                                plane[k + 1, j].CellState = State.Water;
                                k++;
                            }
                            k = walls[i].FirstPoint.X;
                            while (plane[k - 1, j].CellState == State.Air)
                            {
                                plane[k - 1, j].CellState = State.Water;
                                k--;
                            }
                        }
                    }
                }
            }
        }

        static void FloodWall(Cell[,] plane, Wall wall, int wallsNumber)
        {
            if (wall.IsHorizontal())
            {
                for (int j = wall.FirstPoint.Y + 1; j < wall.SecondPoint.Y; j++)
                {
                    if (plane[wall.FirstPoint.X + 1, j].CellState == State.Air && plane[wall.FirstPoint.X - 1, j].CellState == State.Water)
                        plane[wall.FirstPoint.X, j].CellState = State.Water;
                    if (plane[wall.FirstPoint.X - 1, j].CellState == State.Air && plane[wall.FirstPoint.X + 1, j].CellState == State.Water)
                        plane[wall.FirstPoint.X, j].CellState = State.Water;
                }
            }
            if (wall.IsVertical())
            {
                for (int j = wall.FirstPoint.X + 1; j < wall.SecondPoint.X; j++)
                {
                    if (plane[j, wall.FirstPoint.Y + 1].CellState == State.Air && plane[j, wall.FirstPoint.Y - 1].CellState == State.Water)
                        plane[j, wall.FirstPoint.Y].CellState = State.Water;
                    if (plane[j, wall.FirstPoint.Y - 1].CellState == State.Air && plane[j, wall.FirstPoint.Y + 1].CellState == State.Water)
                        plane[j, wall.FirstPoint.Y].CellState = State.Water;
                }
            }
        }

        static void Flood(Cell[,] plane, List<Wall> walls, int wallsNumber, int maxValueX, int maxValueY)
        {
            while (SearchForAir(plane, maxValueX, maxValueY))
            {
                for (int i = 0; i < wallsNumber; i++)
                {
                    FloodWall(plane, walls[i], wallsNumber);
                }
                FloodAir(plane, walls, wallsNumber);
                Console.WriteLine();
                DisplayPlane(plane, maxValueX, maxValueY);
            }
        }

        static void Main(string[] args)
        {
            int wallsNumber;
            int maxValueX;
            int maxValueY;
            List<Wall> walls = ReadFromFile(out maxValueX, out maxValueY, out wallsNumber);
            Cell[,] plane = CreatePlane(maxValueY, maxValueY);
            CreateWalls(plane, wallsNumber, walls);
            plane = PlaneAlignment(plane, walls, wallsNumber, ref maxValueX, ref maxValueY);
            FloodInitialization(plane, maxValueX, maxValueY);
            DisplayPlane(plane, maxValueX, maxValueY);
            Flood(plane, walls, wallsNumber, maxValueX, maxValueY);
            DisplayResult(plane, walls, wallsNumber);
            Console.ReadLine();
        }
    }
}
