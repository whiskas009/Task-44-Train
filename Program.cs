using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Task_44_Train
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RailwayStation RailwayStation = new RailwayStation();
            Train Train = new Train();
            bool isWork = true;

            while (isWork == true)
            {
                Console.Clear();
                RailwayStation.RenderInfo();
                Console.WriteLine("\n1. Создать направление \n2. Продать билеты \n3. Сформировать поезд \n4. Отправить поезд \n5. Выход");

                switch (Console.ReadLine())
                {
                    case "1":
                        RailwayStation.CreateDirection();
                        break;

                    case "2":
                        RailwayStation.SellTickets();
                        break;

                    case "3":
                        Train.FormTrain(RailwayStation.NumberPassengers);
                        break;

                    case "4":
                        RailwayStation.SendTrain();
                        break;

                    case "5":
                        isWork = false;
                        break;


                    default:
                        Console.WriteLine("\nНеккоректный ввод\n");
                        break;
                }  
            }
        }
    }

    class Wagon
    {
        public int NumberPlace { get; private set; }

        public Wagon(int length)
        {
            NumberPlace = length;
        }
    }

    class RailwayStation
    {
        private Train _train = new Train();
        private string _departureStation = "";
        private string _destinationStation = "";
        public int NumberPassengers { get; private set; }

        public RailwayStation()
        {
            NumberPassengers = 0;
        }

        public void CreateDirection()
        {
            if (NumberPassengers == 0)
            {
                Console.WriteLine("\nВведите станцию отправления: \n");
                _departureStation = Console.ReadLine();

                Console.WriteLine("\nВведите станцию назначения: \n");
                _destinationStation = Console.ReadLine();

                if (_departureStation.ToLower() == _destinationStation.ToLower())
                {
                    Console.WriteLine("\nСтанция отправления не должна совпадать со станцией назначения\n");
                    Thread.Sleep(1000);
                    _departureStation = "";
                    _destinationStation = "";
                }
            }
            else
            {
                Console.WriteLine("\nНа данное направление уже проданы билеты! Сформируйте и отправьте поезд\n");
                Thread.Sleep(2000);
            }
        }

        public void RenderInfo()
        {
            if (_departureStation != "")
            {
                Console.WriteLine($"Назначение: {_departureStation} - {_destinationStation}\n");
                Console.WriteLine($"Колличество пассажиров: {NumberPassengers}");
                if (_train.GetTrainLength() != 0)
                {
                    Console.WriteLine("\nСостав поезда: \n");
                    _train.RenderTrain();
                }
                else
                {
                    Console.WriteLine("\nСостав поезда не сформирован\n");
                }
            }
            else
            {
                Console.WriteLine("\nНаправление не выбрано\n");
            }
        }

        public void SellTickets()
        {
            if (NumberPassengers == 0)
            {
                if (_departureStation != "")
                {
                    Random Random = new Random();
                    int minNumberPassengers = 0;
                    int maxNumberPassengers = 100;
                    NumberPassengers = Random.Next(minNumberPassengers, maxNumberPassengers);
                    Console.WriteLine($"\nПродано {NumberPassengers} билетов!\n");
                }
                else
                {
                    Console.WriteLine("\nСоздайте направление, чтобы продать билеты\n");
                    Thread.Sleep(1000);
                }
            }
            else
            {
                Console.WriteLine("\nБилеты уже проданы! Сформируйте и отправьте поезд\n");
                Thread.Sleep(1000);
            }
        }

        public void SendTrain()
        {
            if (_train.GetTrainLength() != 0)
            {
                _train.DisbandTrain();
                NumberPassengers = 0;
                _departureStation = "";
                _destinationStation = "";

                Console.WriteLine("\nПоед отправлен!\n");
                Thread.Sleep(1000);
            }
            else
            {
                Console.WriteLine("\nСформируйте поезд!\n");
                Thread.Sleep(1000);
            }
        }
    }

    class Train
    {
        private List<Wagon> _typesWagons = new List<Wagon> { new Wagon(10), new Wagon(7), new Wagon(20), new Wagon(5) };
        private List<Wagon> _train = new List<Wagon>();

        public void FormTrain(int X)
        {
            int unallocatedPassengers = 0;
            if (unallocatedPassengers != 0 && _train.Count == 0)
            {
                bool isWork = true;
                while (isWork == true)
                {
                    if (unallocatedPassengers > 0)
                    {
                        Console.WriteLine($"\nКолличество нераспределённых пассажиров: {unallocatedPassengers} \n\nВыберите вагон: ");
                        for (int i = 0; i < _typesWagons.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. Вагон на {_typesWagons[i].NumberPlace} мест");
                        }

                        bool isNumber = int.TryParse(Console.ReadLine(), out int input);
                        if (isNumber == true)
                        {
                            if (input <= _typesWagons.Count && unallocatedPassengers > 0)
                            {
                                _train.Add(_typesWagons[input - 1]);
                                unallocatedPassengers -= _typesWagons[input - 1].NumberPlace;
                                Console.WriteLine("\nВагон добавлен\n");
                            }
                            else
                            {
                                Console.WriteLine("\nДанного вагона нет в списке или пассажиры все распределены\n");
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nНеккоректный ввод, введите число\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nВсе пассажиры распределены\n");
                        Thread.Sleep(1000);
                        isWork = false;
                    }
                }
            }
            else if (_train.Count == 0)
            {
                Console.WriteLine("\nПродайте билеты, чтобы сформировать поезд\n");
                Thread.Sleep(1000);
            }
            else
            {
                Console.WriteLine("\nПоезд уже сформирован!\n");
                Thread.Sleep(1000);
            }
        }

        public int GetTrainLength()
        {
            return _train.Count;    
        }

        public void DisbandTrain()
        {
            _train.RemoveRange(0, _train.Count);
        }

        public void RenderTrain()
        {
            for (int i = 0; i < _train.Count; i++)
            {
                Console.WriteLine($"Вагон {i + 1}. Мест: {_train[i].NumberPlace}");
            }
        }
    }
}
