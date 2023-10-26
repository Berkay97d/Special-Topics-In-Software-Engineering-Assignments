using System;
using System.Collections.Generic;
using System.Linq;

namespace SpecialTopicsInSoftwareEngineeringLab2
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();

            var currentTime = 0;
            var simulationTime = 50;   // Simülasyon'un kaç saniye süreceği
            var totalEmptyQueueTime = 0;    // Simulasyon sırasında sıranın boş olduğu süre
            var totalNonemptyQueueTime = 0;     // Simülasyon sırasında sıranın dolu olduğu süre

            var totalCustomerNumber = random.Next(1, 8);    //Simülasyon sırasonda kaç adet müşteri geleceği
            int[] arriveTimesForCustomers = new int[8];     // Simülasyon sırasında gelecek müşterilerin gelme zamanlarını tutan array
            List<Customer> queue = new List<Customer>();    
            bool isBusy = false;
            int busyFinishTime = 0;   //Server meşgul olmaya başladğında işini ne zaman bitireceğini tutan değer
            
            float P0;
            float P1;

            
            for (int i = 0; i < totalCustomerNumber; i++)
            {
                arriveTimesForCustomers[i] = random.Next(1, simulationTime);     // Gelecek her customer için server'ı meşgul tutma sürelerini belirliyoruz
            }
            
            
            while (currentTime < simulationTime)    // Simulasyon döngüsü
            {
                Console.WriteLine("-------");   // Her saniyeyi consoleda görmek için
                
                if (isBusy)     // Server busy ise ne zaman idle'a dönme durumunu kontol et
                {
                    if (currentTime >= busyFinishTime)
                    {
                        Console.WriteLine("CUSTOMER GONE " + currentTime);
                        isBusy = false;
                    }
                }
                else if (queue.Count != 0)    // Server busy değilse ve sırada bekleyen varsa sıradakini al
                {
                    isBusy = true;
                    
                    int busyTime = random.Next(2, 4);   //Server kaç saniye meşgul kalacak
                    busyFinishTime = currentTime + busyTime;   // Server işini ne zaman bitirecek
                    totalNonemptyQueueTime+= busyTime ;  // toplam server meşgul süresine ekle
                    
                    queue.Remove(queue[0]);     // Sıradan alına customer'ı sıradan çıkartma
                }

                if (arriveTimesForCustomers.Any(arriveTime => arriveTime == currentTime ) && currentTime != 0)  // Current time'da arrive olan customer var mı kontrolü
                {
                    Console.WriteLine("CUSTOMER ARRİVED " + currentTime);
                    Customer customer = new Customer();     //Yeni gelen customer
                    
                    if (isBusy)     // Yeni customer geldiğinde server meşgul mü kontrolü
                    {
                        queue.Add(customer);    // Server busy ise customerı queue'ya koy
                    }
                    else
                    {
                        isBusy = true;      // Server busy değildi ise artık busy

                        int busyTime = random.Next(2, 4);   //Server kaç saniye meşgul kalacak
                        busyFinishTime = currentTime + busyTime;   // Server işini ne zaman bitirecek
                        totalNonemptyQueueTime+= busyTime ;  // toplam server meşgul süresine ekle
                    }
                }
                
                currentTime++;
            }

            totalEmptyQueueTime = simulationTime - totalNonemptyQueueTime;  // Kaç saniye meşgul oldu ise onu toplam süreden çıkarıp meşgul olmadığı zamanı bulma
            
            Console.WriteLine($"\t -------------SIMULATION FINISHED-----------------");
            Console.WriteLine($"Simülasyon süresi: {simulationTime} saniye");
            Console.WriteLine($"{simulationTime} saniyelik simülasyon süresi boyunca toplam {totalCustomerNumber} customer geldi");
            Console.WriteLine($"Server, toplam {totalEmptyQueueTime} saniye boyunca sırada kimse yoktu.");
            Console.WriteLine($"Server, toplam {totalNonemptyQueueTime} saniye boyunca sıra doluydu.");

            P0 = (float) totalEmptyQueueTime / simulationTime;
            P1 = (float) totalNonemptyQueueTime / simulationTime;
            
            Console.WriteLine($"PO = {P0} ");
            Console.WriteLine($"P1 = {P1} ");
        }
    }

    public class Customer
    {
        
    }
}