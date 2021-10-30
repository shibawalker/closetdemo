using System;
using System.IO.Ports;


//Fields
List<string> myReceivedLines = new List<string>();


//subscriber method for the port.DataReceived Event
private void DataReceivedHandler(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
{
    SerialPort sp = (SerialPort)sender;
    while (sp.BytesToRead > 0)
    {
        try
        {
            myReceivedLines.Add(sp.ReadLine());
        }
        catch (TimeoutException)
        {
            break;
        }
    }
}


protected override void SolveInstance(IGH_DataAccess DA)
{

    string selectedportname = default(string);
    DA.GetData(1, ref selectedportname);
    int selectedbaudrate = default(int);
    DA.GetData(2, ref selectedbaudrate);
    bool connecttodevice = default(bool);
    DA.GetData(3, ref connecttodevice);
    bool homeall = default(bool);
    DA.GetData(5, ref homeall);

    SerialPort port = new SerialPort(selectedportname, selectedbaudrate, Parity.None, 8, StopBits.One);

    port.DtrEnable = true;
    port.Open();

    if (connecttodevice == true)
    {
        port.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
        DA.SetDataList(0, myReceivedLines);
    }

    if (homeall == true)
    {
        port.Write("g28");
    }

}
