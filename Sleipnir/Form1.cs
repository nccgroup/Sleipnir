using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace Sleipnir
{
    public partial class Sleipnir : Form
    {
        public Sleipnir()
        {
            InitializeComponent();             
        }


        // Create a handle for writing to
        public IntPtr handle = WTSapi32.WTSVirtualChannelOpen(IntPtr.Zero, -1, "Loki1");

        // The read method, reads data from the virtual channel
        private void read(out byte[] data)
        {
            byte[] readInData = new byte[1536];
            GCHandle pinned = GCHandle.Alloc(readInData, GCHandleType.Pinned);
            IntPtr address = pinned.AddrOfPinnedObject();

            uint bytesread = 0;
            int returnValue = WTSapi32.WTSVirtualChannelRead(handle, int.MaxValue, readInData, 1536, out bytesread);
            pinned.Free();

            byte[] correct = new byte[bytesread];
            Array.Copy(readInData, correct, bytesread);
            data = correct;
            Array.Clear(readInData, 0, readInData.Length);
            Application.DoEvents();
        }

        private void buttonReceiveFile_Click(object sender, EventArgs e)
        {
            //Get the title
            byte[] title;
            read(out title);
            string sTitle = "";
            foreach (byte b in title)
            {
                if (b != 0)
                {
                    char c = Convert.ToChar(b);
                    sTitle += c;
                }
                else
                {
                }
                Application.DoEvents();
            }

            //Get the file size
            byte[] bFileSize;
            read(out bFileSize);
            string sFileSize = "";
            foreach (byte b in bFileSize)
            {
                if (b != 0)
                {
                    char c = Convert.ToChar(b);
                    sFileSize += c;
                }
                else
                {
                }
                Application.DoEvents();
            }
            int iFileSize = Convert.ToInt32(sFileSize);

            // Display the ProgressBar control.
            progressBarSleipnir.Visible = true;
            // Set Minimum to 1 to represent the first char being sent.
            progressBarSleipnir.Minimum = 1;
            // Set Maximum to the total number of chars to be sent.
            progressBarSleipnir.Maximum = iFileSize;
            // Set the initial value of the ProgressBar.
            progressBarSleipnir.Value = 1;
            // Set the Step property to a value of 1 to represent each char being sent.
            progressBarSleipnir.Step = 1024;

            //create the file
            using (FileStream writeStream = new FileStream(sTitle, FileMode.Create, FileAccess.Write))
            {
                //Write the file
                while (true)
                {
                    byte[] received;
                    read(out received);
                    progressBarSleipnir.PerformStep();
                    string base64data = "";

                    foreach (byte a in received)
                    {

                        char c = Convert.ToChar(a);

                        if (c != 0)
                        {
                            base64data += c;
                        }
                    }

                    byte[] decoded;

                    // check if the whole file has been sent.
                    if (base64data == "!!!")
                    {
                        MessageBox.Show("File successfully transferred!");
                        break;
                    }

                    // write the transfered file
                    else
                    {
                        decoded = Convert.FromBase64String(base64data);
                        writeStream.Write(decoded, 0, decoded.Length);
                        writeStream.Flush();
                        
                    }
                }
                writeStream.Flush();
                writeStream.Close();
            }   


        }

        private void buttonSendFile_Click(object sender, EventArgs e)
        {
            openFDToSendVC.Title = "Choose a file to transmit";
            openFDToSendVC.FileName = "";
            openFDToSendVC.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFDToSendVC.Filter = "All Files|*.*";

            string path = "";

            if (openFDToSendVC.ShowDialog() != DialogResult.Cancel)
            {
                path = openFDToSendVC.FileName;
            }

            try
            {
                
                
                // Send the file title
                string filename = Path.GetFileName(path);
                string sEncode = System.Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes("title:" + filename)); //to be used for base64 encoding
                byte[] bData = System.Text.Encoding.Unicode.GetBytes(sEncode);
                int bytesWritten = 0;
                WTSapi32.WTSVirtualChannelWrite(handle, bData, bData.Length, ref bytesWritten);

                FileInfo fi = new FileInfo(path);
                // Display the ProgressBar control.
                progressBarSleipnir.Visible = true;
                // Set Minimum to 1 to represent the first char being sent.
                progressBarSleipnir.Minimum = 1;
                // Set Maximum to the file size.
                progressBarSleipnir.Maximum = (int)fi.Length;
                // Set the initial value of the ProgressBar.
                progressBarSleipnir.Value = 1;
                // Set the Step property to a value of 1024 to represent each each buffer being read.
                progressBarSleipnir.Step = 1024;

                using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                
                using (var stream = new MemoryStream())
                {
                    byte[] buffer = new byte[1024]; // read in chunks of 1KB

                    int bytesRead = 0;

                    while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        byte[] title;
                        read(out title);
                        string sReceived = "";
                        foreach (byte b in title)
                        {
                            if (b != 0)
                            {
                                char c = Convert.ToChar(b);
                                sReceived += c;
                            }
                            else
                            {
                            }
                        }
                        if (sReceived == "Received")
                        {
                            string sEncoded = System.Convert.ToBase64String(buffer);
                            byte[] bEncoded = System.Text.Encoding.Unicode.GetBytes(sEncoded);
                            WTSapi32.WTSVirtualChannelWrite(handle, bEncoded, bEncoded.Length, ref bytesWritten);
                            Array.Clear(buffer, 0, buffer.Length);
                            Application.DoEvents();
                            progressBarSleipnir.PerformStep();
                            
                        }
                        else if (sReceived == "Cancelled")
                        {
                            Array.Clear(buffer, 0, buffer.Length);
                            break;
                            
                        }
                        sReceived = "";
                        Array.Clear(buffer, 0, buffer.Length);
                    }
                    byte[] terminator = System.Text.Encoding.Unicode.GetBytes(System.Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes("!!!")));
                    WTSapi32.WTSVirtualChannelWrite(handle, terminator, terminator.Length, ref bytesWritten);

                    Array.Clear(buffer, 0, buffer.Length);
                }

            }
            catch (Exception error)
            {
                Console.WriteLine("The process failed: {0}", error.ToString());
            }
        }

        private void openFDToSendVC_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
