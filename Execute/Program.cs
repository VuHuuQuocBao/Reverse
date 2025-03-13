using Compiler.Core;
using Compiler.Enums;
using Core.Core;
using Core.Core.Visitor;

var path = @"D:\LastDance\Reverse\Test\source.txt";
var source = Extension.Scan(path);

var scanner = new Scanner();
var tokens = scanner.ScanTokens(source);

/*var generator = new AstTreeGenerator();
generator.DefineAst("D:\\LastDance\\Reverse\\Core\\Core", "Expression", new List<string>()
{
     "Binary   : Expression left, Token @operator, Expression right",
     "Grouping : Expression expression",
     "Literal  : Object value",
     "Unary    : Token @operator, Expression right"
});*/

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