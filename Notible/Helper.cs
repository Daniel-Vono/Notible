using System.Text.RegularExpressions;

namespace Notible;

public class Helper
{
    private const int _ID_LENGTH = 24;
    
    public static int GetValidIntInput()
    {
        bool validChoice;
        int choice;
        
        do
        {
            validChoice = int.TryParse(Console.ReadLine(), out choice);
            if(!validChoice) Console.WriteLine("Invalid number. Please enter a valid number.");
            
        } while (!validChoice);
        
        return choice;
    }
    
    public static int GetValidIntInput(int minInclusive, int maxExclusive)
    {
        bool validChoice;
        int choice;
        
        do
        {
            validChoice = int.TryParse(Console.ReadLine(), out choice);
            if (choice < minInclusive || choice > maxExclusive) validChoice = false;
            
            if(!validChoice) Console.WriteLine("Invalid number. Please enter a valid number.");
            
        } while (!validChoice);
        
        return choice;
    }
    
    public static bool GetValidBoolInput()
    {
        bool validChoice;
        bool choice = false;
        
        do
        {
            string input = Console.ReadLine();
            input.ToLower();

            if (input.Equals("y"))
            {
                validChoice = true;
                choice = true;
            }
            else if(input.Equals("n"))
            {
                validChoice = true;
                choice = false;
            }
            else
            {
                validChoice = false;
                Console.WriteLine("Invalid input. Please enter a valid input.");
            }
            
        } while (!validChoice);
        
        return choice;
    }

    public static string GetValidHexInput()
    {
        string number;
        bool validChoice = false;
        
        do
        {
            number = Console.ReadLine().PadLeft(_ID_LENGTH, '0');
            if (Regex.IsMatch(number, @"^[a-fA-F0-9]*$")) validChoice = true;
            
            if(!validChoice) Console.WriteLine("Invalid ID. Please enter a valid hexadecimal ID.");
            
        } while (!validChoice);
        
        return number;
    }
}