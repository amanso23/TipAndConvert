using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TipAndConvert.Logic;
using TipAndConvert.Services;
namespace TipAndConvert.Forms
{
    public partial class MainForm : Form
    {

        private TextBox totalTextBox;
        private ComboBox percentageComboBox;
        private Button calculateButton;
        private Label resultLabel;
        private ComboBox currencyComboBox;
        private readonly CurrencyConvertService _converterService = new();
       

        public MainForm()
        {
            InitializeComponent();
            InitializeUI(); 
        }

        private void InitializeUI()
        {
            this.Text = "TipAndConvert";
            this.Size = new System.Drawing.Size(400, 200);
            this.StartPosition = FormStartPosition.CenterScreen;

            totalTextBox = new TextBox()
            {
                PlaceholderText = "Total en EUR",
                Width = 120,
                Location = new System.Drawing.Point(20, 20)
            };

            percentageComboBox = new ComboBox()
            {
                Width = 80,
                Location = new System.Drawing.Point(160, 20),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            percentageComboBox.Items.AddRange(new string[] { "5%", "10%", "15%", "20%" });
            percentageComboBox.SelectedIndex = 0;
            

            calculateButton = new Button()
            {
                Text = "Calcular",
                Width = 100,
                Location = new System.Drawing.Point(260, 20)
            };

            calculateButton.Click += CalculateButton_Click!;

       
            resultLabel = new Label()
            {
                Text = "Propina: ",
                AutoSize = true,
                Location = new System.Drawing.Point(20, 100)
            };


            currencyComboBox = new ComboBox()
            {
                Width = 80,
                Location = new System.Drawing.Point(160, 70),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            currencyComboBox.Items.AddRange(new string[] { "USD", "GBP", "AED" , "BIF", "CRC" });
            currencyComboBox.SelectedIndex = 0;


            this.Controls.Add(totalTextBox);
            this.Controls.Add(percentageComboBox);
            this.Controls.Add(currencyComboBox);
            this.Controls.Add(calculateButton);
            this.Controls.Add(resultLabel);
        }

        private async void CalculateButton_Click(object sender, EventArgs e)
        {
            if (float.TryParse(totalTextBox.Text, out float total) &&
                percentageComboBox.SelectedItem is string selectedPercentageText && currencyComboBox.SelectedItem is String selectedCurrencyText)
            {
                float percentage = float.Parse(selectedPercentageText.TrimEnd('%'));

                try
                {
                    float tip = TipCalculator.CalculateTip(total, percentage);
          
                    float result = await _converterService.ConvertCurrencyAsync("EUR", selectedCurrencyText, tip);
                    resultLabel.Text = $"Propina: {tip:0.00} € | {selectedCurrencyText}: {(result):0.00}";
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message, "Error de cálculo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Introduce un total válido y selecciona un porcentaje.",
                    "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
