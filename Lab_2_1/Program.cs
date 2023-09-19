using Lab_2_1.BaseClass;
using System.Linq;

namespace Lab_2_1
{
    internal class Program
    {
        static BankAccount? SelectAccount(List<BankAccount> accounts)
        {
            BankAccount? selectedAccount = null;

            Console.WriteLine("Выберите счёт:");

            for (int i = 0; i < accounts.Count; i++)
                Console.WriteLine($"{i + 1}. {accounts[i].Id}: {accounts[i].Money}$ Статус: {accounts[i].Status}");

            if (!int.TryParse(Console.ReadLine(), out int choiceValue))
            {
                Console.WriteLine("Вы ввели не число, возврат");

                return selectedAccount;
            }

            if (choiceValue < 1 || choiceValue > accounts.Count)
            {
                Console.WriteLine("Выбран несуществующий счёт, возврат");

                return selectedAccount;
            }

            return accounts[choiceValue - 1];
        }

        static int MenuSelect()
        {
            Console.WriteLine("""
                1. Сбросить данные пользователя
                2. Добавить банковкий счёт
                3. Внести деньги на банковкий счёт
                4. Получить общую сумму по счетам
                5. Изменить статус счёта
                6. Сортировка
                0. Выйти
                """);

            if(int.TryParse(Console.ReadLine(), out int choiceValue))
                return choiceValue;

            return -1;
        }

        static void Main(string[] args)
        {
            Func<List<BankAccount>, double> calcFunc = (accounts) =>
            {
                double totalSum = .0;

                foreach (BankAccount account in accounts)
                    if (account.Status != AccountStatus.Banned)
                        totalSum += account.Money;

                return totalSum;
            };

            int choice = 1;

            Client currentClient = new();

            do
            {
                choice = MenuSelect();

                if (choice == -1)
                    continue;

                switch (choice)
                {
                    case 1:
                        currentClient = new Client();

                        Console.WriteLine("Данные очищены!");

                        break;

                    case 2:
                        currentClient.AddBankAccount(new BankAccount());

                        Console.WriteLine($"Создан новый счёт!");

                        break;

                    case 3:
                        if(currentClient.BankAccounts.Count == 0)
                        {
                            Console.WriteLine("У данного аккаунта нет счетов в банке!");
                            break;
                        }

                        BankAccount? selectedAcc = selectedAcc = SelectAccount(currentClient.BankAccounts);

                        if (selectedAcc is null)
                            break;

                        Console.WriteLine("Введите сумму: ");

                        if (!double.TryParse(Console.ReadLine(), out double sum))
                        {
                            Console.WriteLine("Вы ввели не число, возврат");

                            break;
                        }
                            
                        if (sum < 0)
                        {
                            Console.WriteLine("Вы ввели отриц. число!");

                            break;
                        }

                        selectedAcc.AddMoney(sum);

                        break;


                    case 4:
                        Console.WriteLine($"Сумма по всем счетам: {currentClient.GetTotalMoney(calcFunc)}");

                        break;

                    case 5:

                        selectedAcc = SelectAccount(currentClient.BankAccounts);

                        if (selectedAcc is null)
                            break;

                        selectedAcc.ToggleBan();

                        Console.WriteLine($"Новый статус: {selectedAcc.Status}");

                        break;

                    case 6:

                        Func<object?, (IEnumerable<BankAccount>, IEnumerable<BankAccount>)> orderFunction = (accounts) =>
                        {
                            List<BankAccount> list = (List<BankAccount>)accounts!;

                            var orderedAsc = list.OrderBy((acc) => acc.Money);

                            var orderedDesc = orderedAsc.Reverse();

                            return (orderedAsc, orderedDesc);
                        };

                        Task<(IEnumerable<BankAccount>, IEnumerable<BankAccount>)> sortTask = Task.Factory.StartNew(orderFunction, currentClient.BankAccounts);

                        sortTask.Wait();

                        Console.WriteLine("Отсортированный по возрастанию список аккаунтов:");

                        foreach(var acc in sortTask.Result.Item1)
                            Console.WriteLine(acc);

                        Console.WriteLine();

                        Console.WriteLine("Отсортированный по убыванию список аккаунтов:");

                        foreach (var acc in sortTask.Result.Item2)
                            Console.WriteLine(acc);

                        break;
                }

                Console.WriteLine("Нажмите любую кнопку для продолжения...");

                Console.ReadKey();

                Console.Clear();

            } while(choice != 0);
        }
    }
}