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
using Newtonsoft.Json;

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

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var jsonString= JsonConvert.DeserializeObject<DataSet>(File.ReadAllText("weaponTemplate.json"));
            string jsonString = File.ReadAllText("weaponTemplate.json");
            var weapons = Weapons.FromJson(jsonString);
            dataGridView1.DataSource = new List<Weapons>() { weapons };

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
