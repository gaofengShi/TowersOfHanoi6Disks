using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Towers_of_Hanoi
{
    public partial class MainForm : Form
    {
        private Board myboard;
        private Disk disk1;
        private Disk disk2;
        private Disk disk3;
        private Disk disk4;
        private Disk disk5;
        private Disk disk6;
        private ArrayList diskMoveList;
        private DiskMove myDiskMove,move;
        private int targetPeg;
        private DiskMove[] moves;
        private int animateLine = 0;
        private SaveFileDialog saveFile;
        private OpenFileDialog openFile;

        public MainForm()
        {
            InitializeComponent();           
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            disk1 = new Disk(lblDisk1, 1, 5, 0);
            disk2 = new Disk(lblDisk2, 2, 4, 0);
            disk3 = new Disk(lblDisk3, 3, 3, 0);
            disk4 = new Disk(lblDisk4, 4, 2, 0);
            disk5 = new Disk(lblDisk5, 5, 1, 0);
            disk6 = new Disk(lblDisk6, 6, 0, 0);
            myboard = new Board(disk1,disk2,disk3,disk4,disk5,disk6);
            diskMoveList = new ArrayList();
            saveFile = new SaveFileDialog();
            openFile = new OpenFileDialog();
            saveFile.InitialDirectory = @"c:\Temp\";
            openFile.InitialDirectory = @"c:\Temp\";
            openFile.Filter = "txt files | *.tht";
            saveFile.Filter = "txt files | *.tht";

        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            myboard.reset();
           // lblMoves.Text = "I am very very hugary";
            txtMoves.Clear();
            diskMoveList.Clear();
        }

        private void lblDisk1_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void lblPeg1_DragDrop(object sender, DragEventArgs e)
        {
            Label alabel = (sender as Label);
            if (alabel == lblPeg1) targetPeg = 0;
            else if (alabel == lblPeg2) targetPeg = 1;
            else if (alabel == lblPeg3) targetPeg = 2;
        }

        private void lblPeg1_DragEnter(object sender, DragEventArgs e)
        {          
                e.Effect = DragDropEffects.All;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!MakeNextMove())
            {
                timer1.Enabled = false;
                lblMoves.Text = "";
            }
        }

        private void btnAnimate_Click(object sender, EventArgs e)
        {
            animating();
        }

        private void animating()
        {
            btnReset.Enabled = false;
            myboard.reset();
            txtMoves.Clear();
            animateLine = 0;
            moves = (DiskMove[])diskMoveList.ToArray(typeof(DiskMove));
            diskMoveList.Clear();
            timer1.Enabled = true;
        }

        private bool MakeNextMove()
        {               
            if (animateLine < moves.Length)
            {
                move = moves[animateLine];
                int ind, peg;
                ind = move.getDiskInd();
                peg = move.getPegInd();
                Disk aDisk = setaDisk(ind);
                myboard.canDrop(aDisk, peg);             //clear the old board[];
                aDisk.setPegNum(peg);                    //set the new peg to the Disk.
                int newLevel = myboard.newLevInPeg(peg); // to find the new level.
                myboard.move(aDisk, newLevel);                 //move the Disk to new position.
                aDisk = null;
                txtMoves.Text = myboard.allMovesAsString();
                diskMoveList.Add(move);                       // add the Moves.
                lblMoves.Text = move.AsText();
                animateLine++;
                return true;
            }
            else
            {
                btnReset.Enabled=true;
                return false;                        
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder();
                DiskMove[] ms = (DiskMove[])diskMoveList.ToArray(typeof(DiskMove));
                foreach (DiskMove item in ms)
                {
                    string s = item.commaText();
                    sb.Append(s + "\r\n");
                }
                StreamWriter sw = new StreamWriter(saveFile.FileName);
                sw.Write(sb.ToString());
                sw.Close();
                openFile.FileName = saveFile.FileName;
            }
            
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    diskMoveList.Clear();
                    StreamReader sr = new StreamReader(openFile.FileName);
                    string temp = sr.ReadLine();
                    while (temp != null)
                    {
                        myDiskMove = new DiskMove(temp);
                        diskMoveList.Add(myDiskMove);
                        temp = sr.ReadLine();
                    }
                    animating();
                    sr.Close();
                    saveFile.FileName = openFile.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\r\n" + ex.ToString());
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lblDisk1_MouseDown_1(object sender, MouseEventArgs e)
        {
            Label alabel = (sender as Label);
            Disk aDisk = myboard.FindDisk(alabel);

            DragDropEffects result = alabel.DoDragDrop(alabel, DragDropEffects.All);
            if ((result != DragDropEffects.None) && (myboard.canStartMove(aDisk)))
            {
                if (myboard.canDrop(aDisk, targetPeg))
                {
                    aDisk.setPegNum(targetPeg);                    //set the new peg to the Disk.
                    int newLevel = myboard.newLevInPeg(targetPeg); // to find the new level.
                    myboard.move(aDisk, newLevel);                 //move the Disk to new position.

                    int a = Convert.ToInt32(alabel.Name.Substring(7));  // find the disk ind from the label
                    myDiskMove = new DiskMove(a, targetPeg);            // set the new DiskMove for story.
                    diskMoveList.Add(myDiskMove);                       // add the Move.


                    txtMoves.Text = myboard.allMovesAsString();  // display the steps on the boxlist.

                    if ((targetPeg == 2) && (newLevel == 5))    // if the game end show the message.
                    {
                        System.Media.SystemSounds.Exclamation.Play();
                        if (myboard.steps == 63)
                            MessageBox.Show("You have successfully completed the game with the minimum number of moves");
                        else
                            MessageBox.Show("You have successfully completed the game but not with the minimum number of moves");
                    }

                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Rainbow Towers of Hanoi 1.0 \nWritten by: Shane", "About");
        }

        private Disk setaDisk(int ind)
        {
            if (ind == 1)
            {
                return disk1;
            }
            else if (ind == 2)
            {
                return disk2;
            }
            else if (ind == 3)
            {
                return disk3;
            }
            else if (ind == 4)
            {
                return disk4;

            }
            else if (ind == 5)
            {
                return disk5;

            }
            else if (ind == 6)
            {
                return disk6;

            }
            else return disk4;
        }
    }
}
