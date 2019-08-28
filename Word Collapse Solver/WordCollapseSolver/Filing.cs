using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace WordCollapseSolver
{
    class Filing
    {
        public void GetInputs(List<string> puz, List<string> dict)
        {
            string[] file = null;
            int dictstart = 0;
            puz.Clear();
            dict.Clear();
            OpenFileDialog filedialog = new OpenFileDialog();
            filedialog.Multiselect = false;
            if (filedialog.ShowDialog() == DialogResult.OK)
            {
                file = File.ReadAllLines(filedialog.FileName);


                for (int i = 0; !file[i].Equals("Dictionary:"); i++)
                {
                    if (!file[i].Equals(""))
                        puz.Add(file[i]);

                    dictstart = i;
                }

                for (int i = dictstart + 2, j = 0; i < file.Length; i++, j++)
                {
                    if (!file[i].Equals(""))
                        dict.Add(file[i]);
                }
            }
        }
    }
}
