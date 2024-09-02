using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace GameDataUploader
{
    
    public partial class Form1 : Form
    {
        private AppDBContext _repository;
        public Form1()
        {
            _repository = new AppDBContext();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            using (var package = new ExcelPackage(new FileInfo(textBox1.Text)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                List<Wall> walls = new List<Wall>();
                for (int i = 1; i < rowCount; i++)
                {
                    for (int j = 1; j < colCount; j++)
                    {
                        if (worksheet.Cells[i, j].Value.Equals("x"))
                        {
                            Wall wall = new Wall(){MapId = 1, X = j, Y = i};
                            walls.Add(wall);
                        }
                    }
                }
                
                _repository.map.AddRange(walls);
                _repository.SaveChanges();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                textBox1.Text = fileName;
            }
        }
    }
}