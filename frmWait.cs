using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UBSAPConnectivity
{
    public partial class frmWait : Form
    {
        public frmWait(string labelText)
        {
            InitializeComponent();
            lblWait.Text = labelText;
        }



       
        private void frmWait_Load(object sender, EventArgs e)
        {
           
        }

        private void lblWait_Click(object sender, EventArgs e)
        {

        }
    }

      
}
