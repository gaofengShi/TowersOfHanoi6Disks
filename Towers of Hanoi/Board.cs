using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Towers_of_Hanoi
{
    class Board
    {
        const int pegStart = 122;
        const int pegGap = 180;
        const int deckHeight = 297;
        const int diskHeight = 18;

        Disk[,] board; //condition says TWO dimentional array            
        ArrayList movements;
        Disk[] disks; //Array of disks

        private const int NUM_DISKS = 6;
        private const int NUM_PEGS = 3;
        public int steps = 0;
        

        public Board()
        {
            board = new Disk[NUM_PEGS, NUM_DISKS];
            movements = new ArrayList();

            //Array of disk objects
            disks = new Disk[NUM_DISKS];
            disks[0] = null;
            disks[1] = null;
            disks[2] = null;
            disks[3] = null;
            disks[4] = null;
            disks[5] = null;

            //Storing disk object into board array(Two dimensional arrray) 
            board = new Disk[NUM_PEGS, NUM_DISKS]; //condition says TWO dimentional array  

            board[0, 3] = new Disk();
            board[0, 2] = new Disk();
            board[0, 1] = new Disk();
            board[0, 0] = new Disk();
            board[0, 4] = new Disk();
            board[0, 5] = new Disk();


            //Creating arraylist of movement 
            movements = new ArrayList();
        }

        //Alterntative constructor
        public Board(Disk d1, Disk d2, Disk d3, Disk d4, Disk d5, Disk d6)
        {
            //Storing into disks array
            disks = new Disk[NUM_DISKS];
            disks[0] = d1;
            disks[1] = d2;
            disks[2] = d3;
            disks[3] = d4;
            disks[4] = d5;
            disks[5] = d6;

            //Storing disk object into board array(Two dimensional arrray) 
            board = new Disk[NUM_PEGS, NUM_DISKS]; //condition says TWO dimentional array  
            board[0, 5] = d1;
            board[0, 4] = d2;
            board[0, 3] = d3;
            board[0, 2] = d4;
            board[0, 1] = d5;
            board[0, 0] = d6;

            //Arraylist of movement.
            movements = new ArrayList();
        }


        public void reset()
        {
            for (int iP = 0; iP < NUM_PEGS; iP++)
            {
                //Remove all elements from board array
                for (int iD = 0; iD < NUM_DISKS; iD++)
                {
                    board[iP, iD] = null;

                    //Update disks array
                    disks[iD].setPegNum(0);
                    disks[iD].setLevel(NUM_DISKS - 1 - iD);
                }
            }

            //Reallocate elements 
            board[0, 5] = disks[0]; //Peg 1/Level4 
            board[0, 4] = disks[1]; //Peg 1/Level3 
            board[0, 3] = disks[2]; //Peg 1/Level2
            board[0, 2] = disks[3]; //Peg 1/Level1 
            board[0, 1] = disks[4];
            board[0, 0] = disks[5];

            //Remove all elements from movement arraylist
            movements.Clear();
            Display();
            steps = 0;
            
        }

        public bool canStartMove(Disk aDisk)
        {
            int oldpeg = aDisk.getPegNum();
            int oldlevel = aDisk.getLevel();
            if (oldlevel < 5)
            {
                if (board[oldpeg, oldlevel+1 ] != null)
                {
                    MessageBox.Show("Invalid Move - can only move top disk");
                    return false;
                }
            }
            return true;
        }

        public bool canDrop(Disk aDisk, int aPeg)
        {
            int newLev = 5;
            for (int i = 0; i < 6; i++)
            {
                if (board[aPeg,i] == null)
                {
                    newLev = i;
                    break;
                }
            }

            if (newLev > 0)
            {
                if ( aDisk.getDiameter() > board[aPeg,newLev-1].getDiameter())
                {
                    MessageBox.Show("Invalid Move - cannot drop bigger disk on smaller");
                    return false;
                }
            }
            board[aDisk.getPegNum(), aDisk.getLevel()] = null;
            return true;
        }

        public void move(Disk aDisk, int newLevel)
        {
            
            aDisk.setLevel(newLevel);
            board[aDisk.getPegNum(), newLevel] = aDisk;
            
            steps++;
            string moveString = "Moves:"+ Convert.ToString(steps)+ ", " + aDisk.getLabel().Name.Substring(3) + " to peg"
                + Convert.ToString(aDisk.getPegNum() + 1) + " level" + Convert.ToString(newLevel + 1);
            movements.Add(moveString);

            Display();
           
        }

        public string allMovesAsString()
        {
            string dummy = "";           
            string[] theboards = (string[])movements.ToArray(typeof(string));
            foreach (string b in theboards)
            {
                 dummy = dummy + b + "\r\n";
            }
            return dummy;  // Dummy return to avoid syntax error - must be changed
        }

        public void Display()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (board[i,j] != null)
                    {
                        Label display = this.board[i, j].getLabel();
                        int diameter = this.board[i, j].getDiameter();
                        //display.Hide();
                        display.Left = pegStart + (i * pegGap) - (diameter / 2);
                        display.Top = deckHeight - (j * diskHeight);
                        display.Show();
                    }
                }
            }
        }


        public Disk FindDisk(Label aLabel)
        {
            Disk fDisk = null;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (board[i, j] != null)
                    {
                        if (board[i, j].getLabel() == aLabel)
                        {
                            fDisk = board[i, j];
                            break;
                        }
                    }
                }
            }
            return fDisk;  // Dummy return to avoid syntax error - must be changed
        }

        public int newLevInPeg(int pegNum)
        {
            int newLevel = 3;
            for (int i = 0; i < 6; i++)
            {
                if (board[pegNum, i] == null)
                {
                    newLevel = i;
                    break;
                }
            }
            return newLevel;    
        }

 
        public String getText(int cnt)
        {
            return "1";    // Dummy return to avoid syntax error - must be changed
        }


        public void backToSelected(int ind)
        {

        }


        public int getPegInd(int ind)
        {
           
            return 1;    // Dummy return to avoid syntax error - must be changed
        }


        public int getLevel(int ind, Label thislable)
        {
            
            return ind;    // Dummy return to avoid syntax error - must be changed
        }


        public void unDo()
        {

        }


        public void loadData(ArrayList dm)
        {

        }
   }
}
