using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace BITTcheckOIB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            listBox1.Items.Add("uat");
            listBox1.Items.Add("env7");
            listBox1.Items.Add("env6");

            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].Name = "OIB";
            dataGridView1.Columns[1].Name = "Environtment";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            int a = Int32.Parse(maskedTextBox1.Text);
            string b = listBox1.Text;
            int i = 1;
            do
            {
                long randnum2 = (long)(rand.NextDouble() * 9000000000) + 1000000000;
                string oib11 = randnum2.ToString();
                string oibtest = CalculateOIBCheckDigit(oib11);
                var index = this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[index].Cells[0].Value = oibtest;
                this.dataGridView1.Rows[index].Cells[1].Value = b;
                //using (var fs = new FileStream(@"C:\\OIB\\oibtest.txt", FileMode.Append))

                //{
                //    using (StreamWriter writer = new StreamWriter("C:\\OIB\\oibtest3.txt", true))
                //    {


                //        writer.WriteLine(oibtest);




                //    }

                //}

                i++;

            } while (i < a);




        }

        public static string CalculateOIBCheckDigit(string oib)
        {
            if (string.IsNullOrEmpty(oib)) return "";
            if (oib.Length != 10) return "";

            long b;
            if (!long.TryParse(oib, out b)) return "";

            int a = 10;
            for (int i = 0; i < 10; i++)
            {
                a = a + Convert.ToInt32(oib.Substring(i, 1));
                a = a % 10;
                if (a == 0) a = 10;
                a *= 2;
                a = a % 11;
            }
            int kontrolni = 11 - a;
            if (kontrolni == 10) kontrolni = 0;

            return oib + kontrolni.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                sfd.FileName = "Output.csv";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            int columnCount = dataGridView1.Columns.Count;
                            string columnNames = "";
                            string[] outputCsv = new string[dataGridView1.Rows.Count + 1];
                            for (int i = 2; i < columnCount; i++)
                            {
                                columnNames += dataGridView1.Columns[i].HeaderText.ToString() + ",";
                            }
                            outputCsv[0] += columnNames;

                            for (int i = 1; i < dataGridView1.Rows.Count; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    outputCsv[i] += dataGridView1.Rows[i - 1].Cells[j].Value.ToString() + ",";
                                }
                            }

                            File.WriteAllLines(sfd.FileName, outputCsv, Encoding.UTF8);
                            MessageBox.Show("Data Exported Successfully !!!", "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record To Export !!!", "Info");
            }
        }
    }
    }

