using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace WordCollapseSolver
{
    class Strategy
    {
        Stack<PuzzleState> frontier;
        PuzzleState initial;
        RichTextBox rt = new RichTextBox();

        public Strategy(PuzzleState init,RichTextBox box)
        {
            initial = init;
            frontier = new Stack<PuzzleState>();
            frontier.Push(initial);
            rt = box;
            SolvePuzzle();
            
        }

        void SolvePuzzle()
        {
            bool Solved = false;
            while (frontier.Count > 0)
            {
                PuzzleState currentstate = frontier.Pop();
               
                currentstate.PrintPuzzle();
                if (currentstate.Wordfound != null)
                {
                    MessageBox.Show(currentstate.Wordfound + " Found","Word Found",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    rt.AppendText("Word Found: " + currentstate.Wordfound + "\n");
                }   

                if (GoalTest(currentstate))
                {
                    initial.PrintPuzzle();
                    MessageBox.Show("Puzzle Solved !!!", "Puzzle Solved", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    rt.AppendText("Puzzle Solved !!!");
                    Solved = true;
                    break;
                }
                
                for (int i = 0; i < currentstate.NextWords.Count; i++)
                {
                    frontier.Push(currentstate.NextWords[i]);
                }
            }
            
            if(!Solved)
            {
                MessageBox.Show("Puzzle Cannot Be Solved !!!", "Puzzle Not Solved", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                rt.AppendText("Puzzle Not Solved !!!");
            }
        }

        bool GoalTest(PuzzleState puz)
        {
            return puz.CheckBlankPuzzle();
        }
    }
}
