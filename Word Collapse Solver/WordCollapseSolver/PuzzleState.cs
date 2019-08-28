using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;

namespace WordCollapseSolver
{
    class PuzzleState
    {
        List<string> Puzzle;
        List<string> Dictionary;
        public string Wordfound;
        public List<PuzzleState> NextWords;
        List<int[]>[] Indexes;
        DataGridView dg = new DataGridView();

        public PuzzleState(List<string> puz, List<string> dict, string word,DataGridView dgv)
        {
            Puzzle = new List<string>();
            Dictionary = new List<string>();
            Puzzle.AddRange(puz);
            Dictionary.AddRange(dict);
            Wordfound = word;
            NextWords = new List<PuzzleState>();
            Indexes = new List<int[]>[Dictionary.Count];
            FindIndexes();
            dg = dgv;
            

            Parallel.ForEach(Dictionary, currentword =>
            {
                SearchWord(currentword, Dictionary.IndexOf(currentword));
            });
            FindIndexes();
        }


        void FindIndexes()
        {
            Parallel.ForEach(Dictionary, currentword =>
            {
                int wordindex = Dictionary.IndexOf(currentword);
                int nextindex = 0;
                Indexes[wordindex] = new List<int[]>();

                for (int i = 0; i < Puzzle.Count; i++)
                {
                    for (int j = 0; j < Puzzle[i].Length; j++)
                    {
                        if (Char.ToLower(currentword[0]) == Char.ToLower(Puzzle[i][j]))
                        {
                            Indexes[wordindex].Insert(nextindex, new int[2] { i, j });
                            nextindex++;
                        }
                    }
                }
            });
        }

        void SearchWord(string word, int index)
        {


            Parallel.For(0, Indexes[index].Count, i =>
            {
                bool isHorizontal = true;
                bool isVertical = true;

                if ((Indexes[index][i][1] + Dictionary[index].Length - 1) < (Puzzle[Indexes[index][i][0]].Length))
                {
                    for (int j = 0, k = word.Length - 1; j < word.Length && k >= 0; j++, k--)
                    {
                        if (!Char.ToLower(Puzzle[Indexes[index][i][0]][Indexes[index][i][1] + j]).Equals(Char.ToLower(Dictionary[index][j])))
                        {
                            isHorizontal = false;
                        }

                        if (!Char.ToLower(Puzzle[Indexes[index][i][0]][Indexes[index][i][1] + k]).Equals(Char.ToLower(Dictionary[index][k])))
                        {
                            isHorizontal = false;
                        }
                    }
                }
                else isHorizontal = false;

                if ((Indexes[index][i][0] + Dictionary[index].Length - 1) < (Puzzle.Count))
                {
                    for (int j = 0, l = word.Length - 1; j < word.Length && l >= 0; j++, l--)
                    {

                        if (!Char.ToLower(Puzzle[Indexes[index][i][0] + j][Indexes[index][i][1]]).Equals(Char.ToLower(Dictionary[index][j])))
                        {
                            isVertical = false;
                        }

                        if (!Char.ToLower(Puzzle[Indexes[index][i][0] + l][Indexes[index][i][1]]).Equals(Char.ToLower(Dictionary[index][l])))
                        {
                            isVertical = false;
                        }
                    }
                }
                else isVertical = false;

                if (isHorizontal)
                {
                    AddChild(index, i, word, true);
                }

                else if (isVertical)
                {
                    AddChild(index, i, word, false);
                }
            });
        }

        void AddChild(int wordindex, int index, string word, bool Horizontal)
        {
            List<string> newpuz = new List<string>();
            List<string> newdict = new List<string>();
            newdict.AddRange(Dictionary);
            newdict.RemoveAt(wordindex);
            newpuz.AddRange(Puzzle);

            if (Horizontal)
            {
                StringBuilder str = new StringBuilder(newpuz[Indexes[wordindex][index][0]]);
                bool isBlank;
                for (int i = Indexes[wordindex][index][1]; i < word.Length + Indexes[wordindex][index][1]; i++)
                {
                    str[i] = ' ';
                };


                if (String.IsNullOrWhiteSpace(str.ToString()))
                    newpuz.RemoveAt(Indexes[wordindex][index][0]);
                else
                    newpuz[Indexes[wordindex][index][0]] = str.ToString();

                for (int i = Indexes[wordindex][index][1]; i < word.Length + Indexes[wordindex][index][1]; i++)
                {
                    isBlank = true;
                    for (int j = 0; j < newpuz.Count; j++)
                    {
                        if (!Char.IsWhiteSpace(newpuz[j][i]))
                        {
                            isBlank = false;
                            break;
                        }
                    }

                    if (isBlank)
                    {
                        for (int k = 0; k < newpuz.Count; k++)
                        {
                            newpuz[k] = newpuz[k].Remove(i, 1);
                        }
                        break;
                    }
                }

            }

            else
            {
                if ((Indexes[wordindex][index][0] == 0 && (Indexes[wordindex][index][0] + word.Length) == Puzzle.Count))
                {
                    for (int i = 0; i < Puzzle.Count; i++)
                    {
                        newpuz[i] = newpuz[i].Remove(Indexes[wordindex][index][1], 1);
                    };
                }

                else if((Indexes[wordindex][index][0] > 0) && (Char.IsWhiteSpace(Puzzle[Indexes[wordindex][index][0] - 1][Indexes[wordindex][index][1]]) && (Indexes[wordindex][index][0] + word.Length) == Puzzle.Count))
                {
                    for (int i = 0; i < Puzzle.Count; i++)
                    {
                        newpuz[i] = newpuz[i].Remove(Indexes[wordindex][index][1], 1);
                    };
                }

                else
                {
                    for (int i = Indexes[wordindex][index][0]; i < word.Length + Indexes[wordindex][index][0]; i++)
                    {
                        StringBuilder str = new StringBuilder(newpuz[i]);
                        str[Indexes[wordindex][index][1]] = ' ';
                        newpuz[i] = str.ToString();
                    }

                    if (Indexes[wordindex][index][0] > 0)
                    {
                        for (int i = Indexes[wordindex][index][0] - 1, j = Indexes[wordindex][index][0] + word.Length - 1; i >= 0; i--, j--)
                        {
                            StringBuilder str = new StringBuilder(newpuz[i]);
                            StringBuilder str2 = new StringBuilder(newpuz[j]);
                            str2[Indexes[wordindex][index][1]] = str[Indexes[wordindex][index][1]];
                            str[Indexes[wordindex][index][1]] = ' ';
                            newpuz[i] = str.ToString();
                            newpuz[j] = str2.ToString();
                        }
                    }
                }
            }

            NextWords.Add(new PuzzleState(newpuz, newdict, word,dg));
        }

        public void PrintPuzzle()
        {
            dg.ClearSelection();
            dg.Rows.Clear();
            dg.ColumnCount = Puzzle[Puzzle.Count-1].Length;
            
            for (int i = 0; i < Puzzle.Count; i++)
            {
                var row = new DataGridViewRow();
                for (int columnIndex = 0; columnIndex < Puzzle[i].Length; columnIndex++)
                {
                    row.Cells.Add(new DataGridViewTextBoxCell()
                    {
                        Value = Puzzle[i][columnIndex]
                    });
                }
                
                dg.Rows.Add(row);
               
            }
        }

        public bool CheckBlankPuzzle()
        {
            for (int i = 0; i < this.Puzzle.Count; i++)
            {
                if (!String.IsNullOrWhiteSpace(this.Puzzle[i]))
                    return false;
            }
            return true;
        }
    }
}
