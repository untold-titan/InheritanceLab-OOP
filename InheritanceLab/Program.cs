using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InheritanceLab
{

    abstract class Employee
    {
        public string id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string phone { get; set; }

        public string sin { get; set; }

        public string dob { get; set; }
        public string dept { get; set; }

        public Employee() { }
        public Employee(string id, string name, string address, string phone, string sin, string dob, string dept)
        {
            this.id = id;
            this.name = name;
            this.address = address;
            this.phone = phone;
            this.sin = sin;
            this.dob = dob;
            this.dept = dept;
        }

        public abstract double getPay();

        public override string ToString()
        {
            return "";
        }
    }

    class Salaried : Employee
    {
        public double salary { get; set; }
        public Salaried() { }
        public Salaried(string id, string name, string address, string phone, string sin, string dob, string dept, double salary) : base(id, name, address, phone, sin, dob, dept)
        {
            this.salary = salary;
        }
        public override double getPay()
        {
            return salary;
        }
    }

    class PartTime : Employee
    {
        public double rate { get; set; }
        public double hours { get; set; }
        public PartTime() { }
        public PartTime(string id, string name, string address, string phone, string sin, string dob, string dept, double rate, double hours) : base(id, name, address, phone, sin, dob, dept)
        {
            this.rate = rate;
            this.hours = hours;
        }

        public override double getPay()
        {
            return rate * hours; //40 Hours in one week
        }
    }

    class Wages : Employee
    {
        public double rate { get; set; }
        public double hours { get; set; }
        public Wages() { }
        public Wages(string id, string name, string address, string phone, string sin, string dob, string dept, double rate, double hours) : base(id, name, address, phone, sin, dob, dept)
        {
            this.rate = rate;
            this.hours = hours;
        }

        public override double getPay()
        {
            return rate * hours; //40 Hours in one week
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            List<Employee> employees = new();
            StreamReader reader = new StreamReader("../../../employees.txt");
            while(!reader.EndOfStream)
            {
                var data = reader.ReadLine().Split(":");
                if ((data[0][0] - '0') <= 4)
                {
                    employees.Add(new Salaried(data[0], data[1], data[2], data[3], data[4], data[5], data[6], double.Parse(data[7])));
                }
                else if ((data[0][0] - '0') <= 7)
                {
                    employees.Add(new PartTime(data[0], data[1], data[2], data[3], data[4], data[5], data[6], double.Parse(data[7]), double.Parse(data[8])));
                }
                else if ((data[0][0] - '0') <= 9)
                {
                    employees.Add(new Wages(data[0], data[1], data[2], data[3], data[4], data[5], data[6], double.Parse(data[7]), double.Parse(data[8])));
                }
            }

            double averagePay = 0;
            foreach(var employee in employees)
            {
                averagePay += employee.getPay();
            }
            Console.WriteLine("Average Employees Pay is $" + (averagePay / employees.Count));
            Wages? highestPaid = null;
            foreach(var employee in employees)
            {
                Wages wage;
                if(employee.GetType() == typeof(Wages))
                {
                    wage = (Wages)employee;
                }
                else
                {
                    continue;
                }
                if(highestPaid == null)
                {
                    highestPaid = wage;
                }
                if (highestPaid != null && wage.getPay() > highestPaid.getPay())
                {
                    highestPaid = wage;
                }
            }
            Console.WriteLine("Highest paid wage worker is " + highestPaid.name + " with $" + highestPaid.getPay());
            Salaried? lowestPaid = null;
            foreach (var employee in employees)
            {
                Salaried salary;
                if (employee.GetType() == typeof(Salaried))
                {
                    salary = (Salaried)employee;
                }
                else
                {
                    continue;
                }
                if (lowestPaid == null)
                {
                    lowestPaid = salary;
                }
                if (lowestPaid != null && salary.getPay() < lowestPaid.getPay())
                {
                    lowestPaid = salary;
                }
            }
            Console.WriteLine("Lowest paid salary worker is " + lowestPaid.name + " with $" + lowestPaid.getPay());

            double partTimers = 0;
            double wageWorkers = 0;
            double salaried = 0;
            foreach(var employee in employees)
            {
                if(employee.GetType() == typeof(Salaried))
                {
                    salaried++;
                    continue;
                }
                if (employee.GetType() == typeof(Wages))
                {
                    wageWorkers++;
                    continue;
                }
                if (employee.GetType() == typeof(PartTime))
                {
                    partTimers++;
                    continue;
                }
            }
            Console.WriteLine("Percentage of Salaried: " + Math.Round(salaried / employees.Count * 100, 1) + 
                "% Percentage of Wage Workers: " + Math.Round(wageWorkers / employees.Count * 100, 1) + 
                "% Percentage of Part-time workers: " + Math.Round(partTimers / employees.Count * 100, 1) + "%");
        }
    }
}
