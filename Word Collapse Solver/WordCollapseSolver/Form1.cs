using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordCollapseSolver
{
    public partial class Form1 : Form
    {
        List<string> puzzle = new List<string>();
        List<string> dictionary = new List<string>();
           
        public Form1()
        {
            InitializeComponent();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            richTextBox1.Clear();
            richTextBox2.Clear();
            richTextBox1.AppendText("Puzzle Log:\n");
            richTextBox2.AppendText("Dictionary:\n");
            try
            {
                Filing file = new Filing();
                file.GetInputs(puzzle, dictionary);

                if (puzzle.Count == 0 && dictionary.Count == 0)
                {
                    throw new Exception();
                }

                dataGridView1.ColumnCount = puzzle[puzzle.Count-1].Length;

                for (int i = 0; i < dictionary.Count;i++ )
                {
                    richTextBox2.AppendText(dictionary[i] + "\n");
                }
                    for (int i = 0; i < (puzzle.Count); i++)
                    {
                        var row = new DataGridViewRow();
                        for (int columnIndex = 0; columnIndex < puzzle[i].Length; columnIndex++)
                        {

                            row.Cells.Add(new DataGridViewTextBoxCell()
                            {
                                Value = puzzle[i][columnIndex]
                            });
                        }

                        dataGridView1.Rows.Add(row);
                    }
            }
            
            catch(Exception)
            {
                MessageBox.Show("Please Load The Puzzle !!!", "Puzzle Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);               
            }
   }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Solve_Click(object sender, EventArgs e)
        {
            if ((puzzle.Count != 0) || (dictionary.Count != 0))
            {
                PuzzleState puz = new PuzzleState(puzzle, dictionary, null, dataGridView1);
                Strategy str = new Strategy(puz, richTextBox1);
            }

            else
            {
                MessageBox.Show("Please Load The Puzzle !!!", "Puzzle Not Found",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            richTextBox1.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            richTextBox2.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            richTextBox1.AppendText("Puzzle Log:\n");
            richTextBox2.AppendText("Dictionary:\n");
        }
    }
}
