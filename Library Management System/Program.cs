using System;

namespace PrivateLibrarySystem
{
    interface ILibraryActions
    {
        void Borrow(string borrower);
        void Return();
    }

    abstract class LibraryItem : ILibraryActions
    {
        public abstract void Display();

        private string id;
        private string title;
        private bool isIssued;
        protected string borrowerName;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public bool IsIssued
        {
            get { return isIssued; }
        }
        public string Borrower
        {
            get { return borrowerName; }
        }

        public LibraryItem(string id, string title)
        {
            ID = id;
            Title = title;
            isIssued = false;
        }

        public void Borrow(string name)
        {
            if (isIssued)
                throw new Exception("Yeh book pehle hi " + borrowerName + " ke paas hai.");

            isIssued = true;
            borrowerName = name;
        }

        public void Return()
        {
            isIssued = false;
            borrowerName = null;
        }
    }

    class Book : LibraryItem
    {
        public string Author;

        public bool Issued = false;
        public string IssuedTo = "";
        public string[] Waiting = new string[10];
        public int WCount = 0;

        public Book(string id, string title, string author) : base(id, title)
        {
            Author = author;
        }

        public override void Display()
        {
            string status;

            if (Issued)
            {
                status = "[Allotted to: " + IssuedTo + "]";
            }
            else
            {
                status = "[Available]";
            }

            Console.WriteLine("ID: " + ID);
            Console.WriteLine("Title: " + Title);
            Console.WriteLine("Author: " + Author + " | " + status);

            if (WCount > 0)
            {
                Console.Write("Waiting: ");
                for (int i = 0; i < WCount; i++)
                {
                    Console.Write(Waiting[i] + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("---------------------------");
        }
    }

    class Student
    {
        public static Student Register()
        {
            Console.Write("Student ID: ");
            string id = Console.ReadLine();

            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Password: ");
            string pass = Console.ReadLine();

            Console.WriteLine("Registration Successful!");

            return new Student(id, name, pass);
        }

        public string ID;
        public string Name;
        public string MyBook = "None";

        private string Password;

        public Student(string id, string naam, string pass)
        {
            ID = id;
            Name = naam;
            Password = pass;
        }

        public bool Login()
        {
            Console.Write("Password enter karo: ");
            string inputPass = Console.ReadLine();

            if (inputPass == Password)
            {
                Console.WriteLine("Login Successful! Welcome " + Name);
                return true;
            }
            else
            {
                Console.WriteLine("Galat Password!");
                return false;
            }
        }

        public void StudentMenu(Book[] books)
        {
            while (true)
            {
                Console.WriteLine("\n-- STUDENT: " + Name + " --");
                Console.WriteLine("1. My Book");
                Console.WriteLine("2. Request Book");
                Console.WriteLine("3. Return Book");
                Console.WriteLine("4. Logout");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.WriteLine("Aapke paas yeh book hai: " + MyBook);
                }
                else if (choice == "2")
                {
                    if (MyBook != "None")
                    {
                        Console.WriteLine("Pehle wali book return karo.");
                    }
                    else
                    {
                        Console.Write("Book name: ");
                        string bname = Console.ReadLine();

                        for (int i = 0; i < books.Length; i++)
                        {
                            if (books[i] != null && books[i].Title == bname)
                            {
                                if (books[i].Issued == false)
                                {
                                    books[i].Issued = true;
                                    books[i].IssuedTo = Name;
                                    MyBook = bname;
                                    Console.WriteLine("Book issue ho gayi!");
                                }
                                else
                                {
                                    books[i].Waiting[books[i].WCount] = Name;
                                    books[i].WCount++;
                                    Console.WriteLine("Book busy hai, aap waiting list mein ho.");
                                }
                                break;
                            }
                        }
                    }
                }
                else if (choice == "3")
                {
                    if (MyBook == "None")
                    {
                        Console.WriteLine("Aapke paas koi book nahi hai.");
                    }
                    else
                    {
                        for (int i = 0; i < books.Length; i++)
                        {
                            if (books[i] != null && books[i].Title == MyBook)
                            {
                                books[i].Return();
                                break;
                            }
                        }
                        MyBook = "None";
                        Console.WriteLine("Book wapis jama ho gayi!");
                    }
                }
                else if (choice == "4")
                {
                    Console.WriteLine("Logout ho gaya!");
                    break;
                }
            }
        }
    }

    class Admin
    {
        private string adminUser = "admin";
        private string adminPass = "123";

        public bool Login()
        {
            Console.Write("Admin Username: ");
            string u = Console.ReadLine();

            Console.Write("Admin Password: ");
            string p = Console.ReadLine();

            if (u == adminUser && p == adminPass)
            {
                Console.WriteLine("Admin Login Successful!");
                return true;
            }
            else
            {
                Console.WriteLine("Wrong Admin Credentials!");
                return false;
            }
        }

        public void AddBook(Book[] books, ref int count)
        {
            Console.Write("Book ID: ");
            string id = Console.ReadLine();

            Console.Write("Name: ");
            string t = Console.ReadLine();

            Console.Write("Author: ");
            string a = Console.ReadLine();

            books[count] = new Book(id, t, a);
            count = count + 1;

            Console.WriteLine("Book stock mein shamil ho gayi!");
        }

        public void ViewAllBooks(Book[] books, int count)
        {
            Console.WriteLine("\n--- COMPLETE BOOK INVENTORY ---");
            for (int i = 0; i < count; i++)
            {
                books[i].Display();
            }
        }

        public void AllotBook(Book[] books, int bCount, Student[] students, int sCount)
        {
            Console.Write("Kaunsi Book? (Enter ID): ");
            string bid = Console.ReadLine();

            Console.Write("Kis Student ko? (Enter ID): ");
            string sid = Console.ReadLine();

            Book b = null;
            Student s = null;

            for (int i = 0; i < bCount; i++)
                if (books[i].ID == bid) b = books[i];

            for (int i = 0; i < sCount; i++)
                if (students[i].ID == sid) s = students[i];

            if (b != null && s != null)
            {
                try
                {
                    b.Borrow(s.Name);
                    s.MyBook = b.Title;

                    Console.WriteLine("Kaam ho gaya! Book student ko de di gayi.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Ghalti: ID sahi nahi hai!");
            }
        }

        public void ViewStatus(Book[] books, int bCount, int sCount)
        {
            int baharGayiBooks = 0;
            for (int i = 0; i < bCount; i++)
                if (books[i].IsIssued) baharGayiBooks++;

            Console.WriteLine("\n--- LIBRARY KI HALAT ---");
            Console.WriteLine("Total Books: " + bCount);
            Console.WriteLine("Total Students: " + sCount);
            Console.WriteLine("Issued Books: " + baharGayiBooks);
        }
    }

    class LibraryData
    {
        public Book[] Books = new Book[100];
        public Student[] Students = new Student[50];
        public int TotalBooks = 0;
        public int TotalStudents = 0;
    }

    class Program
    {
        static LibraryData data = new LibraryData();

        static void Main()
        {
            Admin admin = new Admin();
            AddInitialData();

            while (true)
            {
                Console.WriteLine("\n=== PRIVATE LIBRARY SYSTEM ===");
                Console.WriteLine("1. Admin Panel");
                Console.WriteLine("2. Student Portal");
                Console.WriteLine("3. Student Registration");
                Console.WriteLine("4. Exit");

                string role = Console.ReadLine();

                if (role == "1")
                {
                    if (admin.Login())
                    {
                        while (true)
                        {
                            Console.WriteLine("\n1. Add Book");
                            Console.WriteLine("2. Show All");
                            Console.WriteLine("3. Give Book");
                            Console.WriteLine("4. Stats");
                            Console.WriteLine("5. Logout");

                            string op = Console.ReadLine();

                            if (op == "1")
                                admin.AddBook(data.Books, ref data.TotalBooks);
                            else if (op == "2")
                                admin.ViewAllBooks(data.Books, data.TotalBooks);
                            else if (op == "3")
                                admin.AllotBook(data.Books, data.TotalBooks, data.Students, data.TotalStudents);
                            else if (op == "4")
                                admin.ViewStatus(data.Books, data.TotalBooks, data.TotalStudents);
                            else if (op == "5")
                            {
                                Console.WriteLine("Admin Logout ho gaya!");
                                break;
                            }
                        }
                    }
                }
                else if (role == "2")
                {
                    Console.Write("Login (Enter Student ID): ");
                    string sid = Console.ReadLine();

                    Student foundStudent = null;

                    for (int i = 0; i < data.TotalStudents; i++)
                        if (data.Students[i].ID == sid) foundStudent = data.Students[i];

                    if (foundStudent != null)
                    {
                        if (foundStudent.Login())
                        {
                            foundStudent.StudentMenu(data.Books);
                        }
                    }
                    else
                        Console.WriteLine("Yeh ID mojood nahi hai.");
                }
                else if (role == "3")
                {
                    if (data.TotalStudents < data.Students.Length)
                    {
                        data.Students[data.TotalStudents] = Student.Register();
                        data.TotalStudents++;
                    }
                    else
                    {
                        Console.WriteLine("Student limit full ho chuki hai.");
                    }
                }
                else if (role == "4")
                {
                    break;
                }
            }
        }

        static void AddInitialData()
        {
            data.Books[0] = new Book("B1", "OOP Concepts", "Robert");
            data.Books[1] = new Book("B2", "C# Programming", "Microsoft");
            data.Books[2] = new Book("B3", "Data Structures", "Adam");
            data.Books[3] = new Book("B4", "Database Systems", "Elmasri");
            data.Books[4] = new Book("B5", "Software Engineering", "Sommerville");
            data.Books[5] = new Book("B6", "Operating Systems", "Silberschatz");
            data.Books[6] = new Book("B7", "Computer Networks", "Tanenbaum");
            data.Books[7] = new Book("B8", "Web Development", "Jon Duckett");
            data.Books[8] = new Book("B9", "Python Basics", "Guido");
            data.Books[9] = new Book("B10", "Artificial Intelligence", "Russell");
            data.TotalBooks = 10;

            data.Students[0] = new Student("S1", "Ali", "111");
            data.Students[1] = new Student("S2", "Sara", "222");
            data.Students[2] = new Student("S3", "Ahmed", "333");
            data.Students[3] = new Student("S4", "Zainab", "444");
            data.Students[4] = new Student("S5", "Hamza", "555");
            data.Students[5] = new Student("S6", "Danish", "666");
            data.Students[6] = new Student("S7", "Fatima", "777");
            data.Students[7] = new Student("S8", "Bilal", "888");
            data.Students[8] = new Student("S9", "Ayesha", "999");
            data.Students[9] = new Student("S10", "Umer", "000");
            data.TotalStudents = 10;
        }
    }
}