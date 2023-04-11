namespace CodeParser
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_ParseFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_ParseFile
            // 
            this.btn_ParseFile.Location = new System.Drawing.Point(12, 12);
            this.btn_ParseFile.Name = "btn_ParseFile";
            this.btn_ParseFile.Size = new System.Drawing.Size(112, 34);
            this.btn_ParseFile.TabIndex = 0;
            this.btn_ParseFile.Text = "File";
            this.btn_ParseFile.UseVisualStyleBackColor = true;
            this.btn_ParseFile.Click += new System.EventHandler(this.btn_ParseFile_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_ParseFile);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Button btn_ParseFile;
    }
}