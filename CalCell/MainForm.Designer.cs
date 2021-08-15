namespace CalCell
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.CellFieldPictureBox = new System.Windows.Forms.PictureBox();
            this.CalcPanel = new System.Windows.Forms.Panel();
            this.CommentLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SpeedBar = new System.Windows.Forms.TrackBar();
            this.Button_ABORT = new System.Windows.Forms.Button();
            this.NumTextBox = new System.Windows.Forms.TextBox();
            this.Button_AC = new System.Windows.Forms.Button();
            this.Button_BS = new System.Windows.Forms.Button();
            this.Button_DIV = new System.Windows.Forms.Button();
            this.Button_MUL = new System.Windows.Forms.Button();
            this.Button_MINUS = new System.Windows.Forms.Button();
            this.Button_PLUS = new System.Windows.Forms.Button();
            this.Button_EQ = new System.Windows.Forms.Button();
            this.Button_9 = new System.Windows.Forms.Button();
            this.Button_7 = new System.Windows.Forms.Button();
            this.Button_8 = new System.Windows.Forms.Button();
            this.Button_6 = new System.Windows.Forms.Button();
            this.Button_4 = new System.Windows.Forms.Button();
            this.Button_5 = new System.Windows.Forms.Button();
            this.Button_3 = new System.Windows.Forms.Button();
            this.Button_1 = new System.Windows.Forms.Button();
            this.Button_2 = new System.Windows.Forms.Button();
            this.Button_0 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.CellFieldPictureBox)).BeginInit();
            this.CalcPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpeedBar)).BeginInit();
            this.SuspendLayout();
            // 
            // CellFieldPictureBox
            // 
            this.CellFieldPictureBox.Location = new System.Drawing.Point(12, 12);
            this.CellFieldPictureBox.Name = "CellFieldPictureBox";
            this.CellFieldPictureBox.Size = new System.Drawing.Size(1420, 2200);
            this.CellFieldPictureBox.TabIndex = 1;
            this.CellFieldPictureBox.TabStop = false;
            // 
            // CalcPanel
            // 
            this.CalcPanel.Controls.Add(this.CommentLabel);
            this.CalcPanel.Controls.Add(this.label2);
            this.CalcPanel.Controls.Add(this.label1);
            this.CalcPanel.Controls.Add(this.SpeedBar);
            this.CalcPanel.Controls.Add(this.Button_ABORT);
            this.CalcPanel.Controls.Add(this.NumTextBox);
            this.CalcPanel.Controls.Add(this.Button_AC);
            this.CalcPanel.Controls.Add(this.Button_BS);
            this.CalcPanel.Controls.Add(this.Button_DIV);
            this.CalcPanel.Controls.Add(this.Button_MUL);
            this.CalcPanel.Controls.Add(this.Button_MINUS);
            this.CalcPanel.Controls.Add(this.Button_PLUS);
            this.CalcPanel.Controls.Add(this.Button_EQ);
            this.CalcPanel.Controls.Add(this.Button_9);
            this.CalcPanel.Controls.Add(this.Button_7);
            this.CalcPanel.Controls.Add(this.Button_8);
            this.CalcPanel.Controls.Add(this.Button_6);
            this.CalcPanel.Controls.Add(this.Button_4);
            this.CalcPanel.Controls.Add(this.Button_5);
            this.CalcPanel.Controls.Add(this.Button_3);
            this.CalcPanel.Controls.Add(this.Button_1);
            this.CalcPanel.Controls.Add(this.Button_2);
            this.CalcPanel.Controls.Add(this.Button_0);
            this.CalcPanel.Location = new System.Drawing.Point(1438, 12);
            this.CalcPanel.Name = "CalcPanel";
            this.CalcPanel.Size = new System.Drawing.Size(380, 676);
            this.CalcPanel.TabIndex = 5;
            // 
            // CommentLabel
            // 
            this.CommentLabel.AutoSize = true;
            this.CommentLabel.Font = new System.Drawing.Font("メイリオ", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.CommentLabel.Location = new System.Drawing.Point(3, 599);
            this.CommentLabel.Name = "CommentLabel";
            this.CommentLabel.Size = new System.Drawing.Size(0, 42);
            this.CommentLabel.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(322, 399);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 25);
            this.label2.TabIndex = 19;
            this.label2.Text = "Slow";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 399);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 25);
            this.label1.TabIndex = 18;
            this.label1.Text = "Fast";
            // 
            // SpeedBar
            // 
            this.SpeedBar.Location = new System.Drawing.Point(107, 399);
            this.SpeedBar.Maximum = 100;
            this.SpeedBar.Name = "SpeedBar";
            this.SpeedBar.Size = new System.Drawing.Size(217, 53);
            this.SpeedBar.TabIndex = 17;
            this.SpeedBar.TabStop = false;
            this.SpeedBar.Value = 20;
            this.SpeedBar.Scroll += new System.EventHandler(this.SpeedBar_Scroll);
            this.SpeedBar.MouseLeave += new System.EventHandler(this.SpeedBar_MouseLeave);
            // 
            // Button_ABORT
            // 
            this.Button_ABORT.BackColor = System.Drawing.Color.Red;
            this.Button_ABORT.ForeColor = System.Drawing.Color.White;
            this.Button_ABORT.Location = new System.Drawing.Point(285, 489);
            this.Button_ABORT.Name = "Button_ABORT";
            this.Button_ABORT.Size = new System.Drawing.Size(91, 48);
            this.Button_ABORT.TabIndex = 16;
            this.Button_ABORT.TabStop = false;
            this.Button_ABORT.Text = "ABORT";
            this.Button_ABORT.UseVisualStyleBackColor = false;
            this.Button_ABORT.Click += new System.EventHandler(this.Button_ABORT_Click);
            // 
            // NumTextBox
            // 
            this.NumTextBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.NumTextBox.Font = new System.Drawing.Font("メイリオ", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.NumTextBox.Location = new System.Drawing.Point(3, 3);
            this.NumTextBox.Name = "NumTextBox";
            this.NumTextBox.ReadOnly = true;
            this.NumTextBox.ShortcutsEnabled = false;
            this.NumTextBox.Size = new System.Drawing.Size(373, 48);
            this.NumTextBox.TabIndex = 6;
            this.NumTextBox.Text = "0";
            this.NumTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Button_AC
            // 
            this.Button_AC.BackColor = System.Drawing.Color.Bisque;
            this.Button_AC.Font = new System.Drawing.Font("メイリオ", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Button_AC.Location = new System.Drawing.Point(220, 82);
            this.Button_AC.Name = "Button_AC";
            this.Button_AC.Size = new System.Drawing.Size(75, 50);
            this.Button_AC.TabIndex = 15;
            this.Button_AC.TabStop = false;
            this.Button_AC.Text = "AC";
            this.Button_AC.UseVisualStyleBackColor = false;
            this.Button_AC.Click += new System.EventHandler(this.Button_AC_Click);
            // 
            // Button_BS
            // 
            this.Button_BS.BackColor = System.Drawing.Color.Bisque;
            this.Button_BS.Font = new System.Drawing.Font("メイリオ", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Button_BS.Location = new System.Drawing.Point(301, 82);
            this.Button_BS.Name = "Button_BS";
            this.Button_BS.Size = new System.Drawing.Size(75, 50);
            this.Button_BS.TabIndex = 14;
            this.Button_BS.TabStop = false;
            this.Button_BS.Text = "BS";
            this.Button_BS.UseVisualStyleBackColor = false;
            this.Button_BS.Click += new System.EventHandler(this.Button_BS_Click);
            // 
            // Button_DIV
            // 
            this.Button_DIV.BackColor = System.Drawing.Color.Linen;
            this.Button_DIV.Font = new System.Drawing.Font("メイリオ", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Button_DIV.Location = new System.Drawing.Point(301, 138);
            this.Button_DIV.Name = "Button_DIV";
            this.Button_DIV.Size = new System.Drawing.Size(75, 50);
            this.Button_DIV.TabIndex = 13;
            this.Button_DIV.TabStop = false;
            this.Button_DIV.Text = "÷";
            this.Button_DIV.UseVisualStyleBackColor = false;
            this.Button_DIV.Click += new System.EventHandler(this.Button_DIV_Click);
            // 
            // Button_MUL
            // 
            this.Button_MUL.BackColor = System.Drawing.Color.Linen;
            this.Button_MUL.Font = new System.Drawing.Font("メイリオ", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Button_MUL.Location = new System.Drawing.Point(301, 194);
            this.Button_MUL.Name = "Button_MUL";
            this.Button_MUL.Size = new System.Drawing.Size(75, 50);
            this.Button_MUL.TabIndex = 12;
            this.Button_MUL.TabStop = false;
            this.Button_MUL.Text = "×";
            this.Button_MUL.UseVisualStyleBackColor = false;
            this.Button_MUL.Click += new System.EventHandler(this.Button_MUL_Click);
            // 
            // Button_MINUS
            // 
            this.Button_MINUS.BackColor = System.Drawing.Color.Linen;
            this.Button_MINUS.Font = new System.Drawing.Font("メイリオ", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Button_MINUS.Location = new System.Drawing.Point(301, 250);
            this.Button_MINUS.Name = "Button_MINUS";
            this.Button_MINUS.Size = new System.Drawing.Size(75, 50);
            this.Button_MINUS.TabIndex = 11;
            this.Button_MINUS.TabStop = false;
            this.Button_MINUS.Text = "－";
            this.Button_MINUS.UseVisualStyleBackColor = false;
            this.Button_MINUS.Click += new System.EventHandler(this.Button_MINUS_Click);
            // 
            // Button_PLUS
            // 
            this.Button_PLUS.BackColor = System.Drawing.Color.Linen;
            this.Button_PLUS.Font = new System.Drawing.Font("メイリオ", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Button_PLUS.Location = new System.Drawing.Point(301, 306);
            this.Button_PLUS.Name = "Button_PLUS";
            this.Button_PLUS.Size = new System.Drawing.Size(75, 50);
            this.Button_PLUS.TabIndex = 10;
            this.Button_PLUS.TabStop = false;
            this.Button_PLUS.Text = "＋";
            this.Button_PLUS.UseVisualStyleBackColor = false;
            this.Button_PLUS.Click += new System.EventHandler(this.Button_PLUS_Click);
            // 
            // Button_EQ
            // 
            this.Button_EQ.BackColor = System.Drawing.Color.Bisque;
            this.Button_EQ.Font = new System.Drawing.Font("メイリオ", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Button_EQ.Location = new System.Drawing.Point(220, 306);
            this.Button_EQ.Name = "Button_EQ";
            this.Button_EQ.Size = new System.Drawing.Size(75, 50);
            this.Button_EQ.TabIndex = 9;
            this.Button_EQ.TabStop = false;
            this.Button_EQ.Text = "=";
            this.Button_EQ.UseVisualStyleBackColor = false;
            this.Button_EQ.Click += new System.EventHandler(this.Button_EQ_Click);
            // 
            // Button_9
            // 
            this.Button_9.BackColor = System.Drawing.Color.Gainsboro;
            this.Button_9.Font = new System.Drawing.Font("メイリオ", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Button_9.Location = new System.Drawing.Point(220, 138);
            this.Button_9.Name = "Button_9";
            this.Button_9.Size = new System.Drawing.Size(75, 50);
            this.Button_9.TabIndex = 8;
            this.Button_9.TabStop = false;
            this.Button_9.Text = "9";
            this.Button_9.UseVisualStyleBackColor = false;
            this.Button_9.Click += new System.EventHandler(this.Button_9_Click);
            // 
            // Button_7
            // 
            this.Button_7.BackColor = System.Drawing.Color.Gainsboro;
            this.Button_7.Font = new System.Drawing.Font("メイリオ", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Button_7.Location = new System.Drawing.Point(58, 138);
            this.Button_7.Name = "Button_7";
            this.Button_7.Size = new System.Drawing.Size(75, 50);
            this.Button_7.TabIndex = 6;
            this.Button_7.TabStop = false;
            this.Button_7.Text = "7";
            this.Button_7.UseVisualStyleBackColor = false;
            this.Button_7.Click += new System.EventHandler(this.Button_7_Click);
            // 
            // Button_8
            // 
            this.Button_8.BackColor = System.Drawing.Color.Gainsboro;
            this.Button_8.Font = new System.Drawing.Font("メイリオ", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Button_8.Location = new System.Drawing.Point(139, 138);
            this.Button_8.Name = "Button_8";
            this.Button_8.Size = new System.Drawing.Size(75, 50);
            this.Button_8.TabIndex = 7;
            this.Button_8.TabStop = false;
            this.Button_8.Text = "8";
            this.Button_8.UseVisualStyleBackColor = false;
            this.Button_8.Click += new System.EventHandler(this.Button_8_Click);
            // 
            // Button_6
            // 
            this.Button_6.BackColor = System.Drawing.Color.Gainsboro;
            this.Button_6.Font = new System.Drawing.Font("メイリオ", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Button_6.Location = new System.Drawing.Point(220, 194);
            this.Button_6.Name = "Button_6";
            this.Button_6.Size = new System.Drawing.Size(75, 50);
            this.Button_6.TabIndex = 5;
            this.Button_6.TabStop = false;
            this.Button_6.Text = "6";
            this.Button_6.UseVisualStyleBackColor = false;
            this.Button_6.Click += new System.EventHandler(this.Button_6_Click);
            // 
            // Button_4
            // 
            this.Button_4.BackColor = System.Drawing.Color.Gainsboro;
            this.Button_4.Font = new System.Drawing.Font("メイリオ", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Button_4.Location = new System.Drawing.Point(58, 194);
            this.Button_4.Name = "Button_4";
            this.Button_4.Size = new System.Drawing.Size(75, 50);
            this.Button_4.TabIndex = 3;
            this.Button_4.TabStop = false;
            this.Button_4.Text = "4";
            this.Button_4.UseVisualStyleBackColor = false;
            this.Button_4.Click += new System.EventHandler(this.Button_4_Click);
            // 
            // Button_5
            // 
            this.Button_5.BackColor = System.Drawing.Color.Gainsboro;
            this.Button_5.Font = new System.Drawing.Font("メイリオ", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Button_5.Location = new System.Drawing.Point(139, 194);
            this.Button_5.Name = "Button_5";
            this.Button_5.Size = new System.Drawing.Size(75, 50);
            this.Button_5.TabIndex = 4;
            this.Button_5.TabStop = false;
            this.Button_5.Text = "5";
            this.Button_5.UseVisualStyleBackColor = false;
            this.Button_5.Click += new System.EventHandler(this.Button_5_Click);
            // 
            // Button_3
            // 
            this.Button_3.BackColor = System.Drawing.Color.Gainsboro;
            this.Button_3.Font = new System.Drawing.Font("メイリオ", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Button_3.Location = new System.Drawing.Point(220, 250);
            this.Button_3.Name = "Button_3";
            this.Button_3.Size = new System.Drawing.Size(75, 50);
            this.Button_3.TabIndex = 2;
            this.Button_3.TabStop = false;
            this.Button_3.Text = "3";
            this.Button_3.UseVisualStyleBackColor = false;
            this.Button_3.Click += new System.EventHandler(this.Button_3_Click);
            // 
            // Button_1
            // 
            this.Button_1.BackColor = System.Drawing.Color.Gainsboro;
            this.Button_1.Font = new System.Drawing.Font("メイリオ", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Button_1.Location = new System.Drawing.Point(58, 250);
            this.Button_1.Name = "Button_1";
            this.Button_1.Size = new System.Drawing.Size(75, 50);
            this.Button_1.TabIndex = 1;
            this.Button_1.TabStop = false;
            this.Button_1.Text = "1";
            this.Button_1.UseVisualStyleBackColor = false;
            this.Button_1.Click += new System.EventHandler(this.Button_1_Click);
            // 
            // Button_2
            // 
            this.Button_2.BackColor = System.Drawing.Color.Gainsboro;
            this.Button_2.Font = new System.Drawing.Font("メイリオ", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Button_2.Location = new System.Drawing.Point(139, 250);
            this.Button_2.Name = "Button_2";
            this.Button_2.Size = new System.Drawing.Size(75, 50);
            this.Button_2.TabIndex = 1;
            this.Button_2.TabStop = false;
            this.Button_2.Text = "2";
            this.Button_2.UseVisualStyleBackColor = false;
            this.Button_2.Click += new System.EventHandler(this.Button_2_Click);
            // 
            // Button_0
            // 
            this.Button_0.BackColor = System.Drawing.Color.Gainsboro;
            this.Button_0.Font = new System.Drawing.Font("メイリオ", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Button_0.Location = new System.Drawing.Point(58, 306);
            this.Button_0.Name = "Button_0";
            this.Button_0.Size = new System.Drawing.Size(156, 50);
            this.Button_0.TabIndex = 0;
            this.Button_0.TabStop = false;
            this.Button_0.Text = "0";
            this.Button_0.UseVisualStyleBackColor = false;
            this.Button_0.Click += new System.EventHandler(this.Button_0_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1890, 946);
            this.Controls.Add(this.CalcPanel);
            this.Controls.Add(this.CellFieldPictureBox);
            this.Font = new System.Drawing.Font("メイリオ", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.Text = "CalCell (筆算）";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.CellFieldPictureBox)).EndInit();
            this.CalcPanel.ResumeLayout(false);
            this.CalcPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpeedBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox CellFieldPictureBox;
        private System.Windows.Forms.Panel CalcPanel;
        private System.Windows.Forms.Button Button_AC;
        private System.Windows.Forms.Button Button_BS;
        private System.Windows.Forms.Button Button_DIV;
        private System.Windows.Forms.Button Button_MUL;
        private System.Windows.Forms.Button Button_MINUS;
        private System.Windows.Forms.Button Button_PLUS;
        private System.Windows.Forms.Button Button_EQ;
        private System.Windows.Forms.Button Button_9;
        private System.Windows.Forms.Button Button_7;
        private System.Windows.Forms.Button Button_8;
        private System.Windows.Forms.Button Button_6;
        private System.Windows.Forms.Button Button_4;
        private System.Windows.Forms.Button Button_5;
        private System.Windows.Forms.Button Button_3;
        private System.Windows.Forms.Button Button_1;
        private System.Windows.Forms.Button Button_2;
        private System.Windows.Forms.Button Button_0;
        private System.Windows.Forms.TextBox NumTextBox;
        private System.Windows.Forms.Button Button_ABORT;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar SpeedBar;
        private System.Windows.Forms.Label CommentLabel;
    }
}

