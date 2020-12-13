using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using System.IO;
using Azure.Storage.Blobs.Models;

public class QMessageHandler 
{
     
    public static async Task ReceiveMessagesAsync()
    {
        string connectionString = "Endpoint=sb://demobusk8.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Xpmhbbn6TwtKl0agPsIeqCX6z2ZI29UAcWzx7zwxaWk=";
        string queueName = "queuecityworksnet";

        await using (ServiceBusClient client = new ServiceBusClient(connectionString))
        {
            // create a processor that we can use to process the messages
            ServiceBusProcessor processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

            // add handler to process messages
            processor.ProcessMessageAsync += MessageHandler;

            // add handler to process any errors
            processor.ProcessErrorAsync += ErrorHandler;

            // start processing 
            await processor.StartProcessingAsync();
            Console.WriteLine("Wait for a minute and then press any key to end the processing");
            Console.ReadKey();

            // stop processing 
            Console.WriteLine("\nStopping the receiver...");
            await processor.StopProcessingAsync();
            Console.WriteLine("Stopped receiving messages");
        }     
    }

    static List<KeyValuePair<string, int>> cityCountList = new List<KeyValuePair<string, int>> ();

    // handle received messages
    static async Task MessageHandler(ProcessMessageEventArgs args)
    {
        string body = args.Message.Body.ToString();
         
        Console.WriteLine($"Received Message: {body}");
        KeyValuePair<string, int> oldCC = new KeyValuePair<string, int> ();
        KeyValuePair<string, int> newCC = new KeyValuePair<string, int> ();
        bool found = false;

        foreach (var CityCount in cityCountList)
        {
            if(CityCount.Key==body)
            {
                int currentCount = ((int)CityCount.Value);
                oldCC = CityCount;
                newCC = new KeyValuePair<string, int>(body, ++currentCount);
                string newerx = newCC.Key + ":" + newCC.Value;
                Console.WriteLine("\n oldetimer...", newerx);
                found = true;
            }            
        }
        if(!found)
        {
            newCC = new KeyValuePair<string, int>(body, 1);
            string newerx = newCC.Key + ":" + newCC.Value;
            Console.WriteLine("\n firsttimer... " + body);
        }

        cityCountList.Remove(oldCC);
        cityCountList.Add(newCC);
        string older = oldCC.Key + ":" + oldCC.Value;
        Console.WriteLine("\n older... " + older);
        string newer = newCC.Key + ":" + newCC.Value;
        Console.WriteLine("\n newer... " + newer);
        await UploadMessageAsync();

        // complete the message. messages is deleted from the queue. 
        await args.CompleteMessageAsync(args.Message);
    }

    // handle any errors when receiving messages
    static Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }

    static async Task UploadMessageAsync()
    {
        string jsonFileContents = CreateJsonFileContents(); 

        // Create a BlobServiceClient object which will be used to create a container client
        BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=storagedemok8;AccountKey=jzBHw2//lbnOu3UDGZVabkUkYSft+rDd+bvjif3Bvn/jzlTNF5pAsEyjhygpS1C49/0H88uLXeGEqX2IlcgbzQ==;EndpointSuffix=core.windows.net");

        //Create a unique name for the container
        string containerName = "processcontainer";

        // Create the container and return a container client object
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

        byte[] fileContent = Encoding.ASCII.GetBytes(jsonFileContents);
        MemoryStream ms = new MemoryStream(fileContent);

        // Get a reference to a blob
        BlobClient blobClient = containerClient.GetBlobClient("score.json");

        Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);
         
        await blobClient.UploadAsync(ms, true); 
    }

    static string CreateJsonFileContents()
    {
        StringBuilder sb = new StringBuilder();
        
        foreach (var CityCount in cityCountList)
        {
            string item = "{ \"" +  CityCount.Key + "\" : \"" + CityCount.Value.ToString() + "\" }" ;
            sb.Append(item);
        }
        Console.WriteLine("\n" + sb.ToString());
        return sb.ToString();
    }

}