namespace TestClient
{
    using System;
    using System.Net.Sockets;
    using System.Text;

    class Client
    {
        static void Main()
        {
            TcpClient client = new TcpClient("127.0.0.1", 12345);
            NetworkStream stream = client.GetStream();

            while (true)
            {
                Console.WriteLine("Tilgængelige kommandoer:");
                Console.WriteLine("1. create <navn> <minimumspris> <maksimal længde>");
                Console.WriteLine("2. bid <vare-ID> <bud>");
                Console.WriteLine("3. list");
                Console.WriteLine("4. exit");
                Console.Write("Skriv din kommando: ");

                string user_input = Console.ReadLine();

                if (user_input.ToLower() == "exit")
                    break; // Luk klienten

                byte[] data = Encoding.ASCII.GetBytes(user_input);

                // Send brugerinput til serveren
                stream.Write(data, 0, data.Length);

                // Modtag og udskriv svar fra serveren
                byte[] responseBuffer = new byte[1024];
                int bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);
                string response = Encoding.ASCII.GetString(responseBuffer, 0, bytesRead);
                Console.WriteLine("Serveren siger: " + response);
            }

            // Luk klienten
            client.Close();
        }
    }
}