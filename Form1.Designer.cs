using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;

//Form1.designer

//using System;
//using System.Net.Http;
//using System.Text;
//using System.Windows.Forms;
//using System.Text.RegularExpressions;
//using System.IO;
//using Newtonsoft.Json;  // Install Newtonsoft.Json via NuGet
//using System.Security.Cryptography;

namespace NewApp
{
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

        public Form1()
        {
            // InitializeComponent();

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
            modelDetailComboBox.Items.Add("Select Model Detail ID"); // Placeholder text
            modelDetailComboBox.Items.Add("1 Trilux NEXT");
            modelDetailComboBox.Items.Add("2 BULKe Space");
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
            stateComboBox.Items.Add("Select State"); // Placeholder
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

            // Create and position the Submit button
            Button submitButton = new Button
            {
                Text = "Submit",
                Location = new System.Drawing.Point(280, 440),
                Width = 180,
                Height = 40
            };
            submitButton.Click += new EventHandler(submitButton_Click);
            this.Controls.Add(submitButton);

            // Create and position the Validate button
            Button validateButton = new Button
            {
                Text = "Validate",
                Location = new System.Drawing.Point(20, 440),
                Width = 180,
                Height = 40
            };
            validateButton.Click += new EventHandler(validateButton_Click);
            this.Controls.Add(validateButton);

            // Create and position the Decrypt button
            Button decryptButton = new Button
            {
                Text = "Decrypt",
                Location = new System.Drawing.Point(160, 500),
                Width = 180,
                Height = 40
            };
            //decryptButton.Click += new EventHandler(decryptButton_Click);
            this.Controls.Add(decryptButton);
        }

        private async void validateButton_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                MessageBox.Show("All inputs are valid.");
                await CallApiAndStoreResponse();
            }
            else
            {
                MessageBox.Show("There are validation errors.");
            }
        }

        private async System.Threading.Tasks.Task CallApiAndStoreResponse()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var payload = new
                    {
                        username = "Keto788603",
                        email = "info@ketomotors.com"
                    };

                    string jsonPayload = JsonConvert.SerializeObject(payload);
                    StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("https://empsuat.heavyindustries.gov.in/api/emps/token", content);

                    if (response.IsSuccessStatusCode)
                    {
                        token = await response.Content.ReadAsStringAsync();
                        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "key.txt");
                        File.WriteAllText(filePath, token);
                        MessageBox.Show("Token has been saved to file and stored in the application.");
                    }
                    else
                    {
                        MessageBox.Show($"Failed to get the token from the API. Status code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}\nInner Exception: {ex.InnerException?.Message}");
            }
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                // Ensure the selected dates are within the valid range before submission
                if (manufactDtPicker.Value < manufactDtPicker.MinDate || manufactDtPicker.Value > manufactDtPicker.MaxDate)
                {
                    MessageBox.Show("Manufacturing date is out of range.");
                    return;
                }

                if (invoiceDtPicker.Value < invoiceDtPicker.MinDate || invoiceDtPicker.Value > invoiceDtPicker.MaxDate)
                {
                    MessageBox.Show("Invoice date is out of range.");
                    return;
                }

                // Convert the ComboBox values to appropriate formats
                int modelDetailId = int.Parse(modelDetailComboBox.SelectedItem.ToString().Split(' ')[0]);
                int stateCode = int.Parse(stateComboBox.SelectedItem.ToString().Split(' ')[0]);

                // Collect form data into an anonymous object with the required format
                var formData = new
                {
                    model_detail_id = modelDetailId,
                    vin_chasis_no = chassisNumberTextBox.Text,
                    manufact_dt = manufactDtPicker.Value.ToString("dd-MM-yyyy"),
                    invoice_no = invoice_no.Text,
                    invoice_dt = invoiceDtPicker.Value.ToString("dd-MM-yyyy"),
                    seg_id = 1, // Set to 1 as per your initial requirement
                    cat_id = 1, // Set to 1 as per your initial requirement
                    state = stateCode,
                    pincode = pincode.Text,
                    customer_name = customerName.Text
                };

                // Serialize the form data to JSON
                string jsonData = JsonConvert.SerializeObject(formData);

                // Save the JSON data to formData.json before encryption
                string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "formData.json");
                File.WriteAllText(jsonFilePath, jsonData);

                MessageBox.Show($"Form data saved in JSON format at: {jsonFilePath}");

                // Encrypt the saved form data
                string encryptedData = EncryptData(jsonData, "dwggGKoWnhhhWY56ZuWudttLOIaMnHbt");

                // Save the encrypted data to a separate file
                string encryptedFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "encryptedData.txt");
                File.WriteAllText(encryptedFilePath, encryptedData);

                MessageBox.Show("Data has been encrypted and saved.");

                // Refresh the form after submission
                ResetForm();
            }
        }

        private void ResetForm()
        {
            // Reset the ComboBox selection to the placeholder item
            modelDetailComboBox.SelectedIndex = 0;

            // Clear all TextBox inputs
            chassisNumberTextBox.Clear();
            invoice_no.Clear();
            pincode.Clear();
            customerName.Clear();

            // Reset the DateTimePickers to default values
            manufactDtPicker.Value = manufactDtPicker.MinDate;
            invoiceDtPicker.Value = invoiceDtPicker.MinDate;

            // Reset the ComboBox for state selection to the placeholder item
            stateComboBox.SelectedIndex = 0;
        }



        private bool ValidateInputs()
        {
            // Check if the user has selected a valid model_detail_id from the ComboBox
            if (modelDetailComboBox.SelectedIndex == 0)
            {
                MessageBox.Show("Please select a valid Model Detail ID.");
                return false;
            }

            // Check if the user has entered a valid chassis number (must be alphanumeric and 17 characters long)
            if (!Regex.IsMatch(chassisNumberTextBox.Text, "^[a-zA-Z0-9]{17}$"))
            {
                MessageBox.Show("Please enter a valid 17-character alphanumeric chassis number.");
                return false;
            }

            // Check if the user has entered a valid invoice number (alphanumeric)
            if (!Regex.IsMatch(invoice_no.Text, "^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("Please enter a valid alphanumeric invoice number.");
                return false;
            }

            // Check if the user has entered a valid pincode (numeric, 6 digits)
            if (!Regex.IsMatch(pincode.Text, "^[0-9]{6}$"))
            {
                MessageBox.Show("Please enter a valid 6-digit pincode.");
                return false;
            }

            // Check if the user has entered a customer name (non-empty, alphanumeric)
            if (string.IsNullOrEmpty(customerName.Text))
            {
                MessageBox.Show("Please enter a customer name.");
                return false;
            }

            return true;
        }

        // Encryption method (existing)
        private string EncryptData(string plainText, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] ivBytes = new byte[16]; // AES requires a 16-byte IV
            Array.Clear(ivBytes, 0, ivBytes.Length); // Initialize IV with zeroes

            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = ivBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            cs.Write(plainBytes, 0, plainBytes.Length);
                            cs.FlushFinalBlock();

                            return Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }
            }
        }
        /*
        // Decryption method (new)
        private void decryptButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Path to the encrypted file
                string encryptedFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "encryptedData.txt");

                // Check if the file exists
                if (File.Exists(encryptedFilePath))
                {
                    // Read the encrypted data from the file
                    string encryptedData = File.ReadAllText(encryptedFilePath);

                    // Decrypt the data
                    string decryptedData = DecryptData(encryptedData, "dwggGKoWnhhhWY56ZuWudttLOIaMnHbt");

                    // Path to the decrypted file
                    string decryptedFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "decryptedData.txt");

                    // Write the decrypted data to a file
                    File.WriteAllText(decryptedFilePath, decryptedData);

                    // Show a message to indicate success
                    MessageBox.Show("Decrypted data has been saved to decryptedData.txt");
                }
                else
                {
                    MessageBox.Show("Encrypted file not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during decryption: {ex.Message}");
            }
        }

        // Decryption helper function (new)
        private string DecryptData(string encryptedText, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] ivBytes = new byte[16]; // AES requires a 16-byte IV
            Array.Clear(ivBytes, 0, ivBytes.Length); // Initialize IV with zeroes

            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = ivBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (MemoryStream ms = new MemoryStream(encryptedBytes))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                return sr.ReadToEnd(); // Return the decrypted data as a string
                            }
                        }
                    }
                }
            }
        }*/
    }
}