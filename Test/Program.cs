using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Test;

class Server
{
    private static List<AuktionsItem> auctionItems = new List<AuktionsItem>();
    private static Dictionary<int, List<Bid>> itemBids = new Dictionary<int, List<Bid>>();
    private static int itemIdCounter = 1;

    static void Main()
    {
        TcpListener server = null;
        try
        {
            // Lyt på IP-adresse og port
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 12345;
            server = new TcpListener(ipAddress, port);

            // Start serveren
            server.Start();
            Console.WriteLine("Serveren venter på forbindelser...");

            while (true)
            {
                using (TcpClient client = server.AcceptTcpClient())
                using (NetworkStream stream = client.GetStream())
                {
                    Console.WriteLine("Forbundet til klient: " + ((IPEndPoint)client.Client.RemoteEndPoint).Address);

                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        string request = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        Console.WriteLine("Modtaget fra klient: " + request);

                        string response = HandleRequest(request);

                        // Send svar til klienten
                        byte[] responseBytes = Encoding.ASCII.GetBytes(response);
                        stream.Write(responseBytes, 0, responseBytes.Length);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Fejl: " + e.Message);
        }
        finally
        {
            server.Stop();
        }
    }

    private static string HandleRequest(string request)
    {
        string[] tokens = request.Split(' ');
        string command = tokens[0].ToLower();

        switch (command)
        {
            case "create":
                if (tokens.Length >= 4)
                {
                    string name = tokens[1];
                    decimal minimumPrice = decimal.Parse(tokens[2]);
                    int maxAuctionDuration = int.Parse(tokens[3]);
                    AuktionsItem item = new AuktionsItem(name, minimumPrice, maxAuctionDuration);
                    auctionItems.Add(item);
                    itemBids[itemIdCounter] = new List<Bid>();
                    itemIdCounter++;
                    return "Auktionsvaren er oprettet.";
                }
                return "Ugyldig kommando: create <navn> <minimumspris> <maksimal længde>";

            case "bid":
                if (tokens.Length >= 3)
                {
                    int itemId = int.Parse(tokens[1]);
                    decimal bidAmount = decimal.Parse(tokens[2]);

                    if (itemBids.ContainsKey(itemId))
                    {
                        AuktionsItem item = auctionItems[itemId - 1];
                        if (item.SalePrice == 0.00m && bidAmount > item.MinimumPrice)
                        {
                            Bid bid = new Bid(bidAmount);
                            itemBids[itemId].Add(bid);
                            item.SalePrice = bidAmount;
                            return "Dit bud er blevet accepteret.";
                        }
                        else
                        {
                            return "Budet kunne ikke accepteres.";
                        }
                    }
                    else
                    {
                        return "Ugyldigt vare-ID.";
                    }
                }
                return "Ugyldig kommando: bid <vare-ID> <bud>";

            case "list":
                StringBuilder listResponse = new StringBuilder();
                foreach (var item in auctionItems)
                {
                    listResponse.AppendLine($"Vare-ID: {auctionItems.IndexOf(item) + 1}");
                    listResponse.AppendLine($"Navn: {item.Name}");
                    listResponse.AppendLine($"Minimumspris: {item.MinimumPrice:C}");
                    listResponse.AppendLine($"Nuværende højeste bud: {item.SalePrice:C}");
                    listResponse.AppendLine($"Tid tilbage: {item.MaxAuctionDuration - (DateTime.Now - item.StartTime).TotalSeconds:F} sekunder");
                    listResponse.AppendLine();
                }
                return listResponse.ToString();

            default:
                return "Ukendt kommando.";
        }
    }
}
