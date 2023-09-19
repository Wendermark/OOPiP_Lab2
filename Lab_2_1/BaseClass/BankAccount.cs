
namespace Lab_2_1.BaseClass
{
    public enum AccountStatus
    {
        Banned,
        Normal
    }

    internal class BankAccount
    {
        public Guid Id { get; private set; }

        public double Money { get; private set; }

        public AccountStatus Status { get; private set; }

        public void ChangeStatus(AccountStatus newStatus) => Status = newStatus;

        public void AddMoney(double amount) => Money = amount;

        public void ToggleBan()
        {
            Status = Status == AccountStatus.Banned ? AccountStatus.Normal : AccountStatus.Banned;
        }

        public BankAccount()
        {
            Id = Guid.NewGuid();

            Money = .0;

            Status = AccountStatus.Normal;
        }

        public override string ToString()
        {
            return $"ID: {Id} Money: {Money} Status: {Status}";
        }
    }
}
