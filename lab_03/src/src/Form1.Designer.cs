namespace src
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbl_content = new System.Windows.Forms.Label();
            this.lbl_n = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_forward_nums = new System.Windows.Forms.Button();
            this.btn_random_nums = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btn_full_search = new System.Windows.Forms.Button();
            this.btn_bin_search = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lbl_comp = new System.Windows.Forms.Label();
            this.lbl_state = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btn_save_gisto = new System.Windows.Forms.Button();
            this.btn_gisto = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart3 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart3)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbl_content);
            this.groupBox1.Controls.Add(this.lbl_n);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 58);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Информация о массиве";
            // 
            // lbl_content
            // 
            this.lbl_content.AutoSize = true;
            this.lbl_content.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_content.Location = new System.Drawing.Point(6, 29);
            this.lbl_content.Name = "lbl_content";
            this.lbl_content.Size = new System.Drawing.Size(62, 13);
            this.lbl_content.TabIndex = 1;
            this.lbl_content.Text = "Заполнен: ";
            // 
            // lbl_n
            // 
            this.lbl_n.AutoSize = true;
            this.lbl_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_n.Location = new System.Drawing.Point(6, 16);
            this.lbl_n.Name = "lbl_n";
            this.lbl_n.Size = new System.Drawing.Size(105, 13);
            this.lbl_n.TabIndex = 0;
            this.lbl_n.Text = "Кол-во элементов: ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_forward_nums);
            this.groupBox2.Controls.Add(this.btn_random_nums);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(12, 76);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 100);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Заполнить массив числами";
            // 
            // btn_forward_nums
            // 
            this.btn_forward_nums.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_forward_nums.Location = new System.Drawing.Point(6, 38);
            this.btn_forward_nums.Name = "btn_forward_nums";
            this.btn_forward_nums.Size = new System.Drawing.Size(188, 25);
            this.btn_forward_nums.TabIndex = 10;
            this.btn_forward_nums.Text = "Подряд идущими";
            this.btn_forward_nums.UseVisualStyleBackColor = true;
            this.btn_forward_nums.Click += new System.EventHandler(this.btn_forward_nums_Click_1);
            // 
            // btn_random_nums
            // 
            this.btn_random_nums.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_random_nums.Location = new System.Drawing.Point(6, 69);
            this.btn_random_nums.Name = "btn_random_nums";
            this.btn_random_nums.Size = new System.Drawing.Size(188, 25);
            this.btn_random_nums.TabIndex = 9;
            this.btn_random_nums.Text = "Случайными";
            this.btn_random_nums.UseVisualStyleBackColor = true;
            this.btn_random_nums.Click += new System.EventHandler(this.btn_random_nums_Click_1);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox3.Location = new System.Drawing.Point(12, 182);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 58);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Введите значение Х";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(9, 21);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(185, 22);
            this.textBox1.TabIndex = 6;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btn_full_search);
            this.groupBox4.Controls.Add(this.btn_bin_search);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox4.Location = new System.Drawing.Point(12, 239);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 77);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Найти значение Х";
            // 
            // btn_full_search
            // 
            this.btn_full_search.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_full_search.Location = new System.Drawing.Point(6, 21);
            this.btn_full_search.Name = "btn_full_search";
            this.btn_full_search.Size = new System.Drawing.Size(188, 25);
            this.btn_full_search.TabIndex = 8;
            this.btn_full_search.Text = "Полным перебором";
            this.btn_full_search.UseVisualStyleBackColor = true;
            this.btn_full_search.Click += new System.EventHandler(this.btn_full_search_Click_1);
            // 
            // btn_bin_search
            // 
            this.btn_bin_search.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_bin_search.Location = new System.Drawing.Point(6, 46);
            this.btn_bin_search.Name = "btn_bin_search";
            this.btn_bin_search.Size = new System.Drawing.Size(188, 25);
            this.btn_bin_search.TabIndex = 7;
            this.btn_bin_search.Text = "Бинарным поиском";
            this.btn_bin_search.UseVisualStyleBackColor = true;
            this.btn_bin_search.Click += new System.EventHandler(this.btn_bin_search_Click_1);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lbl_comp);
            this.groupBox5.Controls.Add(this.lbl_state);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox5.Location = new System.Drawing.Point(12, 322);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(200, 52);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Результат поиска Х";
            // 
            // lbl_comp
            // 
            this.lbl_comp.AutoSize = true;
            this.lbl_comp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_comp.Location = new System.Drawing.Point(6, 31);
            this.lbl_comp.Name = "lbl_comp";
            this.lbl_comp.Size = new System.Drawing.Size(104, 13);
            this.lbl_comp.TabIndex = 6;
            this.lbl_comp.Text = "Кол-во сравнений: ";
            // 
            // lbl_state
            // 
            this.lbl_state.AutoSize = true;
            this.lbl_state.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_state.Location = new System.Drawing.Point(6, 18);
            this.lbl_state.Name = "lbl_state";
            this.lbl_state.Size = new System.Drawing.Size(57, 13);
            this.lbl_state.TabIndex = 5;
            this.lbl_state.Text = "Элемент: ";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btn_save_gisto);
            this.groupBox6.Controls.Add(this.btn_gisto);
            this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox6.Location = new System.Drawing.Point(12, 380);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(200, 97);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Гистограммы";
            // 
            // btn_save_gisto
            // 
            this.btn_save_gisto.Location = new System.Drawing.Point(6, 58);
            this.btn_save_gisto.Name = "btn_save_gisto";
            this.btn_save_gisto.Size = new System.Drawing.Size(188, 31);
            this.btn_save_gisto.TabIndex = 7;
            this.btn_save_gisto.Text = "Сохранить гистограммы";
            this.btn_save_gisto.UseVisualStyleBackColor = true;
            this.btn_save_gisto.Click += new System.EventHandler(this.btn_save_gisto_Click);
            // 
            // btn_gisto
            // 
            this.btn_gisto.Location = new System.Drawing.Point(6, 21);
            this.btn_gisto.Name = "btn_gisto";
            this.btn_gisto.Size = new System.Drawing.Size(188, 31);
            this.btn_gisto.TabIndex = 6;
            this.btn_gisto.Text = "Построить гистограммы";
            this.btn_gisto.UseVisualStyleBackColor = true;
            this.btn_gisto.Click += new System.EventHandler(this.btn_gisto_Click_1);
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.DarkGray;
            this.chart1.BorderlineColor = System.Drawing.Color.Black;
            chartArea1.AxisX.LabelAutoFitMaxFontSize = 15;
            chartArea1.AxisX.LabelAutoFitMinFontSize = 7;
            chartArea1.AxisX.LabelStyle.Interval = 59D;
            chartArea1.AxisX.LabelStyle.IntervalOffset = 1D;
            chartArea1.AxisX.MajorGrid.Interval = 59D;
            chartArea1.AxisX.MajorGrid.IntervalOffset = 1D;
            chartArea1.AxisX.MajorTickMark.Interval = 59D;
            chartArea1.AxisX.MajorTickMark.IntervalOffset = 1D;
            chartArea1.AxisX.Title = "индексы элементов";
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            chartArea1.AxisY.LabelAutoFitMaxFontSize = 15;
            chartArea1.AxisY.Title = "сравнения";
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            legend1.IsTextAutoFit = false;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(232, 12);
            this.chart1.Name = "chart1";
            this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(1420, 314);
            this.chart1.TabIndex = 6;
            this.chart1.Text = "chart1";
            // 
            // chart2
            // 
            this.chart2.BackColor = System.Drawing.Color.DarkGray;
            chartArea2.AxisX.LabelAutoFitMaxFontSize = 15;
            chartArea2.AxisX.LabelAutoFitMinFontSize = 7;
            chartArea2.AxisX.LabelStyle.Interval = 59D;
            chartArea2.AxisX.LabelStyle.IntervalOffset = 1D;
            chartArea2.AxisX.MajorGrid.Interval = 59D;
            chartArea2.AxisX.MajorGrid.IntervalOffset = 1D;
            chartArea2.AxisX.MajorTickMark.Interval = 59D;
            chartArea2.AxisX.MajorTickMark.IntervalOffset = 1D;
            chartArea2.AxisX.Title = "индексы элементов";
            chartArea2.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            chartArea2.AxisY.LabelAutoFitMaxFontSize = 15;
            chartArea2.AxisY.Title = "сравнения";
            chartArea2.AxisY.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            chartArea2.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea2);
            legend2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            legend2.IsTextAutoFit = false;
            legend2.Name = "Legend1";
            this.chart2.Legends.Add(legend2);
            this.chart2.Location = new System.Drawing.Point(232, 322);
            this.chart2.Name = "chart2";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart2.Series.Add(series2);
            this.chart2.Size = new System.Drawing.Size(1420, 314);
            this.chart2.TabIndex = 7;
            this.chart2.Text = "chart2";
            // 
            // chart3
            // 
            this.chart3.BackColor = System.Drawing.Color.DarkGray;
            chartArea3.AxisX.LabelAutoFitMaxFontSize = 15;
            chartArea3.AxisX.LabelAutoFitMinFontSize = 7;
            chartArea3.AxisX.LabelStyle.Interval = 59D;
            chartArea3.AxisX.LabelStyle.IntervalOffset = 1D;
            chartArea3.AxisX.MajorGrid.Interval = 59D;
            chartArea3.AxisX.MajorGrid.IntervalOffset = 1D;
            chartArea3.AxisX.MajorTickMark.Interval = 59D;
            chartArea3.AxisX.MajorTickMark.IntervalOffset = 1D;
            chartArea3.AxisX.MinorGrid.Interval = 60D;
            chartArea3.AxisX.Title = "индексы элементов";
            chartArea3.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            chartArea3.AxisY.LabelAutoFitMaxFontSize = 15;
            chartArea3.AxisY.Title = "сравнения";
            chartArea3.AxisY.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            chartArea3.Name = "ChartArea1";
            this.chart3.ChartAreas.Add(chartArea3);
            legend3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            legend3.IsTextAutoFit = false;
            legend3.Name = "Legend1";
            this.chart3.Legends.Add(legend3);
            this.chart3.Location = new System.Drawing.Point(232, 635);
            this.chart3.Name = "chart3";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chart3.Series.Add(series3);
            this.chart3.Size = new System.Drawing.Size(1420, 314);
            this.chart3.TabIndex = 8;
            this.chart3.Text = "chart3";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1664, 961);
            this.Controls.Add(this.chart3);
            this.Controls.Add(this.chart2);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbl_content;
        private System.Windows.Forms.Label lbl_n;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btn_full_search;
        private System.Windows.Forms.Button btn_bin_search;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label lbl_comp;
        private System.Windows.Forms.Label lbl_state;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btn_gisto;
        private System.Windows.Forms.Button btn_random_nums;
        private System.Windows.Forms.Button btn_forward_nums;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart3;
        private System.Windows.Forms.Button btn_save_gisto;
    }
}

