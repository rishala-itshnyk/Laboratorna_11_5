using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public struct Abonent
{
    public string LastName;
    public string Address;
    public double Debt;
    public Telephone Phone;
    public bool AddressChanged;
}

public struct Telephone
{
    public string Number;
    public string Operator;
}

public class Program
{
    static void Main()
    {
        string fileName;
        List<Abonent> abonents = new List<Abonent>();
        List<Telephone> phones = new List<Telephone>();

        Console.Write("Введіть ім'я файлу: ");
        fileName = Console.ReadLine();

        Read(fileName, out abonents, out phones);

        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Додати абонента");
            Console.WriteLine("2. Замінити номер телефону");
            Console.WriteLine("3. Показати список абонентів");
            Console.WriteLine("4. Визначити прізвища абонентів з заборгованістю більше заданої суми");
            Console.WriteLine("5. Змінити адресу абонента");
            Console.WriteLine("6. Вилучити абонентів, адреса яких змінилася");
            Console.WriteLine("7. Вийти");

            Console.Write("Виберіть дію (1-7): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Add(abonents, phones);
                    break;
                case "2":
                    Replace(abonents);
                    break;
                case "3":
                    ShowList(abonents);
                    break;
                case "4":
                    FilterByDebt(abonents);
                    break;
                case "5":
                    ChangeAddress(abonents);
                    break;
                case "6":
                    RemoveByAddressChange(abonents);
                    break;
                case "7":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Некоректний вибір. Спробуйте ще раз.");
                    break;
            }
        }

        Write(fileName, abonents, phones);
    }

    public static void ChangeAddress(List<Abonent> abonents)
    {
        Console.Write("Введіть прізвище абонента, для якого потрібно змінити адресу: ");
        string lastName = Console.ReadLine();

        int index = abonents.FindIndex(a => a.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase));

        if (index != -1)
        {
            Console.Write($"Поточна адреса для абонента {abonents[index].LastName}: {abonents[index].Address}\n");
            Console.Write("Введіть нову адресу: ");

            Abonent updatedAbonent = abonents[index];
            updatedAbonent.Address = Console.ReadLine();
            updatedAbonent.AddressChanged = true;
            abonents[index] = updatedAbonent;

            Console.WriteLine("Адресу змінено успішно.");
        }
        else
        {
            Console.WriteLine($"Абонент з прізвищем {lastName} не знайдений.");
        }
    }

    public static void FilterByDebt(List<Abonent> abonents)
    {
        Console.Write("Введіть суму заборгованості: ");
        if (double.TryParse(Console.ReadLine(), out double debtThreshold))
        {
            var filteredAbonents = abonents.Where(a => a.Debt > debtThreshold);

            if (filteredAbonents.Any())
            {
                Console.WriteLine("\nАбоненти з заборгованістю більше заданої суми:");
                foreach (var abonent in filteredAbonents)
                {
                    Console.WriteLine($"Прізвище: {abonent.LastName}, Заборгованість: {abonent.Debt}");
                }
            }
            else
            {
                Console.WriteLine("Немає абонентів з заборгованістю більше заданої суми.");
            }
        }
        else
        {
            Console.WriteLine("Некоректно введена сума заборгованості.");
        }
    }

    public static void RemoveByAddressChange(List<Abonent> abonents)
    {
        var removedAbonents = abonents.Where(a => a.AddressChanged).ToList();

        if (removedAbonents.Any())
        {
            foreach (var removedAbonent in removedAbonents)
            {
                Console.WriteLine($"Вилучений абонент: {removedAbonent.LastName}, Адреса: {removedAbonent.Address}");
            }

            abonents.RemoveAll(a => a.AddressChanged);

            Console.WriteLine($"Всі абоненти зі зміненою адресою вилучені.");
        }
        else
        {
            Console.WriteLine("Не знайдено абонентів зі зміненою адресою.");
        }
    }
    
    public static void Read(string fileName, out List<Abonent> abonents, out List<Telephone> phones)
    {
        abonents = new List<Abonent>();
        phones = new List<Telephone>();

        try
        {
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.OpenOrCreate)))
            {
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    Abonent abonent = new Abonent();
                    abonent.LastName = reader.ReadString();
                    abonent.Address = reader.ReadString();
                    abonent.Debt = reader.ReadDouble();
                    abonents.Add(abonent);

                    Telephone phone = new Telephone();
                    phone.Number = reader.ReadString();
                    phone.Operator = reader.ReadString();
                    phones.Add(phone);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при зчитуванні з файлу: {ex.Message}");
        }
    }

    public static void Write(string fileName, List<Abonent> abonents, List<Telephone> phones)
    {
        try
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                for (int i = 0; i < abonents.Count; i++)
                {
                    writer.Write(abonents[i].LastName);
                    writer.Write(abonents[i].Address);
                    writer.Write(abonents[i].Debt);

                    writer.Write(phones[i].Number);
                    writer.Write(phones[i].Operator);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при запису в файл: {ex.Message}");
        }
    }

    public static void Add(List<Abonent> abonents, List<Telephone> phones)
    {
        Abonent newAbonent = new Abonent();
        Telephone newPhone = new Telephone();

        Console.Write("Введіть прізвище нового абонента: ");
        newAbonent.LastName = Console.ReadLine();

        Console.Write("Введіть адресу нового абонента: ");
        newAbonent.Address = Console.ReadLine();

        Console.Write("Введіть заборгованість по оплаті нового абонента: ");
        if (double.TryParse(Console.ReadLine(), out double debt))
        {
            newAbonent.Debt = debt;
        }
        else
        {
            Console.WriteLine("Некоректно введена заборгованість. Додавання абонента скасовано.");
            return;
        }

        Console.Write("Введіть номер телефону нового абонента: ");
        newPhone.Number = Console.ReadLine();

        Console.Write("Введіть оператора телефону нового абонента: ");
        newPhone.Operator = Console.ReadLine();

        abonents.Add(newAbonent);
        phones.Add(newPhone);
    }

    public static void Replace(List<Abonent> abonents)
    {
        Console.Write("Введіть прізвище абонента, для якого потрібно замінити номер телефону: ");
        string lastName = Console.ReadLine();

        int index = abonents.FindIndex(a => a.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase));

        if (index != -1)
        {
            Console.Write($"Поточний номер телефону для абонента {abonents[index].LastName}: {abonents[index].Phone.Number}\n");
            Console.Write("Введіть новий номер телефону: ");

            string newPhoneNumber = Console.ReadLine();

            Abonent updatedAbonent = abonents[index];
            updatedAbonent.Phone.Number = newPhoneNumber;
            abonents[index] = updatedAbonent;

            Console.WriteLine("Номер телефону замінено успішно.");
        }
        else
        {
            Console.WriteLine($"Абонент з прізвищем {lastName} не знайдений.");
        }
    }

    public static void ShowList(List<Abonent> abonents)
    {
        Console.WriteLine("\nСписок абонентів:");

        foreach (var abonent in abonents)
        {
            Console.WriteLine($"Прізвище: {abonent.LastName}, Адреса: {abonent.Address}, Заборгованість: {abonent.Debt}, Номер телефону: {abonent.Phone.Number}, Оператор: {abonent.Phone.Operator}");
        }
    }
}
