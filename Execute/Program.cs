using Core.Core;
using Core.Core.Visitor;
using Core.Script;

var path = @"D:\LastDance\Reverse\Test\source.txt";

var source = Extension.Scan(path);

var scanner = new Scanner();
var tokens = scanner.ScanTokens(source);

var generator = new AstTreeGenerator();

var path1 = @"D:\LastDance\Reverse\Core\Core";

generator.DefineAst(path1, "Expression", new List<string>()
{
     "Binary   : Expression Left, Token @Operator, Expression Right",
     "Grouping : Expression Expression",
     "Literal  : Object Value",
     "Unary    : Token @Operator, Expression Right",
     "Variable : Token Name",
     "Assign   : Token Name, Expression Value",
     "Logical  : Expression Left, Token @Operator, Expression Right",
     "Call     : Expression Callee, Token @Paren, List<Expression> Arguments",
     "Get      : Expression Object, Token Name",
     "Set      : Expression Object, Token Name, Expression Value",
});

generator.DefineAst(path1, "Statement", new List<string>()
{
      "BlockStatement      : List<Statement> Statements",
      "IfStatement         : Expression Condition, Statement ThenBranch, Statement ElseBranch",
      "WhileStatement      : Expression Condition, Statement Body",
      "FunctionStatement   : Token Name, List<Token> @Params, List<Statement> body",
      "ReturnStatement     : Token Keyword, Expression Value",
      "Class      : Token Name, List<FunctionStatement> Methods",
});

#region Test print Ast tree

/*Expression expression = new Binary(
            new Unary(
                new Token(TokenType.MINUS, "-", null, 1),
                new Literal(123)),
            new Token(TokenType.STAR, "*", null, 1),
            new Grouping(
                new Literal(45.67)));

Console.WriteLine(new AstPrinter().Print(expression));*/

#endregion

#region Test parse single expression

var parser = new Parser(tokens);

//var expressionParser = parser.Parse();
var statementParser = parser.ParseStatement();

/*Console.WriteLine(new AstPrinter().Print(expressionParser));*/

#endregion

#region Test interpret a single expression
/*
var interpreter = new Interpreter();
var result = interpreter.InterpretExpresion(expressionParser);
*/
#endregion

#region Test interpret statements

var interpreter = new Interpreter();
interpreter.InterpretStatement(statementParser);

var G = 1;
#endregion