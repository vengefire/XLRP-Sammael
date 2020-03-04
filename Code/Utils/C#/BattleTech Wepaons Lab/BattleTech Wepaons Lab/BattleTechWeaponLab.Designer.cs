namespace BattleTech_Wepaons_Lab
{
    partial class BattleTechWeaponLab
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.weaponsDescriptionBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.weaponsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.weaponsDescriptionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.weaponsDescriptionBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.weaponsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.weaponsDescriptionBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 262);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1873, 905);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // weaponsDescriptionBindingSource1
            // 
            this.weaponsDescriptionBindingSource1.DataSource = typeof(BattleTech_Wepaons_Lab.WeaponsDescription);
            // 
            // weaponsBindingSource
            // 
            this.weaponsBindingSource.DataSource = typeof(BattleTech_Wepaons_Lab.Weapons);
            // 
            // weaponsDescriptionBindingSource
            // 
            this.weaponsDescriptionBindingSource.DataSource = typeof(BattleTech_Wepaons_Lab.WeaponsDescription);
            // 
            // BattleTechWeaponLab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1897, 929);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);
            this.Name = "BattleTechWeaponLab";
            this.Text = "Battle Tech Weapons Lab";
            this.Load += new System.EventHandler(this.BattleTechWeaponLab_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.weaponsDescriptionBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.weaponsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.weaponsDescriptionBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource weaponsDescriptionBindingSource;
        private System.Windows.Forms.BindingSource weaponsBindingSource;
        private System.Windows.Forms.BindingSource weaponsDescriptionBindingSource1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

