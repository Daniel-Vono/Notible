namespace Notible;

public class Helper
{
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
}