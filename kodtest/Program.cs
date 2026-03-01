// Frenda kodtest
// Uppgift 1 - Uppvärmning 
using System.Text;


// Avgör om tal är en tvåpotens
bool IsPowerOfTwo(int n)
{
    return n > 0 && (n & (n - 1)) == 0;
}

string ReverseString(string s)
{
    var reversed = new StringBuilder();
    for (int i = s.Length - 1; i >= 0; i--)
    {
        reversed.Append(s[i]);
    }
    
    return reversed.ToString();
}

string ReverseStringCharArray(string s)
{
    char[] charArray = s.ToCharArray();
    Array.Reverse(charArray);
    
    return new string(charArray);
}

string RepeatString(string s, int antal)
{
    var str = new StringBuilder();
    for (int i = 0; i < antal; i++)
    {  
        str.Append(s); 
    }
    
    return str.ToString();
}

void WriteOddIntegers(int n, int m)
{
    int count = 0;
    int start = Math.Min(n, m);
    int end = Math.Max(n, m);
    
    for (int i = start; i <= end; i++)
    {
        if (i % 2 != 0)
        {
            Console.WriteLine(i);
            count++;
        }
    }
}


// --------------------------------------------------
// Inmatning och test
// --------------------------------------------------

var now = DateTime.Now;
Console.WriteLine($"Frenda kodtest {now.ToShortDateString()}");

bool cont = true;

// Mata in heltal
while (cont)
{
    int heltal;
    while (true)
    {
        Console.WriteLine();
        Console.Write("Mata in ett heltal: ");
        string? input = Console.ReadLine();
        
        if (int.TryParse(input, out heltal))
        {
            break;
        }
        else
        {
            Console.WriteLine("Ogiltigt heltal, försök igen.");
        }
    }

    Console.WriteLine($"Talet är en tvåpotens: {IsPowerOfTwo(heltal)}");
    
    Console.Write("Vill du mata in ytterligare ett tal? (j/n): ");
    string? svar = Console.ReadLine()?.ToLower();
    cont = svar == "j" || svar == "ja";
}

// Stränghantering
bool contString = true;
int antal;
while (contString)
{
    Console.WriteLine();
    Console.Write("Mata in en sträng: ");
    var myString = Console.ReadLine();
    if (!string.IsNullOrEmpty(myString))
    {
        Console.WriteLine();
        Console.WriteLine("Strängen i omvänd ordning: " + ReverseString(myString));

        Console.Write("Hur många gåner vill du upprepa strängen? ");
        string? input = Console.ReadLine();
        if (int.TryParse(input, out antal))
        {
            Console.WriteLine(RepeatString(myString, antal));
        }
        else
        {
            Console.WriteLine("Ogiltigt heltal, försök igen.");
        }
    }
    else
    {
        Console.WriteLine("Strängen var tom!");
    }

    Console.Write("Vill du mata in ytterligare en sträng? (j/n): ");
    string? svar = Console.ReadLine()?.ToLower();
    contString = svar == "j" || svar == "ja";
}

Console.WriteLine();

// Skriv ut udda tal mellan 0 och 100
Console.WriteLine("Udda tal mellan 0 och 100:");
WriteOddIntegers(0, 100);
