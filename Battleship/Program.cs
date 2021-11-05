using System;
using System.Linq;


namespace Battleship
{
    class HumanPlayer
    {
        char[,] RadarGrid = new char[10, 10]; //Initialize the RadarGrid array
        public int Hits = 0; //Keep track of the hits
        //The x and y variables are nullable ints so they can be set to null if the user gives an invalid input
        //If they are not nullable then they will get a default value of 0 when the user gives an invalid input
        int? x = 0;
        int? y = 0;
        public Random randomCoordinate = new Random();
        //The following integers are used to store the x and y coordinate values of the battleship, 
        //the first destroyer, and the second destroyer respectively
        public int battleship_X;
        public int battleship_Y;
        public int destroyer1_X;
        public int destroyer1_Y;
        public int destroyer2_X;
        public int destroyer2_Y;
        //The EnemyPositionArray is a matrix to store the coordinates of the enemy ships
        //The default value is false, and will be set to true if a ship is placed there
        bool[,] EnemyPositionArray = new bool[10, 10];
        //This function keeps track of the number of hits

        //This function prompts the user to pick an x-coordinate
        public void choose_X_coordinate()
        {
            int xInput;
            Console.WriteLine("Enter an X coordinate (1-10): ");
            string userInput_X = Console.ReadLine();

            if (int.TryParse(userInput_X, out xInput))
            {
                x = xInput - 1; // -1 because the grid uses 0-9 but the user uses 1-10
            }
            else
            {
                Console.WriteLine("Invalid coordinate.");
                x = null;
                choose_X_coordinate(); //Restart the process if the user gave invalid input
            }
        }
        //This function prompts the user to pick a y-coordinate
        public void choose_Y_coordinate()
        {
            char yInput;
            char[] rowLetter = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
            Console.WriteLine("Enter a Y coordinate (A-J): ");
            string userInput_Y = Console.ReadLine().ToUpper();

            if (char.TryParse(userInput_Y, out yInput))
            {
                if (rowLetter.Contains(yInput))
                {
                    y = Array.IndexOf(rowLetter, yInput);
                }
            }
            else
            {
                Console.WriteLine("Invalid coordinate");
                y = null;
                choose_Y_coordinate(); //Restart the process if the user gives invalid input
            }
            //Error catching to try to set hits and misses on the grid accordingly, and throw an error for invalid input
            try
            {
                if (EnemyPositionArray[(int)x, (int)y].Equals(true))
                {
                    RadarGrid[(int)x, (int)y] = 'H';
                    Console.Clear();
                    Console.WriteLine("     HIT\n");
                    Hits += 1;
                }
                else
                {
                    RadarGrid[(int)x, (int)y] = 'M';
                    Console.Clear();
                    Console.WriteLine("     MISS\n");
                }
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("     Error: invalid coordinates");
            }
        }
        //This function is used to place the battleship on the grid
        public void setEnemyBattleship()
        {
            //Place battleship
            battleship_X = randomCoordinate.Next(4, 10);
            battleship_Y = randomCoordinate.Next(0, 10);

            //Ships will be placed horizontally to keep things simple
            PlaceShip(battleship_X, battleship_Y);
            PlaceShip(battleship_X - 1, battleship_Y);
            PlaceShip(battleship_X - 2, battleship_Y);
            PlaceShip(battleship_X - 3, battleship_Y);
            PlaceShip(battleship_X - 4, battleship_Y);
        }
        //This function places the first destroyer on the grid
        public void setEnemyDestroyer_1()
        {
            destroyer1_X = randomCoordinate.Next(0, 7);
            destroyer1_Y = randomCoordinate.Next(0, 9);
            //Keep reseting the function if the ships overlap until they don't overlap
            if (destroyer1_Y == battleship_Y)
            {
                setEnemyDestroyer_1(); //Restart the random shuffle of coordinates if they overlapped with another ship
            }
            else
            {
                PlaceShip(destroyer1_X, destroyer1_Y);
                PlaceShip(destroyer1_X + 1, destroyer1_Y);
                PlaceShip(destroyer1_X + 2, destroyer1_Y);
                PlaceShip(destroyer1_X + 3, destroyer1_Y);
            }
        }

        //This function places the second destroyer on the grid
        public void setEnemyDestroyer_2()
        {
            //Place second destroyer 
            destroyer2_X = randomCoordinate.Next(0, 7);
            destroyer2_Y = randomCoordinate.Next(0, 9);

            //Check Y-coordinates to make sure the ships don't overlap
            if (destroyer2_Y == battleship_Y || destroyer2_Y == destroyer1_Y)
            {
                setEnemyDestroyer_2(); //Restart random shuffle of coordinates if they overlapped with another ship
            }
            else
            {
                PlaceShip(destroyer2_X, destroyer2_Y);
                PlaceShip(destroyer2_X + 1, destroyer2_Y);
                PlaceShip(destroyer2_X + 2, destroyer2_Y);
                PlaceShip(destroyer2_X + 3, destroyer2_Y);
            }
        }
        //This function uses user-input coordinates to place the ships on the grid
        public void PlaceShip(int enemyX, int enemyY)
        {
            EnemyPositionArray[enemyX, enemyY] = true;
        }
        //This function is used to check if a ship has been hit at a given location or not
        public bool[,] GetEnemyGrid()
        {
            return EnemyPositionArray;
        }
        //This function returns the number of hits made
        public int returnHits()
        {
            return Hits;
        }
        //This function is used to update the grid
        public char[,] GetGrid()
        {
            return RadarGrid;
        }
    }

    //This class is used to create the grid and keep it updated
    class RadarBoard
    {
        //This function initializes the grid with +'s to make the coordinates easier to see.
        public void SetTargetingGrid(char[,] RadarDisplay)
        {
            //These 2 lines will keep the spacing above the grid consistent so it doesn't change when messages are displayed
            Console.WriteLine("\n");
            //For loop to initialize the grid coordinates with the value of '+'
            for (int x = 0; x <= 9; x++)
            {
                for (int y = 0; y <= 9; y++)
                {
                    RadarDisplay[x, y] = '+';
                }
            }
        }

        //This function will be used repeatedly to display the updated targeting grid during gameplay
        public void TargetingGrid(char[,] RadarDisplay)
        {
            char[] rowLetter = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
            Console.WriteLine("  | 1 2 3 4 5 6 7 8 9 10");
            Console.WriteLine("________________________");
            //This switch statement will give the proper labels for the rest of the y-axis label area
            for (int x = 0; x <= 9; x++)
            {
                Console.Write(rowLetter[x] + " | ");
                for (int y = 0; y <= 9; y++)
                {
                    Console.Write(RadarDisplay[y, x] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            HumanPlayer human = new HumanPlayer();
            RadarBoard rb = new RadarBoard();
            human.setEnemyBattleship();
            human.setEnemyDestroyer_1();
            human.setEnemyDestroyer_2();
            rb.SetTargetingGrid(human.GetGrid());
            //While there is still at least one ship afloat keep the game going
            while (human.returnHits() < 13)
            {
                rb.TargetingGrid(human.GetGrid());
                human.GetEnemyGrid();
                human.choose_X_coordinate();
                human.choose_Y_coordinate();
            }
            //It takes 13 hits to sink all the ships. After 13 hits display the following end-of-game message:
            Console.WriteLine("Good show. You sank all the enemy ships. The game is now over.\n");
            Console.WriteLine("Enter any key to exit.");
            Console.ReadKey();
        }
    }
}
