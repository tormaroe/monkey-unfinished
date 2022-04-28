using Monkey.Lexing;
using Monkey.Parsing;

const string PROMPT = ">> ";

Console.WriteLine("Hello, Monkeys!");

if (args.Length < 1)
{
    Console.WriteLine("Specify operation, one of: lex, parse, eval");
    return;
}

Monkey.Evaluation.Environment env = new();
Action<string>? eval = null;
switch (args[0])
{
    case "lex": eval = Lex; break;
    case "parse": eval = Parse; break;
    case "eval": eval = Eval; break;
    default:
        Console.WriteLine("Unknown evaluation mode!");
        return;
}

if(args.Length > 1)
{
    var source = File.ReadAllText(args[1]);
    eval(source);
}
else
{
    InteractiveLoop(eval);
}

return;

/************************** helper functions *********************************/

void InteractiveLoop(Action<string> eval)
{
    Console.WriteLine("Feel free to type in commands");
    while (true)
    {
        eval(Read());
        Console.WriteLine();
    }
}

string Read()
{
    Console.Write(PROMPT);
    var line = Console.ReadLine();
    Console.WriteLine();
    return line ?? String.Empty;
}

/******************************************************************************
    ██╗     ███████╗██╗  ██╗                                           
    ██║     ██╔════╝╚██╗██╔╝                                           
    ██║     █████╗   ╚███╔╝                                            
    ██║     ██╔══╝   ██╔██╗                                            
    ███████╗███████╗██╔╝ ██╗                                           
    ╚══════╝╚══════╝╚═╝  ╚═╝     
*******************************************************************************/
void Lex(string input)
{
    Lexer l = new(input);

    Token t;
    do
    {
        t = l.NextToken();
        Console.WriteLine(t);
    } while (t.TokenType != TokenType.EOF);
}

/******************************************************************************
    ██████╗  █████╗ ██████╗ ███████╗███████╗                           
    ██╔══██╗██╔══██╗██╔══██╗██╔════╝██╔════╝                           
    ██████╔╝███████║██████╔╝███████╗█████╗                             
    ██╔═══╝ ██╔══██║██╔══██╗╚════██║██╔══╝                             
    ██║     ██║  ██║██║  ██║███████║███████╗                           
    ╚═╝     ╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝╚══════╝     
*******************************************************************************/
void Parse(string input)
{
    Parser parser = new(new Lexer(input));
    var ast = parser.ParseProgram();

    if (parser.Errors.Count > 0)
    {
        Console.WriteLine($"{parser.Errors.Count} parse error(s):");
        parser.Errors.ForEach(Console.WriteLine);
        return;
    }

    Console.WriteLine(ast);
}

/******************************************************************************
    ███████╗██╗   ██╗ █████╗ ██╗     ██╗   ██╗ █████╗ ████████╗███████╗
    ██╔════╝██║   ██║██╔══██╗██║     ██║   ██║██╔══██╗╚══██╔══╝██╔════╝
    █████╗  ██║   ██║███████║██║     ██║   ██║███████║   ██║   █████╗  
    ██╔══╝  ╚██╗ ██╔╝██╔══██║██║     ██║   ██║██╔══██║   ██║   ██╔══╝  
    ███████╗ ╚████╔╝ ██║  ██║███████╗╚██████╔╝██║  ██║   ██║   ███████╗
    ╚══════╝  ╚═══╝  ╚═╝  ╚═╝╚══════╝ ╚═════╝ ╚═╝  ╚═╝   ╚═╝   ╚══════╝
*******************************************************************************/
void Eval(string input)
{
    Parser parser = new(new Lexer(input));
    var ast = parser.ParseProgram();

    if (parser.Errors.Count > 0)
    {
        Console.WriteLine($"{parser.Errors.Count} parse error(s):");
        parser.Errors.ForEach(Console.WriteLine);
        return;
    }

    Monkey.Evaluation.Evaluator e = new();

    var result = e.Eval(ast, env);

    if (result != null)
    {
        Console.WriteLine(result.Inspect());
    }
}
