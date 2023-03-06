using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DocTranslate
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public Form2(string txt, int DocCount)
        {

            InitializeComponent();
        //    Label lbl = new Label();
            label1.Text ="Please Wait!!! Processing Document" + txt + "Document No" + DocCount + "In input folder ";
           // lblWait.ResetText();
           // lblWait.Refresh();

        }



    }
}
