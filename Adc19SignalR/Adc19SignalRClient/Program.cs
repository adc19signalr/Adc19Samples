using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace Adc19SignalRClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hub Verbindung wird aufgebaut...");

            var url = "http://localhost:54394/adcHub";

            // alternativ: HttpTransportType.WebSockets, HttpTransportType.ServerSentEvents, HttpTransportType.LongPolling
            var transportType = HttpTransportType.None; ;

            var hubConnection = transportType == HttpTransportType.None ?
                new HubConnectionBuilder()
                .WithUrl(url)
                .Build() :
                new HubConnectionBuilder()
                .WithUrl(url, transportType)
                .Build();

            hubConnection.On<string>("EmpfangeNachricht", message => EmpfangeNachricht(message));

            try
            {
                hubConnection.StartAsync().Wait();

                var running = true;

                while (running)
                {
                    var message = string.Empty;
                    var groupName = string.Empty;

                    Console.WriteLine("Bitte eine Aktion auswählen:");
                    Console.WriteLine("0 - Broadcast to all");
                    Console.WriteLine("1 - Send to itself");
                    Console.WriteLine("2 - Send to others");
                    Console.WriteLine("3 - Send to a group");
                    Console.WriteLine("4 - Add user to a group");
                    Console.WriteLine("5 - Remove user from a group");
                    Console.WriteLine("exit - Exit the program");

                    var action = Console.ReadLine();

                    if (action == "0" || action == "1" || action == "2" || action == "3")
                    {
                        Console.WriteLine("Bitte Nachricht eingeben:");
                        message = Console.ReadLine();
                    }

                    if (action == "3" || action == "4" || action == "5")
                    {
                        Console.WriteLine("Bitte Gruppennamen angeben:");
                        groupName = Console.ReadLine();
                    }

                    switch (action)
                    {
                        case "0":
                            hubConnection.SendAsync("BroadcastMessage", message).Wait();
                            break;
                        case "1":
                            hubConnection.SendAsync("SendToCaller", message).Wait();
                            break;
                        case "2":
                            hubConnection.SendAsync("SendToOthers", message).Wait();
                            break;
                        case "3":
                            hubConnection.SendAsync("SendToGroup", groupName, message).Wait();
                            break;
                        case "4":
                            hubConnection.SendAsync("AddUserToGroup", groupName).Wait();
                            break;
                        case "5":
                            hubConnection.SendAsync("RemoveUserFromGroup", groupName).Wait();
                            break;
                        case "exit":
                            running = false;
                            break;
                        default:
                            Console.WriteLine("Ungültige Aktion gewählt");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Drücken Sie eine Taste zum Verlassen der Anwendung...");
                Console.ReadKey();
                return;
            }

            void EmpfangeNachricht(string msg)
            {
                Console.WriteLine($"SignalR Hub Nachricht: {msg}");
            }
        }
    }
}
