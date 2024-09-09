
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Text;

public partial class Form1 : Form
{
    // Array to store input data
    private string[] dataArray;

    // ComboBox and other input controls for user input
    private ComboBox modelDetailComboBox;
    private TextBox chassisNumberTextBox;
    private DateTimePicker manufactDtPicker;
    private TextBox invoice_no;
    private DateTimePicker invoiceDtPicker;
    private ComboBox stateComboBox;
    private TextBox pincode;
    private TextBox customerName;

    // This variable will store the API token
    private string token;

    // Submit button reference
    private Button submitButton;
    public class SaleData
    {
        public int model_detail_id { get; set; }
        public string vin_chasis_no { get; set; }
        public string manufact_dt { get; set; }
        public string invoice_no { get; set; }
        public string invoice_dt { get; set; }
        public int seg_id { get; set; }
        public int cat_id { get; set; }
        public int state { get; set; }
        public int pincode { get; set; }
        public string customer_name { get; set; }
    }

    public class SaleDataWrapper
    {
        public List<SaleData> saleData { get; set; }
    }


    public Form1()
    {
        //  InitializeComponent();

        // Set the background image for the form
        this.BackgroundImage = System.Drawing.Image.FromFile(@"C:\Users\hp\Downloads\bg3.jpeg");
        this.BackgroundImageLayout = ImageLayout.Stretch;

        // Initialize the array with a fixed size (8 elements for the input fields)
        dataArray = new string[8];

        // Initialize the ComboBox for model_detail_id with placeholder text
        modelDetailComboBox = new ComboBox
        {
            Location = new System.Drawing.Point(120, 100),
            Width = 200,
            DropDownStyle = ComboBoxStyle.DropDownList // Prevent typing custom values
        };
        modelDetailComboBox.Items.Add("model_detail_id"); // Placeholder text
        modelDetailComboBox.Items.Add("1 TRILUX NEXT");
        modelDetailComboBox.Items.Add("2 BULKE SPACE");
      //  modelDetailComboBox.Items.Add("3");
        modelDetailComboBox.SelectedIndex = 0; // Set placeholder as default selection

        chassisNumberTextBox = new TextBox { Location = new System.Drawing.Point(120, 140), Width = 200, PlaceholderText = "Chassis Number" };

        manufactDtPicker = new DateTimePicker
        {
            Location = new System.Drawing.Point(120, 180),
            Width = 200,
            Format = DateTimePickerFormat.Custom, // Custom date format
            CustomFormat = "dd-MM-yyyy",
            MaxDate = DateTime.Now,
            MinDate = new DateTime(2000, 1, 1) // Set a reasonable lower bound
        };

        invoice_no = new TextBox { Location = new System.Drawing.Point(120, 220), Width = 200, PlaceholderText = "Invoice No" };

        invoiceDtPicker = new DateTimePicker
        {
            Location = new System.Drawing.Point(120, 260),
            Width = 200,
            Format = DateTimePickerFormat.Custom, // Custom date format
            CustomFormat = "dd-MM-yyyy",
            MaxDate = DateTime.Now,
            MinDate = new DateTime(2000, 1, 1) // Set a reasonable lower bound
        };

        stateComboBox = new ComboBox
        {
            Location = new System.Drawing.Point(120, 300),
            Width = 200,
            DropDownStyle = ComboBoxStyle.DropDownList // Prevent typing custom values
        };
        stateComboBox.Items.Add("State"); // Placeholder
        stateComboBox.SelectedIndex = 0;
        stateComboBox.Items.AddRange(new string[]
        {
            "1 ANDAMAN & NICOBAR ISLANDS",
            "2 ANDHRA PRADESH",
            "3 ARUNACHAL PRADESH",
            "4 ASSAM",
            "5 BIHAR",
            "6 CHANDIGARH",
            "7 CHATTISGARH",
            "8 DADRA & NAGAR HAVELI",
            "9 DAMAN & DIU",
            "10 DELHI",
            "11 GOA",
            "12 GUJARAT",
            "13 HARYANA",
            "14 HIMACHAL PRADESH",
            "15 JAMMU & KASHMIR",
            "16 JHARKHAND",
            "17 KARNATAKA",
            "18 KERALA",
            "19 LAKSHADWEEP",
            "20 MADHYA PRADESH",
            "21 MAHARASHTRA",
            "22 MANIPUR",
            "23 MEGHALAYA",
            "24 MIZORAM",
            "25 NAGALAND",
            "26 ODISHA",
            "27 PONDICHERRY",
            "28 PUNJAB",
            "29 RAJASTHAN",
            "30 SIKKIM",
            "31 TAMIL NADU",
            "32 TELANGANA",
            "33 TRIPURA",
            "34 UTTARAKHAND",
            "35 UTTAR PRADESH",
            "36 WEST BENGAL"
        });

        pincode = new TextBox { Location = new System.Drawing.Point(120, 340), Width = 200, PlaceholderText = "Pincode" };
        customerName = new TextBox { Location = new System.Drawing.Point(120, 380), Width = 200, PlaceholderText = "Customer Name" };

        // Add controls to the form
        this.Controls.Add(modelDetailComboBox);
        this.Controls.Add(chassisNumberTextBox);
        this.Controls.Add(manufactDtPicker);
        this.Controls.Add(invoice_no);
        this.Controls.Add(invoiceDtPicker);
        this.Controls.Add(stateComboBox);
        this.Controls.Add(pincode);
        this.Controls.Add(customerName);

        // Create and position the Submit button (initially disabled)
        submitButton = new Button
        {
            Text = "Submit",
            Location = new System.Drawing.Point(20, 440),
            Width = 180,
            Height = 40,
            Enabled = false // Disabled until validation
        };
        submitButton.Click += new EventHandler(submitButton_Click);
        this.Controls.Add(submitButton);

        // Create and position the Validate button
        Button validateButton = new Button
        {
            Text = "Validate",
            Location = new System.Drawing.Point(280, 440),
            Width = 180,
            Height = 40
        };
        validateButton.Click += new EventHandler(validateButton_Click);
        this.Controls.Add(validateButton);
    }

    private void validateButton_Click(object sender, EventArgs e)
    {
        if (ValidateInputs())
        {
            MessageBox.Show("All inputs are valid.");
            submitButton.Enabled = true; // Enable the Submit button after successful validation
        }
        else
        {
            submitButton.Enabled = false; // Keep Submit button disabled if validation fails
        }
    }

    private void submitButton_Click(object sender, EventArgs e)
    {
        // Submit the data after validation is successful
        MessageBox.Show("Submitting data...");
        ExecuteFormOperations();
    }


    private bool ValidateInputs()
    {
        if (modelDetailComboBox.SelectedIndex == 0)
        {
            MessageBox.Show("Please select a valid Model Detail ID.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(chassisNumberTextBox.Text) || !Regex.IsMatch(chassisNumberTextBox.Text, "^[a-zA-Z0-9]+$"))
        {
            MessageBox.Show("Chassis number is invalid. Only alphanumeric characters are allowed.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(invoice_no.Text) || !Regex.IsMatch(invoice_no.Text, "^[a-zA-Z0-9]+$"))
        {
            MessageBox.Show("Invoice number is invalid. Only alphanumeric characters are allowed.");
            return false;
        }

        if (stateComboBox.SelectedIndex == 0)
        {
            MessageBox.Show("Please select a valid state.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(pincode.Text) || !Regex.IsMatch(pincode.Text, "^[1-9][0-9]{5}$"))
        {
            MessageBox.Show("Pincode is invalid. It should be a 6-digit number.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(customerName.Text))
        {
            MessageBox.Show("Customer name is required.");
            return false;
        }

        return true;
    }

    private void ExecuteFormOperations()
    {
        try
        {
            // After validation, call the API and store the response
            CallApiAndStoreResponse().Wait();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred: {ex.Message}");
        }
    }

    private async System.Threading.Tasks.Task CallApiAndStoreResponse()
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                var payload = new { username = "Keto788603", email = "info@ketomotors.com" };
                string jsonPayload = JsonConvert.SerializeObject(payload);
                StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("https://empsuat.heavyindustries.gov.in/api/emps/token", content);

                if (response.IsSuccessStatusCode)
                {
                    token = await response.Content.ReadAsStringAsync();
                    File.WriteAllText(@"key.txt", token);
                    MessageBox.Show("Token saved to key.txt and stored in memory.");
                }
                else
                {
                    MessageBox.Show("API call failed.");
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error in API call: {ex.Message}");
        }
    }


    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
        SuspendLayout();
        // 
        // Form1
        // 
        ClientSize = new Size(1192, 401);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Name = "Form1";
        ResumeLayout(false);
    }
}
