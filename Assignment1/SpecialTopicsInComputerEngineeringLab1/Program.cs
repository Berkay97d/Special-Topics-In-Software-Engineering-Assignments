using System;
using System.Collections;
using System.Collections.Generic;

namespace SpecialTopicsInComputerEngineeringLab1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Server server = new Server();
            UserSender userSender = new UserSender();
            
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Spacebar) // SPACE TUŞU YENİ USER YOLLAR
                    {
                        userSender.SendRandomUser(server);
                    }
                    else if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        break; // ESC PROGRAMI KAPAR
                    }
                }
            }


        }
    }
    
    public class Server
    {
        private float m_MasterClock;
        private bool m_IsBusy;
        private int m_numOfCustomer;
        private List<User> m_Queue = new List<User>();
        
        
        public void OnUserArrived(User user) //FLOWCHART LOGIC'I ON USER ARRIVED FONKSIYONUNDADIR
        {
            m_numOfCustomer++;
            
            Console.WriteLine("User " + user.GetId() + " is arrived, current number of user is " + m_numOfCustomer);
            
            if (!m_IsBusy)
            {
                ServiceToUser(user);
                return;
            }

            PutUserToQueue(user);
        }
        
        private User GetNextUserFromQueue() //SERVER BUSY OLMAYI BIRAKINCA SIRADAKİ KİSİYE GECER
        {
            var nextUser = m_Queue[0];
            m_Queue.Remove(nextUser);
            return nextUser;
        }

        private void PutUserToQueue(User user) //SERVER DOLU ISE SIRAYA KOYAR
        {
            m_Queue.Add(user);
            
            Console.WriteLine("User " + user.GetId() + " is putted to queue cause server is bussy");
        }

        private void DepartureUser(User user) //ISI BITEN USERI YOLLAR 
        {
            m_numOfCustomer--;
            
            Console.WriteLine("User " + user.GetId() + " is departured, current number of user is " + m_numOfCustomer);
        }

        private void ServiceToUser(User user) //USER'A USERIN BELİRLEDDİĞİ SERVİS SÜRESİ KADAR HİZMET EDER
        {   
            DateTime baslangicZamani = DateTime.Now;
                
            Console.WriteLine("Servicing to user " + user.GetId());

            while (true)
            {
                
                m_IsBusy = true;

                DateTime bitisZamani = DateTime.Now;
                TimeSpan gecenSure = Helpers.CalculateTime(baslangicZamani, bitisZamani);

                if (gecenSure.Seconds >= user.GetServiceTime())
                {
                    Console.WriteLine("Service finished to user " + user.GetId());
                    m_IsBusy = false;
                
                    DepartureUser(user);

                    if (m_Queue.Count != 0)
                    {
                        ServiceToUser(GetNextUserFromQueue());
                        
                        return;
                    }
                    
                    Console.WriteLine("There is no user to service");
                    return;
                }
            }
        }
        
    }

    public class User
    {
        private int m_Id;
        private int m_ServiceTime;

        public User(int id, int serviceTime)
        {
            m_Id = id;
            m_ServiceTime = serviceTime;
        }

        public int GetServiceTime()
        {
            return m_ServiceTime;
        }

        public int GetId()
        {
            return m_Id;
        }
    }

    public class UserSender
    {
        public void SendRandomUser(Server server)
        {
            var randomId = Helpers.GetRandomInteger(1, 5000);
            var randomServiceTime = Helpers.GetRandomInteger(4, 4);

            User user = new User(randomId, randomServiceTime);

            server.OnUserArrived(user);
        }
    }
    
    public static class Helpers
    {
        public static int GetRandomInteger(int min, int max)
        {
            if (min > max)
            {
                throw new ArgumentException("Minimum değer maksimum değerden büyük olamaz.");
            }

            Random random = new Random();
            return random.Next(min, max + 1);
        }
        
        public static TimeSpan CalculateTime(DateTime baslangicZamani, DateTime bitisZamani)
        {
            return bitisZamani - baslangicZamani;
        }
    }
}