using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace BattleTech_Wepaons_Lab
{
    public partial class BattleTechWeaponLab : Form
    {
        int leftcontrol = 1;
        public BattleTechWeaponLab()
        {
            InitializeComponent();
        }

        private void BattleTechWeaponLab_Load(object sender, EventArgs e)
        {
            ListBox lb = new ListBox();
            this.Controls.Add(lb);
            lb.Items.Add("123");
            lb.Items.Add(456);
            lb.Items.Add(false);
        }
        private void CreateLabels(int topControl, int leftControl, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Label dynamicLabel = new Label();
                this.Controls.Add(dynamicLabel);
                dynamicLabel.Top = topControl * 25;
                dynamicLabel.Left = 25;
                dynamicLabel.Text = "Text: " + this.leftcontrol.ToString();
                topControl++;
            }
        }
        private void CreateTexBox(int topControl, int leftControl, int count)
        {
            for (int i = 0; i < count; i++)
            {
                TextBox textBox = new TextBox();
                this.Controls.Add(textBox);
                textBox.Top = topControl*25;
                textBox.Left = leftControl;
                textBox.Text = "Box of text" + count.ToString();
                topControl++;
            }
        }
        private void CreateComboBox(int topControl, int leftControl, int count)
        {
            for (int i = 0; i < count; i++)
            {
                ComboBox cmbBox = new ComboBox();
                this.Controls.Add(cmbBox);
                cmbBox.Top = topControl * 25;
                cmbBox.Left = leftControl;
                cmbBox.Text = "Box of text" + count.ToString();
                topControl++;
            }
        }
    }
}
