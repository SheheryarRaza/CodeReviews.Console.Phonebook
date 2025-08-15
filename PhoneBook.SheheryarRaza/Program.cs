using Microsoft.EntityFrameworkCore;
using PhoneBook.SheheryarRaza.Data;
using PhoneBook.SheheryarRaza.Models;

namespace ThePhoneBook
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var context = new ApplicationDBContext())
            {
                context.Database.Migrate();
            }

            Console.WriteLine("Welcome to the Phone Book Application!");
            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1. Add a new contact");
                Console.WriteLine("2. View all contacts");
                Console.WriteLine("3. Update a contact");
                Console.WriteLine("4. Delete a contact");
                Console.WriteLine("5. Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddContact();
                        break;
                    case "2":
                        ViewContacts();
                        break;
                    case "3":
                        UpdateContact();
                        break;
                    case "4":
                        DeleteContact();
                        break;
                    case "5":
                        isRunning = false;
                        Console.WriteLine("Exiting application. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        public static void AddContact()
        {
            Console.WriteLine("\n--- Add New Contact ---");
            string name, email, phoneNumber;

            Console.Write("Enter Name: ");
            name = Console.ReadLine();

            while (true)
            {
                Console.Write("Enter Email: ");
                email = Console.ReadLine();
                if (Validations.IsValidEmail(email))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid email format. Please try again.");
                }
            }

            while (true)
            {
                Console.Write("Enter Phone Number (e.g., 123-456-7890): ");
                phoneNumber = Console.ReadLine();
                if (Validations.IsValidPhoneNumber(phoneNumber))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid phone number format. Please use XXX-XXX-XXXX format.");
                }
            }
            int categoryId = GetCategoryChoice();

            var newContact = new Contact { Name = name, Email = email, PhoneNumber = phoneNumber, CategoryId = categoryId };


            using (var context = new ApplicationDBContext())
            {
                try
                {
                    context.Contacts.Add(newContact);
                    context.SaveChanges();
                    Console.WriteLine("Contact added successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while adding the contact: {ex.Message}");
                }
            }
        }

        public static void ViewContacts()
        {
            Console.WriteLine("\n--- All Contacts ---");
            using (var context = new ApplicationDBContext())
            {
                try
                {
                    var contacts = context.Contacts.Include(c => c.Category).OrderBy(c => c.Name).ToList();
                    if (contacts.Any())
                    {
                        foreach (var contact in contacts)
                        {
                            Console.WriteLine($"Id: {contact.Id}, Name: {contact.Name}, Email: {contact.Email}, Phone: {contact.PhoneNumber}, Category: {contact.Category?.Name ?? "N/A"}");

                        }
                    }
                    else
                    {
                        Console.WriteLine("No contacts found.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while retrieving contacts: {ex.Message}");
                }
            }
        }

        public static void UpdateContact()
        {
            Console.WriteLine("\n--- Update Contact ---");
            ViewContacts();

            Console.Write("Enter the Id of the contact to update: ");
            if (!int.TryParse(Console.ReadLine(), out int contactId))
            {
                Console.WriteLine("Invalid Id. Please enter a number.");
                return;
            }

            using (var context = new ApplicationDBContext())
            {
                try
                {
                    var contactToUpdate = context.Contacts.Find(contactId);
                    if (contactToUpdate == null)
                    {
                        Console.WriteLine("Contact not found.");
                        return;
                    }

                    Console.WriteLine($"Current details for Id {contactToUpdate.Id}:");
                    Console.WriteLine($"Name: {contactToUpdate.Name}, Email: {contactToUpdate.Email}, Phone: {contactToUpdate.PhoneNumber}");

                    Console.Write("Enter new Name (leave blank to keep current): ");
                    string newName = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newName))
                    {
                        contactToUpdate.Name = newName;
                    }

                    string newEmail = "";
                    while (true)
                    {
                        Console.Write("Enter new Email (leave blank to keep current): ");
                        newEmail = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(newEmail) || Validations.IsValidEmail(newEmail))
                        {
                            if (!string.IsNullOrWhiteSpace(newEmail))
                            {
                                contactToUpdate.Email = newEmail;
                            }
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid email format. Please try again.");
                        }
                    }

                    string newPhoneNumber = "";
                    while (true)
                    {
                        Console.Write("Enter new Phone Number (leave blank to keep current): ");
                        newPhoneNumber = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(newPhoneNumber) || Validations.IsValidPhoneNumber(newPhoneNumber))
                        {
                            if (!string.IsNullOrWhiteSpace(newPhoneNumber))
                            {
                                contactToUpdate.PhoneNumber = newPhoneNumber;
                            }
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid phone number format. Please use XXX-XXX-XXXX format.");
                        }
                    }
                    int newCategoryId = GetCategoryChoice();
                    contactToUpdate.CategoryId = newCategoryId;

                    context.SaveChanges();
                    Console.WriteLine("Contact updated successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while updating the contact: {ex.Message}");
                }
            }
        }

        public static void DeleteContact()
        {
            Console.WriteLine("\n--- Delete Contact ---");
            ViewContacts();

            Console.Write("Enter the Id of the contact to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int contactId))
            {
                Console.WriteLine("Invalid Id. Please enter a number.");
                return;
            }

            using (var context = new ApplicationDBContext())
            {
                try
                {
                    var contactToDelete = context.Contacts.Find(contactId);
                    if (contactToDelete == null)
                    {
                        Console.WriteLine("Contact not found.");
                        return;
                    }

                    context.Contacts.Remove(contactToDelete);
                    context.SaveChanges();
                    Console.WriteLine("Contact deleted successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while deleting the contact: {ex.Message}");
                }
            }
        }

        private static int GetCategoryChoice()
        {
            using (var context = new ApplicationDBContext())
            {
                var categories = context.Categories.ToList();
                Console.WriteLine("\nSelect a category:");
                foreach (var category in categories)
                {
                    Console.WriteLine($"{category.Id}. {category.Name}");
                }

                int categoryId;
                while (true)
                {
                    Console.Write("Enter category Id: ");
                    if (int.TryParse(Console.ReadLine(), out categoryId) && categories.Any(c => c.Id == categoryId))
                    {
                        return categoryId;
                    }
                    Console.WriteLine("Invalid category Id. Please select from the list.");
                }
            }
        }
    }
}