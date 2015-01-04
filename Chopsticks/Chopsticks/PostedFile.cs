using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chopsticks
{
    class PostedFile
    {
        static int i = 0;
        readonly string[] Backgrounds = { "#FE6A6A", "#ED91B6", "#DF93FF", "#BA97FF", "#C3C2E2", "#B0CFFE", "#BEE1E3", "#A7FCDB" };

        public PostedFile()
        {
            // Select a backgrpound color.
            Background = Backgrounds[i++];
            if (i >= Backgrounds.Length)
            {
                i = 0;
            }
        }

        public string FileName { get; set; }
        public string Background { get; set; }
        public string Status { 
            get
            {
                return "Pending";
            }
        }
    }
}
